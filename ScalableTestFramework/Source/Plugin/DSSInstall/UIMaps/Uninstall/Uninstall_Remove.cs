﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//
//     J2: Template .\Templates
//         using modules ['Xml2CS']
// </auto-generated>
//------------------------------------------------------------------------------


namespace HP.ScalableTest.Plugin.DSSInstall.UIMaps.Uninstall
{
    using TopCat.TestApi.GUIAutomation.Controls;
    using TopCat.TestApi.GUIAutomation.Enums;

    /// <summary>
    /// Application which exercises the TopCat Coded C# test API.
    /// </summary>
    public class Uninstall_Remove
    {
        private string _version;
        /// <summary>
        /// Backing field for HPDigitalSendinWindow property
        /// </summary>
        private Window fHPDigitalSendinWindow;

        /// <summary>
        /// Backing field for RemoveButton733Button property
        /// </summary>
        private Button fRemoveButton733Button;

        /// <summary>
        /// Backing field for CancelButton713Button property
        /// </summary>
        private Button fCancelButton713Button;

        /// <summary>
        /// Backing field for BackButton7139Button property
        /// </summary>
        private Button fBackButton7139Button;

        /// <summary>
        /// Backing field for YouhavechosentoText property
        /// </summary>
        private Text fYouhavechosentoText;

        /// <summary>
        /// Backing field for RemovethePrograText property
        /// </summary>
        private Text fRemovethePrograText;

        /// <summary>
        /// Backing field for ClickRemovetoreText property
        /// </summary>
        private Text fClickRemovetoreText;

        /// <summary>
        /// Backing field for Static7176Text property
        /// </summary>
        private Text fStatic7176Text;

        /// <summary>
        /// Backing field for NewBinary20StatImage property
        /// </summary>
        private Image fNewBinary20StatImage;

        /// <summary>
        /// Backing field for NewBinary20StatText property
        /// </summary>
        private Text fNewBinary20StatText;

        /// <summary>
        /// Backing field for InstallShieldStDup0Text property
        /// </summary>
        private Text fInstallShieldStDup0Text;

        /// <summary>
        /// Backing field for InstallShieldStDup1Text property
        /// </summary>
        private Text fInstallShieldStDup1Text;

        /// <summary>
        /// Backing field for InstallShieldStDup2Text property
        /// </summary>
        private Text fInstallShieldStDup2Text;

        /// <summary>
        /// Backing field for IfyouwanttoreviText property
        /// </summary>
        private Text fIfyouwanttoreviText;

        /// <summary>
        /// Backing field for HPDigitalSendinTitleBar property
        /// </summary>
        private TitleBar fHPDigitalSendinTitleBar;

        /// <summary>
        /// Backing field for SystemMenuBarSyMenuBar property
        /// </summary>
        private MenuBar fSystemMenuBarSyMenuBar;

        /// <summary>
        /// Backing field for SystemItem1MenuItem property
        /// </summary>
        private MenuItem fSystemItem1MenuItem;

        /// <summary>
        /// Backing field for CloseCloseButton property
        /// </summary>
        private Button fCloseCloseButton;

        private Window fErrorWindow;
        private Button fYesErrorButton;
        private Button fNoErrorButton;
        private Button fCancelErrorButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="Uninstall_Remove"/> class.
        /// </summary>
        /// <param name="framework">
        /// The framework.
        /// </param>
        public Uninstall_Remove(UIAFramework framework)
        {
            Desktop = new Desktop(framework);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Uninstall_Remove"/> class using the default UIAFramework.
        /// </summary>
        public Uninstall_Remove()
        {
            Desktop = new Desktop();
        }

        public Uninstall_Remove(string version)
        {
            Desktop = new Desktop(UIAFramework.ManagedUIA);
            _version = version;
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Desktop Desktop { get; private set; }

        public void WaitForAvailable(int timeout)
        {
            HPDigitalSendinWindow.WaitForAvailable(timeout);
            RemoveButton733Button.WaitForAvailable(timeout);

        }
        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Window HPDigitalSendinWindow
        {
            get
            {
                if (null == fHPDigitalSendinWindow)
                {
                    fHPDigitalSendinWindow = new Window("HPDigitalSendinWindow", Desktop);
                    fHPDigitalSendinWindow.UIMap.Scope = UIASeachScope.Children;
                    fHPDigitalSendinWindow.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"MsiDialogCloseClass");
                    fHPDigitalSendinWindow.UIMap.SearchProperties.Add(UIASearchProperty.Name, $@"HP Digital Sending Software {_version} - InstallShield Wizard");
                }

                return fHPDigitalSendinWindow;
            }
        }
        public Window ErrorWindow
        {
            get
            {
                if (null == fErrorWindow)
                {
                    fErrorWindow = new Window("ErrorWindow", HPDigitalSendinWindow);
                    fErrorWindow.UIMap.Scope = UIASeachScope.Children;
                    fErrorWindow.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"#32770");
                    fErrorWindow.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Error");
                }

                return fErrorWindow;
            }
        }

        public Button YesErrorButton
        {
            get
            {
                if (null == fYesErrorButton)
                {
                    fYesErrorButton = new Button("YesErrorButton", ErrorWindow);
                    fYesErrorButton.UIMap.Scope = UIASeachScope.Children;
                    fYesErrorButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fYesErrorButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Yes");
                }

                return fYesErrorButton;
            }
        }

        public Button NoErrorButton
        {
            get
            {
                if (null == fNoErrorButton)
                {
                    fNoErrorButton = new Button("NoErrorButton", ErrorWindow);
                    fNoErrorButton.UIMap.Scope = UIASeachScope.Children;
                    fNoErrorButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fNoErrorButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"No");
                }

                return fNoErrorButton;
            }
        }

        public Button CancelErrorButton
        {
            get
            {
                if (null == fCancelErrorButton)
                {
                    fCancelErrorButton = new Button("CancelErrorButton", ErrorWindow);
                    fCancelErrorButton.UIMap.Scope = UIASeachScope.Children;
                    fCancelErrorButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fCancelErrorButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Cancel");
                }

                return fCancelErrorButton;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Button RemoveButton733Button
        {
            get
            {
                if (null == fRemoveButton733Button)
                {
                    fRemoveButton733Button = new Button("RemoveButton733Button", HPDigitalSendinWindow);
                    fRemoveButton733Button.UIMap.Scope = UIASeachScope.Children;
                    fRemoveButton733Button.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fRemoveButton733Button.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Remove");
                }

                return fRemoveButton733Button;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Button CancelButton713Button
        {
            get
            {
                if (null == fCancelButton713Button)
                {
                    fCancelButton713Button = new Button("CancelButton713Button", HPDigitalSendinWindow);
                    fCancelButton713Button.UIMap.Scope = UIASeachScope.Children;
                    fCancelButton713Button.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fCancelButton713Button.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Cancel");
                }

                return fCancelButton713Button;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Button BackButton7139Button
        {
            get
            {
                if (null == fBackButton7139Button)
                {
                    fBackButton7139Button = new Button("BackButton7139Button", HPDigitalSendinWindow);
                    fBackButton7139Button.UIMap.Scope = UIASeachScope.Children;
                    fBackButton7139Button.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fBackButton7139Button.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"< Back");
                }

                return fBackButton7139Button;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text YouhavechosentoText
        {
            get
            {
                if (null == fYouhavechosentoText)
                {
                    fYouhavechosentoText = new Text("YouhavechosentoText", HPDigitalSendinWindow);
                    fYouhavechosentoText.UIMap.Scope = UIASeachScope.Children;
                    fYouhavechosentoText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fYouhavechosentoText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"7168");
                    fYouhavechosentoText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"You have chosen to remove the program from your system.");
                }

                return fYouhavechosentoText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text RemovethePrograText
        {
            get
            {
                if (null == fRemovethePrograText)
                {
                    fRemovethePrograText = new Text("RemovethePrograText", HPDigitalSendinWindow);
                    fRemovethePrograText.UIMap.Scope = UIASeachScope.Children;
                    fRemovethePrograText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fRemovethePrograText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"7170");
                    fRemovethePrograText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Remove the Program");
                }

                return fRemovethePrograText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text ClickRemovetoreText
        {
            get
            {
                if (null == fClickRemovetoreText)
                {
                    fClickRemovetoreText = new Text("ClickRemovetoreText", HPDigitalSendinWindow);
                    fClickRemovetoreText.UIMap.Scope = UIASeachScope.Children;
                    fClickRemovetoreText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fClickRemovetoreText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"7172");
                    fClickRemovetoreText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Click Remove to remove HP Digital Sending Software 5.06.00 from your computer. After removal, this program will no longer be available for use.");
                }

                return fClickRemovetoreText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text Static7176Text
        {
            get
            {
                if (null == fStatic7176Text)
                {
                    fStatic7176Text = new Text("Static7176Text", HPDigitalSendinWindow);
                    fStatic7176Text.UIMap.Scope = UIASeachScope.Children;
                    fStatic7176Text.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fStatic7176Text.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"7176");
                }

                return fStatic7176Text;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Image NewBinary20StatImage
        {
            get
            {
                if (null == fNewBinary20StatImage)
                {
                    fNewBinary20StatImage = new Image("NewBinary20StatImage", HPDigitalSendinWindow);
                    fNewBinary20StatImage.UIMap.Scope = UIASeachScope.Children;
                    fNewBinary20StatImage.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fNewBinary20StatImage.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8422");
                    fNewBinary20StatImage.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"NewBinary20");
                }

                return fNewBinary20StatImage;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text NewBinary20StatText
        {
            get
            {
                if (null == fNewBinary20StatText)
                {
                    fNewBinary20StatText = new Text("NewBinary20StatText", HPDigitalSendinWindow);
                    fNewBinary20StatText.UIMap.Scope = UIASeachScope.Children;
                    fNewBinary20StatText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fNewBinary20StatText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8424");
                    fNewBinary20StatText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"NewBinary20");
                }

                return fNewBinary20StatText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text InstallShieldStDup0Text
        {
            get
            {
                if (null == fInstallShieldStDup0Text)
                {
                    fInstallShieldStDup0Text = new Text("InstallShieldStDup0Text", HPDigitalSendinWindow);
                    fInstallShieldStDup0Text.UIMap.Scope = UIASeachScope.Children;
                    fInstallShieldStDup0Text.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fInstallShieldStDup0Text.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8426");
                    fInstallShieldStDup0Text.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"InstallShield");
                }

                return fInstallShieldStDup0Text;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text InstallShieldStDup1Text
        {
            get
            {
                if (null == fInstallShieldStDup1Text)
                {
                    fInstallShieldStDup1Text = new Text("InstallShieldStDup1Text", HPDigitalSendinWindow);
                    fInstallShieldStDup1Text.UIMap.Scope = UIASeachScope.Children;
                    fInstallShieldStDup1Text.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fInstallShieldStDup1Text.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8428");
                    fInstallShieldStDup1Text.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"InstallShield");
                }

                return fInstallShieldStDup1Text;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text InstallShieldStDup2Text
        {
            get
            {
                if (null == fInstallShieldStDup2Text)
                {
                    fInstallShieldStDup2Text = new Text("InstallShieldStDup2Text", HPDigitalSendinWindow);
                    fInstallShieldStDup2Text.UIMap.Scope = UIASeachScope.Children;
                    fInstallShieldStDup2Text.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fInstallShieldStDup2Text.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8437");
                    fInstallShieldStDup2Text.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"InstallShield");
                }

                return fInstallShieldStDup2Text;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text IfyouwanttoreviText
        {
            get
            {
                if (null == fIfyouwanttoreviText)
                {
                    fIfyouwanttoreviText = new Text("IfyouwanttoreviText", HPDigitalSendinWindow);
                    fIfyouwanttoreviText.UIMap.Scope = UIASeachScope.Children;
                    fIfyouwanttoreviText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fIfyouwanttoreviText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8615");
                    fIfyouwanttoreviText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"If you want to review or change any settings, click Back.");
                }

                return fIfyouwanttoreviText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public TitleBar HPDigitalSendinTitleBar
        {
            get
            {
                if (null == fHPDigitalSendinTitleBar)
                {
                    fHPDigitalSendinTitleBar = new TitleBar("HPDigitalSendinTitleBar", HPDigitalSendinWindow);
                    fHPDigitalSendinTitleBar.UIMap.Scope = UIASeachScope.Children;
                    fHPDigitalSendinTitleBar.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"TitleBar");
                    fHPDigitalSendinTitleBar.UIMap.SearchProperties.Add(UIASearchProperty.Name, $@"HP Digital Sending Software {_version} - InstallShield Wizard");
                }

                return fHPDigitalSendinTitleBar;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public MenuBar SystemMenuBarSyMenuBar
        {
            get
            {
                if (null == fSystemMenuBarSyMenuBar)
                {
                    fSystemMenuBarSyMenuBar = new MenuBar("SystemMenuBarSyMenuBar", HPDigitalSendinTitleBar);
                    fSystemMenuBarSyMenuBar.UIMap.Scope = UIASeachScope.Children;
                    fSystemMenuBarSyMenuBar.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"SystemMenuBar");
                    fSystemMenuBarSyMenuBar.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"System Menu Bar");
                }

                return fSystemMenuBarSyMenuBar;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public MenuItem SystemItem1MenuItem
        {
            get
            {
                if (null == fSystemItem1MenuItem)
                {
                    fSystemItem1MenuItem = new MenuItem("SystemItem1MenuItem", SystemMenuBarSyMenuBar);
                    fSystemItem1MenuItem.UIMap.Scope = UIASeachScope.Children;
                    fSystemItem1MenuItem.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"Item 1");
                    fSystemItem1MenuItem.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"System");
                }

                return fSystemItem1MenuItem;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Button CloseCloseButton
        {
            get
            {
                if (null == fCloseCloseButton)
                {
                    fCloseCloseButton = new Button("CloseCloseButton", HPDigitalSendinTitleBar);
                    fCloseCloseButton.UIMap.Scope = UIASeachScope.Children;
                    fCloseCloseButton.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"Close");
                    fCloseCloseButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Close");
                }

                return fCloseCloseButton;
            }
        }
    }
}