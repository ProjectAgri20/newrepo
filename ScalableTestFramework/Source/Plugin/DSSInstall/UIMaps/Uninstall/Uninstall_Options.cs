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
    public class Uninstall_Options
    {
        /// <summary>
        /// Backing field for HPDigitalSendinWindow property
        /// </summary>
        private Window fHPDigitalSendinWindow;

        /// <summary>
        /// Backing field for ModifyButton872RadioButton property
        /// </summary>
        private RadioButton fModifyButton872RadioButton;

        /// <summary>
        /// Backing field for RepairButton730RadioButton property
        /// </summary>
        private RadioButton fRepairButton730RadioButton;

        /// <summary>
        /// Backing field for RemoveButton624RadioButton property
        /// </summary>
        private RadioButton fRemoveButton624RadioButton;

        /// <summary>
        /// Backing field for BackButton7139Button property
        /// </summary>
        private Button fBackButton7139Button;

        /// <summary>
        /// Backing field for NextButton7196Button property
        /// </summary>
        private Button fNextButton7196Button;

        /// <summary>
        /// Backing field for CancelButton713Button property
        /// </summary>
        private Button fCancelButton713Button;

        /// <summary>
        /// Backing field for ModifyrepairorrText property
        /// </summary>
        private Text fModifyrepairorrText;

        /// <summary>
        /// Backing field for ProgramMaintenaText property
        /// </summary>
        private Text fProgramMaintenaText;

        /// <summary>
        /// Backing field for ProgramMaintenaImage property
        /// </summary>
        private Image fProgramMaintenaImage;

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
        /// Backing field for ChangewhichprogText property
        /// </summary>
        private Text fChangewhichprogText;

        /// <summary>
        /// Backing field for RepairinstallatText property
        /// </summary>
        private Text fRepairinstallatText;

        /// <summary>
        /// Backing field for RemoveHPDigitalText property
        /// </summary>
        private Text fRemoveHPDigitalText;

        /// <summary>
        /// Backing field for RemoveHPDigitalImage property
        /// </summary>
        private Image fRemoveHPDigitalImage;

        /// <summary>
        /// Backing field for NewBinary6StatiImage property
        /// </summary>
        private Image fNewBinary6StatiImage;

        /// <summary>
        /// Backing field for NewBinary7StatiImage property
        /// </summary>
        private Image fNewBinary7StatiImage;

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

        private string _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="Uninstall_Options"/> class.
        /// </summary>
        /// <param name="framework">
        /// The framework.
        /// </param>
        public Uninstall_Options(UIAFramework framework)
        {
            Desktop = new Desktop(framework);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Uninstall_Options"/> class using the default UIAFramework.
        /// </summary>
        public Uninstall_Options()
        {
            Desktop = new Desktop();
        }

        public Uninstall_Options(string version)
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
            RemoveButton624RadioButton.WaitForAvailable(timeout);

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

      
        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public RadioButton ModifyButton872RadioButton
        {
            get
            {
                if (null == fModifyButton872RadioButton)
                {
                    fModifyButton872RadioButton = new RadioButton("ModifyButton872RadioButton", HPDigitalSendinWindow);
                    fModifyButton872RadioButton.UIMap.Scope = UIASeachScope.Descendants;
                    fModifyButton872RadioButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                fModifyButton872RadioButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Modify");
                }

                return fModifyButton872RadioButton;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public RadioButton RepairButton730RadioButton
        {
            get
            {
                if (null == fRepairButton730RadioButton)
                {
                    fRepairButton730RadioButton = new RadioButton("RepairButton730RadioButton", HPDigitalSendinWindow);
                    fRepairButton730RadioButton.UIMap.Scope = UIASeachScope.Descendants;
                    fRepairButton730RadioButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fRepairButton730RadioButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Repair");
                }

                return fRepairButton730RadioButton;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public RadioButton RemoveButton624RadioButton
        {
            get
            {
                if (null == fRemoveButton624RadioButton)
                {
                    fRemoveButton624RadioButton = new RadioButton("RemoveButton624RadioButton", HPDigitalSendinWindow);
                    fRemoveButton624RadioButton.UIMap.Scope = UIASeachScope.Descendants;
                    fRemoveButton624RadioButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fRemoveButton624RadioButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Remove");
                }

                return fRemoveButton624RadioButton;
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
        public Button NextButton7196Button
        {
            get
            {
                if (null == fNextButton7196Button)
                {
                    fNextButton7196Button = new Button("NextButton7196Button", HPDigitalSendinWindow);
                    fNextButton7196Button.UIMap.Scope = UIASeachScope.Children;
                    fNextButton7196Button.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fNextButton7196Button.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Next >");
                }

                return fNextButton7196Button;
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
                    fCancelButton713Button.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"7136");
                    fCancelButton713Button.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Cancel");
                }

                return fCancelButton713Button;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text ModifyrepairorrText
        {
            get
            {
                if (null == fModifyrepairorrText)
                {
                    fModifyrepairorrText = new Text("ModifyrepairorrText", HPDigitalSendinWindow);
                    fModifyrepairorrText.UIMap.Scope = UIASeachScope.Children;
                    fModifyrepairorrText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fModifyrepairorrText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"7168");
                    fModifyrepairorrText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Modify, repair, or remove the program.");
                }

                return fModifyrepairorrText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text ProgramMaintenaText
        {
            get
            {
                if (null == fProgramMaintenaText)
                {
                    fProgramMaintenaText = new Text("ProgramMaintenaText", HPDigitalSendinWindow);
                    fProgramMaintenaText.UIMap.Scope = UIASeachScope.Children;
                    fProgramMaintenaText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fProgramMaintenaText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"7170");
                    fProgramMaintenaText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Program Maintenance");
                }

                return fProgramMaintenaText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Image ProgramMaintenaImage
        {
            get
            {
                if (null == fProgramMaintenaImage)
                {
                    fProgramMaintenaImage = new Image("ProgramMaintenaImage", HPDigitalSendinWindow);
                    fProgramMaintenaImage.UIMap.Scope = UIASeachScope.Children;
                    fProgramMaintenaImage.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fProgramMaintenaImage.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8422");
                    fProgramMaintenaImage.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Program Maintenance");
                }

                return fProgramMaintenaImage;
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
        public Text ChangewhichprogText
        {
            get
            {
                if (null == fChangewhichprogText)
                {
                    fChangewhichprogText = new Text("ChangewhichprogText", HPDigitalSendinWindow);
                    fChangewhichprogText.UIMap.Scope = UIASeachScope.Children;
                    fChangewhichprogText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fChangewhichprogText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8463");
                    fChangewhichprogText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Change which program features are installed. This option displays the Custom Selection dialog in which you can change the way features are installed.");
                }

                return fChangewhichprogText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text RepairinstallatText
        {
            get
            {
                if (null == fRepairinstallatText)
                {
                    fRepairinstallatText = new Text("RepairinstallatText", HPDigitalSendinWindow);
                    fRepairinstallatText.UIMap.Scope = UIASeachScope.Children;
                    fRepairinstallatText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fRepairinstallatText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8465");
                    fRepairinstallatText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Repair installation errors in the program. This option fixes missing or corrupt files, shortcuts, and registry entries.");
                }

                return fRepairinstallatText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text RemoveHPDigitalText
        {
            get
            {
                if (null == fRemoveHPDigitalText)
                {
                    fRemoveHPDigitalText = new Text("RemoveHPDigitalText", HPDigitalSendinWindow);
                    fRemoveHPDigitalText.UIMap.Scope = UIASeachScope.Children;
                    fRemoveHPDigitalText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fRemoveHPDigitalText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8467");
                    fRemoveHPDigitalText.UIMap.SearchProperties.Add(UIASearchProperty.Name, $@"Remove HP Digital Sending Software {_version} from your computer.");
                }

                return fRemoveHPDigitalText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Image RemoveHPDigitalImage
        {
            get
            {
                if (null == fRemoveHPDigitalImage)
                {
                    fRemoveHPDigitalImage = new Image("RemoveHPDigitalImage", HPDigitalSendinWindow);
                    fRemoveHPDigitalImage.UIMap.Scope = UIASeachScope.Children;
                    fRemoveHPDigitalImage.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fRemoveHPDigitalImage.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8592");
                    fRemoveHPDigitalImage.UIMap.SearchProperties.Add(UIASearchProperty.Name, $@"Remove HP Digital Sending Software {_version} from your computer.");
                }

                return fRemoveHPDigitalImage;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Image NewBinary6StatiImage
        {
            get
            {
                if (null == fNewBinary6StatiImage)
                {
                    fNewBinary6StatiImage = new Image("NewBinary6StatiImage", HPDigitalSendinWindow);
                    fNewBinary6StatiImage.UIMap.Scope = UIASeachScope.Children;
                    fNewBinary6StatiImage.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fNewBinary6StatiImage.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8593");
                    fNewBinary6StatiImage.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"NewBinary6");
                }

                return fNewBinary6StatiImage;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Image NewBinary7StatiImage
        {
            get
            {
                if (null == fNewBinary7StatiImage)
                {
                    fNewBinary7StatiImage = new Image("NewBinary7StatiImage", HPDigitalSendinWindow);
                    fNewBinary7StatiImage.UIMap.Scope = UIASeachScope.Children;
                    fNewBinary7StatiImage.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fNewBinary7StatiImage.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8594");
                    fNewBinary7StatiImage.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"NewBinary7");
                }

                return fNewBinary7StatiImage;
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
