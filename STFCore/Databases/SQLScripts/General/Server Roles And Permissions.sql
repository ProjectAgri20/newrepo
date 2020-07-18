
/*****************************************************************************
 * Title:        Server Roles And Permissions
 * Author:       Gary Parham
 * Created Date: 2019-02-12
 * Description:  View server roles and permissions per login.
 *****************************************************************************/

SELECT sp.[Name] AS ServerPrincipal
      ,sp.[type_desc] AS LoginType
      ,CASE sp.is_disabled 
           WHEN 0 THEN 'No' 
           WHEN 1 THEN 'Yes' 
       END AS UserDisabled
      ,sp.create_date AS DateCreated
      ,sp.modify_date AS DateModified
      ,sp.default_database_name AS DefaultDB
      ,sp.default_language_name AS DefaultLang
      ,ISNULL(STUFF((SELECT ',' + CASE ssp22.[Name] WHEN 'sysadmin' THEN ssp22.[Name] + ' "Full privilages"' ELSE ssp22.[Name] END
                     FROM sys.server_principals ssp2
                     INNER JOIN sys.server_role_members ssrm2 ON ssp2.principal_id = ssrm2.member_principal_id
                     INNER JOIN sys.server_principals ssp22 ON ssrm2.role_principal_id = ssp22.principal_id
                     WHERE ssp2.principal_id = sp.principal_id
                     ORDER BY ssp2.[Name]
                     FOR XML PATH (N''), TYPE
                    ).value(N'.[1]', N'nvarchar(max)'), 1, 1, N''
                   ), 'No Roles Held'
             ) AS ListofServerRoles
      ,ISNULL(STUFF((SELECT ';' + ' Permission [' + sspm3.[permission_name] + '] is [' + CASE WHEN sspm3.state_desc = 'GRANT' THEN 'Granted]' WHEN sspm3.state_desc = 'DENY' THEN 'Denied]' END AS PermGrants
                     FROM sys.server_principals ssp3
                     INNER JOIN sys.server_permissions sspm3 ON ssp3.principal_id = sspm3.grantee_principal_id
                     WHERE sspm3.[class] = 100 AND sspm3.grantee_principal_id = sp.principal_id
                     FOR XML PATH (N''), TYPE
                    ).value(N'.[1]', N'nvarchar(max)'), 1, 1, N''
                   ), 'No Server Permissions'
             ) + ' in Server::' + @@ServerName + '' AS PermGrants
FROM sys.server_principals sp
WHERE sp.[Type] IN ('S', 'G', 'U') AND sp.[Name] NOT LIKE '##%##'
ORDER BY ServerPrincipal