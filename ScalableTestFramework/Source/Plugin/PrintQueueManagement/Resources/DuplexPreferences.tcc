<tcx:Context xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework" xmlns:gui="http://sherman.americas.hpqcorp.net/topcat/schemas/tools/gui-automation">
  <tcx:Resources>
    <gui:Window tcx:ID="HPOfficejetProXWindow">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">Desktop</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP Officejet Pro X576dw MFP (16.185.187.41) Properties</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Window>
    <gui:Pane tcx:ID="TreeView32770Pane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">TreeView</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Tree tcx:ID="SysTreeView3291Tree">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.TreeView32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">SysTreeView32</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">9110</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Tree>
    <gui:ComboBox tcx:ID="DuplexUnitfor2SComboBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTreeView3291Tree</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Duplex Unit (for 2-Sided Printing): </gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">9060</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ComboBox>
    <gui:List tcx:ID="DuplexUnitfor2SList">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.DuplexUnitfor2SComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboLBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Duplex Unit (for 2-Sided Printing): </gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">ListBox</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:List>
    <gui:ListItem tcx:ID="NotInstalledListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.DuplexUnitfor2SList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="InstalledListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.DuplexUnitfor2SList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:Button tcx:ID="DropDownButtonDButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.DuplexUnitfor2SComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Drop Down Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">DropDown</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:ScrollBar tcx:ID="HorizontalScrolScrollBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTreeView3291Tree</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Horizontal Scroll Bar</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Horizontal ScrollBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ScrollBar>
    <gui:Button tcx:ID="Backbysmallamou_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HorizontalScrolScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Back by small amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SmallDecrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="Backbylargeamou_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HorizontalScrolScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Back by large amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">LargeDecrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Thumb tcx:ID="Thumb_Dup0Thumb">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HorizontalScrolScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Thumb</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Thumb</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Thumb>
    <gui:Button tcx:ID="Forwardbylargea_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HorizontalScrolScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Forward by large amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">LargeIncrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="Forwardbysmalla_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HorizontalScrolScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Forward by small amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SmallIncrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:ScrollBar tcx:ID="VerticalScrollBScrollBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTreeView3291Tree</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Vertical Scroll Bar</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Vertical ScrollBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ScrollBar>
    <gui:Button tcx:ID="Backbysmallamou_Dup1Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollBScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Back by small amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SmallDecrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="Backbylargeamou_Dup1Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollBScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Back by large amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">LargeDecrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Thumb tcx:ID="Thumb_Dup1Thumb">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollBScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Thumb</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Thumb</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Thumb>
    <gui:Button tcx:ID="Forwardbylargea_Dup1Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollBScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Forward by large amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">LargeIncrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="Forwardbysmalla_Dup1Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollBScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Forward by small amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SmallIncrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:TreeItem tcx:ID="HPUniversalPrinTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTreeView3291Tree</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP Universal Printing PCL 6 Device Settings</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="FormToTrayAssigTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPUniversalPrinTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Form To Tray Assignment</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="PrinterautoseleTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Printer auto select: Letter</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="ManualFeedLetteTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Manual Feed: Letter</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray1LetterTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 1: Letter</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray2LetterTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 2: Letter</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray3NotAvailabTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 3: Not Available</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray4NotAvailabTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 4: Not Available</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray5NotAvailabTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 5: Not Available</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray6NotAvailabTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 6: Not Available</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray7NotAvailabTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 7: Not Available</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray8NotAvailabTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 8: Not Available</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray9NotAvailabTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 9: Not Available</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray10NotAvailaTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 10: Not Available</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="EnvelopeFeederN_Dup0TreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.FormToTrayAssigTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Envelope Feeder: Not Available</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="FontSubstitutioTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPUniversalPrinTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Font Substitution Table</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="ExternalFontsTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPUniversalPrinTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">External Fonts... </gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="InstallableOptiTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPUniversalPrinTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Installable Options</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="AutomaticConfigTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Automatic Configuration: Off</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="ProductClassHPTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Product Class: HP</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray3NotInstallTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 3: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray4NotInstallTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 4: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray5NotInstallTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 5: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray6NotInstallTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 6: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray7NotInstallTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 7: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray8NotInstallTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 8: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray9NotInstallTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 9: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="Tray10NotInstalTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 10: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="EnvelopeFeederN_Dup1TreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Envelope Feeder: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="DuplexUnitfor2STreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Duplex Unit (for 2-Sided Printing): </gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="AccessoryOutputTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Accessory Output Bin: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="PrinterHardDiskTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Printer Hard Disk: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="JobStorageAutomTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Job Storage: Automatic</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="SecurePrintingDTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Secure Printing: Disabled</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="JobSeparatorDisTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Job Separator: Disabled</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="MopierModeDisabTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Mopier Mode: Disabled</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="DeviceTypeAutoDTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Device Type: Auto Detect</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:TreeItem tcx:ID="PunchUnitNotInsTreeItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.InstallableOptiTreeItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Punch Unit: Not Installed</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:Button tcx:ID="OKButton1Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">OK</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="CancelButton2Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Cancel</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">2</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="ApplyButton1232Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Apply</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">12321</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Tab tcx:ID="SysTabControl32Tab">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">SysTabControl32</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">12320</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Tab>
    <gui:TabItem tcx:ID="GeneralTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">General</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="SharingTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Sharing</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="PortsTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Ports</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="AdvancedTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Advanced</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="ColorManagementTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Color Management</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="SecurityTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Security</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="DeviceSettingsTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Device Settings</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="AboutTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">About</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TitleBar tcx:ID="HPOfficejetProXTitleBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP Officejet Pro X576dw MFP (16.185.187.41) Properties</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">TitleBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TitleBar>
    <gui:MenuBar tcx:ID="SystemMenuBarSyMenuBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXTitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">System Menu Bar</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SystemMenuBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:MenuBar>
    <gui:MenuItem tcx:ID="SystemItem1MenuItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SystemMenuBarSyMenuBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">System</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Item 1</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:MenuItem>
    <gui:Button tcx:ID="CloseCloseButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXTitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Close</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Close</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
  </tcx:Resources>
</tcx:Context>