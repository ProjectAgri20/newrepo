<tcx:Context xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework" xmlns:gui="http://sherman.americas.hpqcorp.net/topcat/schemas/tools/gui-automation">
  <tcx:Resources>
    <gui:Pane tcx:ID="HPePrint32770Pane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">Desktop</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Pane tcx:ID="A32770Pane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Button tcx:ID="PrintingrayscalButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Print in grayscale</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1006</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="PrintonbothsideButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Print on both sides</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1005</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="Static1088_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1088</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Static1086_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1086</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Static1078_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1078</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Static1023Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1023</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="PreviewStatic10Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Preview</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1004</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Button tcx:ID="PreviewButton10Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Preview</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1025</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="Static1066Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1066</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="CopiesStatic101Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Copies:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1010</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:ComboBox tcx:ID="CopiesComboBox1ComboBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Copies:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1007</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ComboBox>
    <gui:List tcx:ID="CopiesComboLBoxList">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.CopiesComboBox1ComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboLBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Copies:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">ListBox</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:List>
    <gui:ListItem tcx:ID="A1ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.CopiesComboLBoxList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">1</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A2ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.CopiesComboLBoxList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">2</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A4ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.CopiesComboLBoxList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">4</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A6ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.CopiesComboLBoxList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">6</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A9ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.CopiesComboLBoxList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">9</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A16ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.CopiesComboLBoxList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">16</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:Button tcx:ID="DropDownButtonDButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.CopiesComboBox1ComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Drop Down Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">DropDown</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="PagespersheetStText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Pages per sheet:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1011</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Button tcx:ID="SettingsButton1Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Settings</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1079</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Spinner tcx:ID="msctls_updown32Spinner">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">msctls_updown32</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Spinner>
    <gui:Button tcx:ID="ForwardSmallIncButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.msctls_updown32Spinner</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Forward</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SmallIncrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="BackwardSmallDeButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.msctls_updown32Spinner</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Backward</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SmallDecrement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Pane tcx:ID="A327701442760Pane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPePrint32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1442760</gui:SearchProperty>
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
    <gui:Text tcx:ID="Static1078_Dup1Text">
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
    <gui:DataItem tcx:ID="EnterpriseDataItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysListView3210DataGrid</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Enterprise</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:DataItem>
    <gui:Text tcx:ID="EnterpriseText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.EnterpriseDataItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Enterprise</gui:SearchProperty>
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
    <gui:Text tcx:ID="Static1086_Dup1Text">
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
    <gui:Text tcx:ID="Static1088_Dup1Text">
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