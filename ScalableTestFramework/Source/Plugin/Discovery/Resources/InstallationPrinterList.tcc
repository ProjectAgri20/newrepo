<tcx:Context xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework" xmlns:gui="http://sherman.americas.hpqcorp.net/topcat/schemas/tools/gui-automation">
  <tcx:Resources>
    <gui:Window tcx:ID="BonjourPrinterWWindow">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">Desktop</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Bonjour Printer Wizard</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Window>
    <gui:Tab tcx:ID="SysTabControl32Tab">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">SysTabControl32</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">12320</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Tab>
    <gui:TabItem tcx:ID="BonjourPrinterW_Dup0_I0XTabItem" gui:Index="0">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Bonjour Printer Wizard</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:Pane tcx:ID="BonjourPrinterW_Dup0Pane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterW_Dup0_I0XTabItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Bonjour Printer Wizard</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Text tcx:ID="SharedprintersS_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterW_Dup0Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Shared printers:</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Tree tcx:ID="SharedprintersS_Dup0Tree">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterW_Dup0Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">SysTreeView32</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Shared printers:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1000</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Tree>
    <gui:ScrollBar tcx:ID="VerticalScrollB_Dup0ScrollBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SharedprintersS_Dup0Tree</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Vertical Scroll Bar</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Vertical ScrollBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ScrollBar>
    <gui:Button tcx:ID="Backbysmallamou_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollB_Dup0ScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Back by small amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SmallDecrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="Backbylargeamou_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollB_Dup0ScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Back by large amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">LargeDecrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Thumb tcx:ID="Thumb_Dup0Thumb">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollB_Dup0ScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Thumb</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Thumb</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Thumb>
    <gui:Button tcx:ID="Forwardbylargea_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollB_Dup0ScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Forward by large amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">LargeIncrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="Forwardbysmalla_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.VerticalScrollB_Dup0ScrollBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Forward by small amount</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SmallIncrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="DescriptionStat_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterW_Dup0Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Description:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1033</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="LocationStatic1_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterW_Dup0Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Location:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1029</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Group tcx:ID="Printerinformat_Dup0Group">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterW_Dup0Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Printer information</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1026</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Group>
     <gui:TreeItem tcx:ID="PrinterName">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SharedprintersS_Dup0Tree</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">${BonjourServiceName}</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TreeItem>
    <gui:Text tcx:ID="Static12326Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">12326</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Button tcx:ID="NextButton12324Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Next &gt;</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">12324</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="CancelButton2Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Cancel</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">2</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="Static12327Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">12327</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:TitleBar tcx:ID="BonjourPrinterWTitleBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Bonjour Printer Wizard</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">TitleBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TitleBar>
    <gui:MenuBar tcx:ID="SystemMenuBarSyMenuBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWTitleBar</gui:ReferenceContainer>
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
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWTitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Close</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Close</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
  </tcx:Resources>
</tcx:Context>