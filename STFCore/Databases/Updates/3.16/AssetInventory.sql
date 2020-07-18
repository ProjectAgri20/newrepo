USE [AssetInventory]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[License](
	[LicenseId] [uniqueidentifier] NOT NULL,
	[ServerId] [uniqueidentifier] NOT NULL,
	[Solution] [varchar](50) NOT NULL,
	[SolutionVersion] [varchar](50) NOT NULL,
	[InstallationKey] [varchar](50) NULL,
	[Seats] [int] NOT NULL,
	[ExpirationDate] [datetime] NOT NULL,
	[Owner] [varchar](50) NOT NULL,
	[ExpirationNoticeDays] [int] NOT NULL,
 CONSTRAINT [PK_Licence] PRIMARY KEY CLUSTERED 
(
	[LicenseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

---------------------------------------------------------------------------------------------------------------------------------
--10/26/15 kyoungman - Create LicenseKey Table

/****** Object:  Table [dbo].[LicenseKey]    Script Date: 10/26/2015 3:28:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LicenseKey](
	[KeyId] [uniqueidentifier] NOT NULL,
	[LicenseId] [uniqueidentifier] NOT NULL,
	[KeyName] [varchar](50) NOT NULL,
	[Key] [varchar](100) NOT NULL,
 CONSTRAINT [PK_LicenseKey] PRIMARY KEY CLUSTERED 
(
	[KeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LicenseKey]  WITH CHECK ADD  CONSTRAINT [FK_LicenseKey_Licence] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[License] ([LicenseId])
GO

ALTER TABLE [dbo].[LicenseKey] CHECK CONSTRAINT [FK_LicenseKey_Licence]
GO

---------------------------------------------------------------------------------------------------------------------------------
--10/27/15 kyoungman - Create LicenseOwner Table
/****** Object:  Table [dbo].[LicenseOwner]    Script Date: 10/27/2015 2:41:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LicenseOwner](
	[LicenseId] [uniqueidentifier] NOT NULL,
	[Contact] [varchar](50) NOT NULL,
 CONSTRAINT [PK_LicenseOwner] PRIMARY KEY CLUSTERED 
(
	[LicenseId] ASC,
	[Contact] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[LicenseOwner]  WITH CHECK ADD  CONSTRAINT [FK_LicenseOwner_License] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[License] ([LicenseId])
GO

ALTER TABLE [dbo].[LicenseOwner] CHECK CONSTRAINT [FK_LicenseOwner_License]
GO

---------------------------------------------------------------------------------------------------------------------------------
--11/4/15 Cambron - Update VirtualMachineReservation with new ManagedMachineType - default to Virtual
UPDATE VirtualMachineReservation SET OsType = 'WindowsVirtual'
