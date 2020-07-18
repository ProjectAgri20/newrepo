<tcx:Context xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework" xmlns:gui="http://sherman.americas.hpqcorp.net/topcat/schemas/tools/gui-automation">
  <tcx:Resources>
    <gui:Pane tcx:ID="HPePrint32770Pane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">Desktop</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Button tcx:ID="HPConnectedButtButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP Connected</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1061</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Pane tcx:ID="A32770393900Pane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">393900</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Text tcx:ID="Static1078Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1078</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Static1022Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1022</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:DataGrid tcx:ID="SysListView3210DataGrid">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">SysListView32</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1098</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:DataGrid>
    <gui:DataItem tcx:ID="RecentDataItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysListView3210DataGrid</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Recent</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:DataItem>
    <gui:Text tcx:ID="RecentText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.RecentDataItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Recent</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:DataItem tcx:ID="JetAdvantageDataItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysListView3210DataGrid</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">JetAdvantage</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:DataItem>
    <gui:Text tcx:ID="JetAdvantageText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.JetAdvantageDataItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">JetAdvantage</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:DataItem tcx:ID="ConnectedDataItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysListView3210DataGrid</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Connected</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:DataItem>
    <gui:Text tcx:ID="ConnectedText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.ConnectedDataItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Connected</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:DataItem tcx:ID="ServiceDataItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysListView3210DataGrid</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Service</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:DataItem>
    <gui:Text tcx:ID="ServiceText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.ServiceDataItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Service</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Static1069Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1069</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Static1086Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1086</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Button tcx:ID="CancelButton2Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Cancel</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">2</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="OKButton1Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">OK</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="Button8Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">8</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="Static1088Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1088</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Button tcx:ID="Button3336Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">3336</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="HPePrintStatic1Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP ePrint</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1050</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
  </tcx:Resources>
</tcx:Context>