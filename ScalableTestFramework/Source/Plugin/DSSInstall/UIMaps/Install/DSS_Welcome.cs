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


using HP.ScalableTest.Framework.Plugin;
using TopCat.TestApi.Enums;

namespace HP.ScalableTest.Plugin.DSSInstall.UIMaps
{
    using TopCat.TestApi.GUIAutomation.Controls;
    using TopCat.TestApi.GUIAutomation.Enums;

    /// <summary>
    /// Application which exercises the TopCat Coded C# test API.
    /// </summary>
    public class DSS_Welcome: UIMap
    {

        private string _version;

       

      
        /// <summary>
        /// Backing field for HPDigitalSendinWindow property
        /// </summary>
        private Window fHPDigitalSendinWindow;

        /// <summary>
        /// Backing field for NextButton7196Button property
        /// </summary>
        private Button fNextButton7196Button;

        /// <summary>
        /// Backing field for CancelButton713Button property
        /// </summary>
        private Button fCancelButton713Button;

        /// <summary>
        /// Backing field for BackButton7139Button property
        /// </summary>
        private Button fBackButton7139Button;

        /// <summary>
        /// Backing field for WARNINGThisprogText property
        /// </summary>
        private Text fWARNINGThisprogText;

        /// <summary>
        /// Backing field for TheInstallShielDup0Text property
        /// </summary>
        private Text fTheInstallShielDup0Text;

      
        /// <summary>
        /// Backing field for NewBinary19StatImage property
        /// </summary>
        private Image fNewBinary19StatImage;

        /// <summary>
        /// Backing field for WelcometotheInsText property
        /// </summary>
        private Text fWelcometotheInsText;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="DSS_Welcome"/> class.
        /// </summary>
        /// <param name="framework">
        /// The framework.
        /// </param>
        public DSS_Welcome(UIAFramework framework)
        {
            Desktop = new Desktop(framework);
            ScreenName = "Welcome Screen";
            ScreenIndex = 2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DSS_Welcome"/> class using the default UIAFramework.
        /// </summary>
        public DSS_Welcome()
        {
            Desktop = new Desktop();
            ScreenName = "Welcome Screen";
            ScreenIndex = 2;
        }

        public DSS_Welcome(string version)
        {
            Desktop = new Desktop(UIAFramework.ManagedUIA);
            _version = version;
            ScreenName = "Welcome Screen";
            ScreenIndex = 2;
        }

        public void WaitForAvailable(int timeout)
        {
            HPDigitalSendinWindow.WaitForAvailable(timeout);
            WARNINGThisprogText.WaitForAvailable(timeout);
            NextButton7196Button.WaitForAvailable(timeout);
        }

        public override PluginExecutionResult PerformAction(int cancelScreen)
        {
            WaitForAvailable(90);
            if (cancelScreen == ScreenIndex)
            {
                if (CancelButton713Button.Click() != ResultCode.Passed)
                {
                    return new PluginExecutionResult(PluginResult.Failed, $"Failed to cancel installation at { (object)ScreenName}");
                }

                return new PluginExecutionResult(PluginResult.Skipped, $"Cancelled the installation at {ScreenName}");
            }

            if (NextButton7196Button.Click() != ResultCode.Passed)
            {
                return new PluginExecutionResult(PluginResult.Failed, "Failed to proceed beyond Welcome screen");
            }

            return new PluginExecutionResult(PluginResult.Passed);

        }
        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Desktop Desktop { get; private set; }

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
        public Text WARNINGThisprogText
        {
            get
            {
                if (null == fWARNINGThisprogText)
                {
                    fWARNINGThisprogText = new Text("WARNINGThisprogText", HPDigitalSendinWindow);
                    fWARNINGThisprogText.UIMap.Scope = UIASeachScope.Children;
                    fWARNINGThisprogText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fWARNINGThisprogText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"WARNING: This program is protected by copyright law and international treaties.");
                }

                return fWARNINGThisprogText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text TheInstallShielDup0Text
        {
            get
            {
                if (null == fTheInstallShielDup0Text)
                {
                    fTheInstallShielDup0Text = new Text("TheInstallShielDup0Text", HPDigitalSendinWindow);
                    fTheInstallShielDup0Text.UIMap.Scope = UIASeachScope.Children;
                    fTheInstallShielDup0Text.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fTheInstallShielDup0Text.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"7151");
                    fTheInstallShielDup0Text.UIMap.SearchProperties.Add(UIASearchProperty.Name, $@"The InstallShield(R) Wizard will install HP Digital Sending Software {_version} on your computer. To continue, click Next.");
                }

                return fTheInstallShielDup0Text;
            }
        }

       

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Image NewBinary19StatImage
        {
            get
            {
                if (null == fNewBinary19StatImage)
                {
                    fNewBinary19StatImage = new Image("NewBinary19StatImage", HPDigitalSendinWindow);
                    fNewBinary19StatImage.UIMap.Scope = UIASeachScope.Children;
                    fNewBinary19StatImage.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fNewBinary19StatImage.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8451");
                    fNewBinary19StatImage.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"NewBinary19");
                }

                return fNewBinary19StatImage;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text WelcometotheInsText
        {
            get
            {
                if (null == fWelcometotheInsText)
                {
                    fWelcometotheInsText = new Text("WelcometotheInsText", HPDigitalSendinWindow);
                    fWelcometotheInsText.UIMap.Scope = UIASeachScope.Children;
                    fWelcometotheInsText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Static");
                    fWelcometotheInsText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"8461");
                    fWelcometotheInsText.UIMap.SearchProperties.Add(UIASearchProperty.Name, $@"Welcome to the InstallShield Wizard for HP Digital Sending Software {_version}");
                }

                return fWelcometotheInsText;
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