USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[upd_ReserveVMsByPlatformContiguous]    Script Date: 07/17/2012 11:14:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[upd_ReserveVMsByPlatformContiguous]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[upd_ReserveVMsByPlatformContiguous]
GO

USE [EnterpriseTest]
GO

/****** Object:  StoredProcedure [dbo].[upd_ReserveVMsByPlatformContiguous]    Script Date: 07/17/2012 11:14:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[upd_ReserveVMsByPlatformContiguous] (
	@requiredRange int,
	@platform nvarchar(50),
	@sessionId varchar(50)
	)
AS
BEGIN
	DECLARE @rangeStart INT
	DECLARE	@rangeTable TABLE (RangeStart int, RangeEnd int) --This will hold the list of ranges available

	select @rangeStart = 0
	-- The table aliasing here needs some explanation.  We're only selecting from one table here, but we're doing it 4 times for each record.
	-- I started out using l and r for left and right because of the utilization of the right join which helps us find the beginning and ending
	-- of each contiguous block.  For the inner query, I couldn't use l and r again, so I used a and b.  Not sure if ANY naming convention makes
	-- much sense in this case.
	insert into @rangeTable
	select l.SortOrder as start,
		(
			select min(a.SortOrder) as SortOrder
			from 
				VirtualMachine a left outer join 
				(
					select 
						* 
					from 
						VirtualMachine 
					where 
						UsageState = 'Available' and 
						PowerState = 'Powered Off' and
						Platform = @platform and
						HoldId is null
				) b on a.SortOrder = b.SortOrder - 1
			where 
				a.UsageState = 'Available' and 
				a.PowerState = 'Powered Off' and 
				a.Platform = @platform and
				a.HoldId is null and 
				b.SortOrder is null and 
				a.SortOrder >= l.SortOrder            		
		) as [end]
	from VirtualMachine l
		left outer join 
		(
			select 
				* 
			from 
				VirtualMachine 
			where 
				UsageState = 'Available' and 
				HoldId is null and 
				PowerState = 'Powered Off' and
				Platform = @platform
		) r on r.SortOrder = l.SortOrder - 1
	where 
		l.UsageState = 'Available' and 
		l.PowerState = 'Powered Off' and 
		l.Platform = @platform and
		l.HoldId is null and 
		r.SortOrder is null;

	--Retrieve the first block of VMs that will satisfy the required number
	select 
		top 1 @rangeStart = RangeStart 
	from 
		@rangeTable 
	where 
		(RangeEnd - RangeStart) + 1 >= @requiredRange 
	order by RangeStart
	
	if (@rangeStart = 0) begin
		raiserror (N'A contiguous block of %d VMs is not available.', 16, 1, @requiredRange)
	end
	else begin
		--Reserve the required number of VMs
		update 
			VirtualMachine 
		set 
			UsageState = 'Reserved', 
			SessionId = @sessionId, 
			LastUpdated = GETDATE()
		where 
			SortOrder between @rangeStart and 
			((@rangeStart + @requiredRange) - 1)
	end
	
END


GO

