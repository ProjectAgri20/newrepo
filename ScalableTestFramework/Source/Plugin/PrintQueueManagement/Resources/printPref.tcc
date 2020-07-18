<tcx:Context xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework" xmlns:gui="http://sherman.americas.hpqcorp.net/topcat/schemas/tools/gui-automation">
  <tcx:Resources>
    <gui:Window tcx:ID="HPOfficejetProXWindow">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">Desktop</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
       
      </gui:SearchProperties>
    </gui:Window>
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
    <gui:TabItem tcx:ID="AdvancedTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Advanced</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="PrintingShortcuTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Printing Shortcuts</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:Pane tcx:ID="A32770Pane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Tree tcx:ID="SysTreeView3291Tree">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.A32770Pane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">SysTreeView32</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">9110</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Tree>
    <gui:Spinner tcx:ID="CopyCountmsctlsSpinner">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTreeView3291Tree</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">msctls_updown32</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Copy Count: </gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">9000</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Spinner>
    <gui:Pane tcx:ID="PrintingShortcuPane">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuTabItem</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">#32770</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Printing Shortcuts</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Pane>
    <gui:Text tcx:ID="AprintingshortcText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">A printing shortcut is a collection of saved print settings that you can select with a single click.</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">957</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:List tcx:ID="ListBox956List">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ListBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">956</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:List>
    <gui:ListItem tcx:ID="FactoryDefaultsListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.ListBox956List</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Factory Defaults</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="EcoSMARTSettingListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.ListBox956List</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">EcoSMART Settings</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:Text tcx:ID="PapersizesStatiText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Paper sizes:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">500</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:ComboBox tcx:ID="PapersizesComboComboBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Paper sizes:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">501</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ComboBox>
    <gui:List tcx:ID="PapersizesComboList">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboLBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Paper sizes:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">ListBox</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:List>
    <gui:ListItem tcx:ID="LetterListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Letter</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="LegalListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Legal</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="ExecutiveListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Executive</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="StatementListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Statement</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Oficio85x13ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Oficio 8.5x13</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A3x5ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">3x5</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A4x6ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">4x6</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A5x7ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">5x7</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A5x8ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">5x8</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A4ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">A4</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A5ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">A5</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A6ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">A6</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="B5JISListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">B5 (JIS)</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="B6JISListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">B6 (JIS)</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="L9x13cmListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">L 9x13 cm</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A10x15cmListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">10x15cm</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Oficio216x340mmListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Oficio 216x340 mm</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A16K195x270mmListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">16K 195x270 mm</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A16K184x260mmListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">16K 184x260 mm</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A16K197x273mmListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">16K 197x273 mm</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="JapanesePostcarListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Japanese Postcard</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="DoubleJapanPostListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Double Japan Postcard Rotated</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Envelope10ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Envelope #10</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="EnvelopeMonarchListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Envelope Monarch</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="EnvelopeB5ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Envelope B5</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="EnvelopeC5ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Envelope C5</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="EnvelopeC6ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Envelope C6</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="EnvelopeDLListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Envelope DL</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="JapaneseEnvelop_Dup0ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Japanese Envelope Chou #3</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="JapaneseEnvelop_Dup1ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Japanese Envelope Chou #4</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:Button tcx:ID="DropDownButtonD_Dup0Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersizesComboComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Drop Down Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">DropDown</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="PapersourceStatText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Paper source:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">502</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:ComboBox tcx:ID="PapersourceCombComboBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Paper source:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">503</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ComboBox>
    <gui:List tcx:ID="PapersourceCombList">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboLBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Paper source:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">ListBox</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:List>
    <gui:ListItem tcx:ID="AutomaticallySeListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Automatically Select</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="PrinterautoseleListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Printer auto select</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="ManualFeedinTraListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Manual Feed in Tray 1</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 1ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 1</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 2ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 2</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 3ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 3</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 4ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 4</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 5ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 5</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 6ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 6</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 7ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 7</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 8ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 8</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 9ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 9</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Tray 10ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Tray 10</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:Button tcx:ID="DropDownButtonD_Dup1Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapersourceCombComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Drop Down Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">DropDown</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="PagespersheetStText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Pages per sheet:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">504</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:ComboBox tcx:ID="PagespersheetCoComboBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Pages per sheet:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">505</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ComboBox>
    <gui:List tcx:ID="PagespersheetCoList">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PagespersheetCoComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboLBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Pages per sheet:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">ListBox</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:List>
    <gui:ListItem tcx:ID="A1pagepersheetListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PagespersheetCoList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">1 page per sheet</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A2pagespersheetListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PagespersheetCoList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">2 pages per sheet</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A4pagespersheetListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PagespersheetCoList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">4 pages per sheet</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A6pagespersheetListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PagespersheetCoList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">6 pages per sheet</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A9pagespersheetListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PagespersheetCoList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">9 pages per sheet</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="A16pagespersheeListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PagespersheetCoList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">16 pages per sheet</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:Button tcx:ID="DropDownButtonD_Dup2Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PagespersheetCoComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Drop Down Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">DropDown</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="PrintonbothsideText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Print on both sides:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">506</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:ComboBox tcx:ID="PrintonbothsideComboBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Print on both sides:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">507</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ComboBox>
    <gui:List tcx:ID="PrintonbothsideList">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintonbothsideComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboLBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Print on both sides:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">ListBox</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:List>
    <gui:ListItem tcx:ID="NoListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintonbothsideList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">No</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="YesflipoverListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintonbothsideList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Yes, flip over</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="YesflipupListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintonbothsideList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Yes, flip up</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:Button tcx:ID="DropDownButtonD_Dup3Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintonbothsideComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Drop Down Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">DropDown</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="PapertypeStaticText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Paper type:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">508</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:ComboBox tcx:ID="PapertypeComboBComboBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Paper type:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">509</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ComboBox>
    <gui:List tcx:ID="PapertypeComboLList">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboBComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboLBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Paper type:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">ListBox</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:List>
    <gui:ListItem tcx:ID="UnspecifiedListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Unspecified</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="PlainListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Plain</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="HPEcoSMARTLiteListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP EcoSMART Lite</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Light6074gListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Light 60-74g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Intermediate859ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Intermediate 85-95g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="HPPremiumPresenListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP Premium Presentation Matte 120g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="MidWeight96110gListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Mid-Weight 96-110g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Heavy111130gListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Heavy 111-130g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="HPBrochureMatteListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP Brochure Matte 180g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="HPBrochureGlossListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP Brochure Glossy 180g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="ExtraHeavy13117ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Extra Heavy 131-175g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="HPCoverMatte200ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP Cover Matte 200g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="HPAdvancedPhotoListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP Advanced Photo Papers</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="Cardstock176220ListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Cardstock 176-220g</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="LabelsListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Labels</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="LetterheadListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Letterhead</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="EnvelopeListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Envelope</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="HeavyEnvelopeListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Heavy Envelope</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="PreprintedListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Preprinted</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="PrepunchedListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboLList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Prepunched</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:Button tcx:ID="DropDownButtonD_Dup4Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PapertypeComboBComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Drop Down Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">DropDown</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="OrientationStatText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Orientation:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">510</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:ComboBox tcx:ID="OrientationCombComboBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Orientation:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">511</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ComboBox>
    <gui:List tcx:ID="OrientationCombList">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.OrientationCombComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">ComboLBox</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Orientation:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">ListBox</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:List>
    <gui:ListItem tcx:ID="PortraitListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.OrientationCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Portrait</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:ListItem tcx:ID="LandscapeListItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.OrientationCombList</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Landscape</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:ListItem>
    <gui:Button tcx:ID="DropDownButtonD_Dup5Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.OrientationCombComboBox</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Drop Down Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">DropDown</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="SaveAsButton644Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Save As...</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">644</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="DeleteButton676Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Delete</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">676</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="ResetButton1027Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Reset</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1027</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Button tcx:ID="LandscapeButtonButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Landscape</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">441</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="ToresetthesettiText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">To reset the settings, select the type of printing you want to do from the Printing Shortcuts menu.</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">960</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="Static111Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">111</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Button tcx:ID="HelpButton409Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Help</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">409</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Text tcx:ID="PrintingshortcuText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Printing shortcuts:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">958</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="A1169827inchesSText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Static</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">11.69 × 8.27 inches</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">1014</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Button tcx:ID="AboutButton410Button">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.PrintingShortcuPane</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">Button</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">About...</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">410</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:TabItem tcx:ID="PaperQualityTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Paper/Quality</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="EffectsTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Effects</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="FinishingTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Finishing</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="ColorTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Color</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TabItem tcx:ID="ServicesTabItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SysTabControl32Tab</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Services</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TabItem>
    <gui:TitleBar tcx:ID="HPOfficejetProXTitleBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPOfficejetProXWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP Officejet Pro X576dw MFP (16.190.4.33) Printing Preferences</gui:SearchProperty>
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