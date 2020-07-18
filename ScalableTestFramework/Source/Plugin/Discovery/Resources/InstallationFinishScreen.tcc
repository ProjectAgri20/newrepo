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
    <gui:Pane tcx:ID="BonjourPrinterWPane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Bonjour Printer Wizard</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Text tcx:ID="CompletingtheBoText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Completing the Bonjour Printer Wizard.</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1021</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="YouarereadytocoText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">You are ready to complete the Bonjour Printer Wizard.  The printer has the following settings:</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="NameStaticText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Name:</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="ManufacturerStaText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Manufacturer:</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="ModelStaticText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Model:</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="ProtocolStaticText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Protocol:</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="DefaultStaticText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Default:</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="HPOfficeJetPro8_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP OfficeJet Pro 8210 [7C800A] (3)</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1018</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="HPStatic1019Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1019</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="HPOfficeJetPro8_Dup1Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP OfficeJet Pro 8210 PCL 6</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1020</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Priority">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">${BonjourHighestService}</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1022</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="NoStatic1023Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">No</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1023</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="TocompletetheinText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">To complete the installation, click Finish.</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1034</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="TochangetheseseText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">To change these settings, click Back.</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1035</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Static12326Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">12326</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Button tcx:ID="BackButton12323Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">&lt; Back</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">12323</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="FinishButton123Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.BonjourPrinterWWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Finish</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">12325</gui:SearchProperty>
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