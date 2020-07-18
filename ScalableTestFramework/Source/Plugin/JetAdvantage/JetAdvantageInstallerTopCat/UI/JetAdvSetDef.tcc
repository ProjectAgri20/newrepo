<tcx:Context xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework" xmlns:gui="http://sherman.americas.hpqcorp.net/topcat/schemas/tools/gui-automation">
  <tcx:Resources>
    <gui:Window tcx:ID="HPJetAdvantageO_Dup0Window">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">Desktop</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WixStdBA</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Window>
    <gui:Window tcx:ID="HPJetAdvantageO_Dup1Window">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">MsiDialogCloseClass</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand Setup</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Window>
    <gui:Button tcx:ID="InstallButton43Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Install</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">439</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="BackButton442Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Back</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">442</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="CancelButton444Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Cancel</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">444</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Image tcx:ID="WixUI_Bmp_BanneImage">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">WixUI_Bmp_Banner</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">445</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Image>
    <gui:CheckBox tcx:ID="SetHPePrintasdeCheckBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Set 'HP ePrint' as default printer</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">449</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:CheckBox>
    <gui:Text tcx:ID="HPJetAdvantageO_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand will create a printer named 'HP ePrint'.  Check the box below to set 'HP ePrint' as the default printer.</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">11</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="SetDefaultPrint_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Set Default Printer</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">125</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="SetDefaultPrint_Dup1Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Set Default Printer</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">450</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Static452Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">452</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:TitleBar tcx:ID="HPJetAdvantageO_Dup0TitleBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand Setup</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">TitleBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TitleBar>
    <gui:MenuBar tcx:ID="SystemMenuBarSy_Dup0MenuBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0TitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">System Menu Bar</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SystemMenuBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:MenuBar>
    <gui:MenuItem tcx:ID="SystemItem1_Dup0MenuItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SystemMenuBarSy_Dup0MenuBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">System</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Item 1</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:MenuItem>
    <gui:Button tcx:ID="CloseClose_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0TitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Close</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Close</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="HPJetAdvantageO_Dup1Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand Progress</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">270</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="ProcessingStatiText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Processing:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">271</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="HPJetAdvantageO_Dup2Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1048</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:ProgressBar tcx:ID="HPJetAdvantageOProgressBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">msctls_progress32</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1050</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ProgressBar>
    <gui:Button tcx:ID="CancelButton105Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Cancel</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1052</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="Static290Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">290</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="HPJetAdvantageO_Dup3Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">291</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:TitleBar tcx:ID="HPJetAdvantageO_Dup1TitleBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup0Window</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">TitleBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TitleBar>
    <gui:MenuBar tcx:ID="SystemMenuBarSy_Dup1MenuBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1TitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">System Menu Bar</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SystemMenuBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:MenuBar>
    <gui:MenuItem tcx:ID="SystemItem1_Dup1MenuItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SystemMenuBarSy_Dup1MenuBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">System</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Item 1</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:MenuItem>
    <gui:Button tcx:ID="MinimizeMinimizButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1TitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Minimize</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Minimize</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="MaximizeMaximizButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1TitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Maximize</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Maximize</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="CloseClose_Dup1Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageO_Dup1TitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Close</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Close</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
  </tcx:Resources>
</tcx:Context>