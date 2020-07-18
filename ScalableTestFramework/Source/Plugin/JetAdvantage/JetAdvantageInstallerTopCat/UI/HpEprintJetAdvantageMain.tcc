<tcx:Context xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework" xmlns:gui="http://sherman.americas.hpqcorp.net/topcat/schemas/tools/gui-automation">
  <tcx:Resources>
    <gui:Pane tcx:ID="HPePrintHPJetAdPane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">Desktop</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WixStdBA</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP ePrint - HP JetAdvantage On Demand</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Button tcx:ID="InstallationSetButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrintHPJetAdPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Installation Settings</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1028</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
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
  </tcx:Resources>
</tcx:Context>