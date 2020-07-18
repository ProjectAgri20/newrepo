<tcx:Context xmlns:tcx="http://sherman.americas.hpqcorp.net/topcat/schemas/framework" xmlns:gui="http://sherman.americas.hpqcorp.net/topcat/schemas/tools/gui-automation">
  <tcx:Resources>
    <!-- 
      There are differences between Win 7 and Win 8 on ClassNames.  Find and replace depending on which O/S this will run on
        Win7:  _r27_
        Win8:  _r11_
        Win8.1:  _r28_
    -->
    <gui:Window tcx:ID="HPJetAdvantageOWindow">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">Desktop</gui:ReferenceContainer>
      <gui:SearchProperties>
        <!-- gui:SearchProperty gui:Name="ClassName">WindowsForms10.Window.8.app.0.33c0d9d_r27_ad1</gui:SearchProperty -->
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">HPAuthForm</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Window>
    <gui:Hyperlink tcx:ID="CreateNewAccounHyperlink">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.STATIC.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Create New Account</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">CreateAccountLink</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Hyperlink>
    <gui:Hyperlink tcx:ID="HelpWindowsFormHyperlink">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.STATIC.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Help</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">HelpLink</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Hyperlink>
    <gui:Hyperlink tcx:ID="HPOnlinePrivacyHyperlink">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.STATIC.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">HP Online Privacy Statement</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">PrivacyPolicyLink</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Hyperlink>
    <gui:Text tcx:ID="SignInWindowsFo_Dup0Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.STATIC.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Sign-In</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SignInLabel</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="SignInWindowsFo_Dup1Text">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.STATIC.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Sign-In</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">lbMessage</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:CheckBox tcx:ID="RememberMeWindoCheckBox">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.BUTTON.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Remember Me</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">RememberMeCheckbox</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:CheckBox>
    <gui:Button tcx:ID="SignInWindowsFoButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.BUTTON.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Sign-In</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">SignInButton</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
    <gui:Edit tcx:ID="WindowsForms10EEdit">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.EDIT.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Password</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Edit>
    <gui:Edit tcx:ID="PasswordWindowsEdit">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.EDIT.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Password:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Username</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Edit>
    <gui:Text tcx:ID="PasswordWindowsText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.STATIC.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">Password:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">PasswordLabel</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:Text tcx:ID="EmailWindowsForText">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="ClassName">WindowsForms10.STATIC.app.0.33c0d9d_r27_ad1</gui:SearchProperty>
        <gui:SearchProperty gui:Name="Name">E-mail:</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">EmailLabel</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Text>
    <gui:TitleBar tcx:ID="HPJetAdvantageOTitleBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOWindow</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">HP JetAdvantage On Demand</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">TitleBar</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:TitleBar>
    <gui:MenuBar tcx:ID="SystemMenuBarHPMenuBar">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOTitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">System Menu Bar</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">HPAuthForm</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:MenuBar>
    <gui:MenuItem tcx:ID="SystemItem1MenuItem">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.SystemMenuBarHPMenuBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">System</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Item 1</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:MenuItem>
    <gui:Button tcx:ID="CloseCloseButton">
      <gui:ProviderType>ManagedUIA</gui:ProviderType>
      <gui:ReferenceContainer gui:Scope="Children">${Context}.HPJetAdvantageOTitleBar</gui:ReferenceContainer>
      <gui:SearchProperties>
        <gui:SearchProperty gui:Name="Name">Close</gui:SearchProperty>
        <gui:SearchProperty gui:Name="AutomationId">Close</gui:SearchProperty>
      </gui:SearchProperties>
    </gui:Button>
  </tcx:Resources>
</tcx:Context>