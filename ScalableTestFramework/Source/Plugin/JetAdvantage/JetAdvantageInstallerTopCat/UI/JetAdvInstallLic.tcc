<tcx:Context xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework" xmlns:gui="http://sherman.americas.hpqcorp.net/topcat/schemas/tools/gui-automation">
  <tcx:Resources>
    <gui:Pane tcx:ID="HPePrintHPJetAdPane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">Desktop</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WixStdBA</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Text tcx:ID="Static259Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrintHPJetAdPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">259</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="ForHPJetAdvantaText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrintHPJetAdPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">For HP JetAdvantage Pull Print</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">260</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="InstallationwilText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrintHPJetAdPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Installation will be done per the Installation Settings. Unless changed, “HP ePrint” will become your default printer and anonymous usage data will be transmitted.</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">261</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Button tcx:ID="InstallationSetButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrintHPJetAdPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Installation Settings</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1028</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Hyperlink tcx:ID="IagreetotheEndUHyperlink">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrintHPJetAdPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">SysLink</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">I agree to the End-User License Agreement</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1030</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Hyperlink>
    <gui:Hyperlink tcx:ID="AHyperlink">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.IagreetotheEndUHyperlink</gui:ReferenceContainer>
      <gui:SearchProperties />
    </gui:Hyperlink>
    <gui:CheckBox tcx:ID="Button1031CheckBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrintHPJetAdPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1031</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:CheckBox>
    <gui:Button tcx:ID="InstallButton10Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrintHPJetAdPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Install</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1027</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="CloseButton1032Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrintHPJetAdPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Close</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1032</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
  </tcx:Resources>
</tcx:Context>