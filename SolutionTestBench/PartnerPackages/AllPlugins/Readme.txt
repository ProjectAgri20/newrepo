The PluginList.txt file located in this folder is used to determine what plugins will be enabled.  The format of this file simply maps to the information stored in the MetadataType table in the STB database.  The installer will read this file and for each line add a row to the table.  Each row must follow the format:

Name,Title,Group

Where Name is the name of the plugin, and must match the name of the plugins assembly, without the "plugin." prefix.  For example, the Printing plugin assembly is named "Plugin.Printing.dll" and an entry in this file would look like:

Printing,General Printing,<can be blank>

Note that the last field, which is the Group, can be left blank if the plugin isn't assigned to any group.  The group is used in the UI when the user pulls down the list of plugins, it will have a secondary context menu with the label of the group, where you can group multiple plugins together.

Another example that may use Group are the DSS plugins, Scan to Folder (Plugin.ScanToFolder.dll), and Scan to Email (Plugin.ScanToEmail.dll).  Lines for them would look like:

ScanToFolder,Scan To Folder,DSS
ScanToEmail,Scan To Email,DSS

The middle field is the Title, and this is used in the dropdown menu in the UI for the title of the plugin.