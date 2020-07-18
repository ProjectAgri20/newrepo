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

namespace HP.ScalableTest.Plugin.ConnectivityPrint.UIMaps
{
    using TopCat.TestApi.GUIAutomation.Controls;
    using TopCat.TestApi.GUIAutomation.Enums;

    /// <summary>
    /// Application which exercises the TopCat Coded C# test API.
    /// </summary>
    public class WindowsSecurityToInstall
    {
        /// <summary>
        /// Backing field for WindowsSecurityWindow property
        /// </summary>
        private Window fWindowsSecurityWindow;

        /// <summary>
        /// Backing field for WindowsSecurityPane property
        /// </summary>
        private Pane fWindowsSecurityPane;

        /// <summary>
        /// Backing field for CtrlNotifySinkI0XPane property
        /// </summary>
        private Pane fCtrlNotifySinkI0XPane;

        /// <summary>
        /// Backing field for CtrlNotifySinkI1XPane property
        /// </summary>
        private Pane fCtrlNotifySinkI1XPane;

        /// <summary>
        /// Backing field for CtrlNotifySinkI2XPane property
        /// </summary>
        private Pane fCtrlNotifySinkI2XPane;

        /// <summary>
        /// Backing field for CtrlNotifySinkI3XPane property
        /// </summary>
        private Pane fCtrlNotifySinkI3XPane;

        /// <summary>
        /// Backing field for WouldyouliketoiText property
        /// </summary>
        private Text fWouldyouliketoiText;

        /// <summary>
        /// Backing field for ElementContentText property
        /// </summary>
        private Text fElementContentText;

        /// <summary>
        /// Backing field for XElementPane property
        /// </summary>
        private Pane fXElementPane;

        /// <summary>
        /// Backing field for XBabyPane property
        /// </summary>
        private Pane fXBabyPane;

        /// <summary>
        /// Backing field for CCSysLinkContenHyperlink property
        /// </summary>
        private Hyperlink fCCSysLinkContenHyperlink;

        /// <summary>
        /// Backing field for AlwaystrustsoftCheckBox property
        /// </summary>
        private CheckBox fAlwaystrustsoftCheckBox;

        /// <summary>
        /// Backing field for InstallCCPushBuButton property
        /// </summary>
        private Button fInstallCCPushBuButton;

        /// <summary>
        /// Backing field for DontInstallCCPuButton property
        /// </summary>
        private Button fDontInstallCCPuButton;

        /// <summary>
        /// Backing field for FooterIconElemeImage property
        /// </summary>
        private Image fFooterIconElemeImage;

        /// <summary>
        /// Backing field for YoushouldonlyinHyperlink property
        /// </summary>
        private Hyperlink fYoushouldonlyinHyperlink;

        /// <summary>
        /// Backing field for AHyperlink property
        /// </summary>
        private Hyperlink fAHyperlink;

        /// <summary>
        /// Backing field for WindowsSecurityTitleBar property
        /// </summary>
        private TitleBar fWindowsSecurityTitleBar;

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
        /// Initializes a new instance of the <see cref="WindowsSecurityToInstall"/> class.
        /// </summary>
        /// <param name="framework">
        /// The framework.
        /// </param>
        public WindowsSecurityToInstall(UIAFramework framework)
        {
            Desktop = new Desktop(framework);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsSecurityToInstall"/> class using the default UIAFramework.
        /// </summary>
        public WindowsSecurityToInstall()
        {
            Desktop = new Desktop();
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Desktop Desktop { get; private set; }

        /// <summary>
        /// Gets Reference to WindowsSecurityWindow
        /// </summary>
        public Window WindowsSecurityWindow
        {
            get
            {
                if (null == fWindowsSecurityWindow)
                {
                    fWindowsSecurityWindow = new Window("WindowsSecurityWindow", Desktop);
                    fWindowsSecurityWindow.UIMap.Scope = UIASeachScope.Children;
                    fWindowsSecurityWindow.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"#32770");
                    fWindowsSecurityWindow.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Windows Security");
                }

                return fWindowsSecurityWindow;
            }
        }

        /// <summary>
        /// Gets Reference to WindowsSecurityPane
        /// </summary>
        public Pane WindowsSecurityPane
        {
            get
            {
                if (null == fWindowsSecurityPane)
                {
                    fWindowsSecurityPane = new Pane("WindowsSecurityPane", WindowsSecurityWindow);
                    fWindowsSecurityPane.UIMap.Scope = UIASeachScope.Children;
                    fWindowsSecurityPane.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"TaskDialog");
                    fWindowsSecurityPane.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Windows Security");
                    fWindowsSecurityPane.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"Window");
                }

                return fWindowsSecurityPane;
            }
        }

        /// <summary>
        /// Gets Reference to CtrlNotifySinkI0XPane
        /// </summary>
        public Pane CtrlNotifySinkI0XPane
        {
            get
            {
                if (null == fCtrlNotifySinkI0XPane)
                {
                    fCtrlNotifySinkI0XPane = new Pane("CtrlNotifySinkI0XPane", WindowsSecurityPane);
                    fCtrlNotifySinkI0XPane.UIMap.Scope = UIASeachScope.Children;
                    fCtrlNotifySinkI0XPane.UIMap.Index = 0;
                    fCtrlNotifySinkI0XPane.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"CtrlNotifySink");
                }

                return fCtrlNotifySinkI0XPane;
            }
        }

        /// <summary>
        /// Gets Reference to CtrlNotifySinkI1XPane
        /// </summary>
        public Pane CtrlNotifySinkI1XPane
        {
            get
            {
                if (null == fCtrlNotifySinkI1XPane)
                {
                    fCtrlNotifySinkI1XPane = new Pane("CtrlNotifySinkI1XPane", WindowsSecurityPane);
                    fCtrlNotifySinkI1XPane.UIMap.Scope = UIASeachScope.Children;
                    fCtrlNotifySinkI1XPane.UIMap.Index = 1;
                    fCtrlNotifySinkI1XPane.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"CtrlNotifySink");
                }

                return fCtrlNotifySinkI1XPane;
            }
        }

        /// <summary>
        /// Gets Reference to CtrlNotifySinkI2XPane
        /// </summary>
        public Pane CtrlNotifySinkI2XPane
        {
            get
            {
                if (null == fCtrlNotifySinkI2XPane)
                {
                    fCtrlNotifySinkI2XPane = new Pane("CtrlNotifySinkI2XPane", WindowsSecurityPane);
                    fCtrlNotifySinkI2XPane.UIMap.Scope = UIASeachScope.Children;
                    fCtrlNotifySinkI2XPane.UIMap.Index = 2;
                    fCtrlNotifySinkI2XPane.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"CtrlNotifySink");
                }

                return fCtrlNotifySinkI2XPane;
            }
        }

        /// <summary>
        /// Gets Reference to CtrlNotifySinkI3XPane
        /// </summary>
        public Pane CtrlNotifySinkI3XPane
        {
            get
            {
                if (null == fCtrlNotifySinkI3XPane)
                {
                    fCtrlNotifySinkI3XPane = new Pane("CtrlNotifySinkI3XPane", WindowsSecurityPane);
                    fCtrlNotifySinkI3XPane.UIMap.Scope = UIASeachScope.Children;
                    fCtrlNotifySinkI3XPane.UIMap.Index = 3;
                    fCtrlNotifySinkI3XPane.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"CtrlNotifySink");
                }

                return fCtrlNotifySinkI3XPane;
            }
        }

        /// <summary>
        /// Gets Reference to WouldyouliketoiText
        /// </summary>
        public Text WouldyouliketoiText
        {
            get
            {
                if (null == fWouldyouliketoiText)
                {
                    fWouldyouliketoiText = new Text("WouldyouliketoiText", WindowsSecurityPane);
                    fWouldyouliketoiText.UIMap.Scope = UIASeachScope.Children;
                    fWouldyouliketoiText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Element");
                    fWouldyouliketoiText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Would you like to install this device software?");
                    fWouldyouliketoiText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"MainInstruction");
                }

                return fWouldyouliketoiText;
            }
        }

        /// <summary>
        /// Gets Reference to ElementContentText
        /// </summary>
        public Text ElementContentText
        {
            get
            {
                if (null == fElementContentText)
                {
                    fElementContentText = new Text("ElementContentText", WindowsSecurityPane);
                    fElementContentText.UIMap.Scope = UIASeachScope.Children;
                    fElementContentText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Element");
                    fElementContentText.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"ContentText");
                }

                return fElementContentText;
            }
        }

        /// <summary>
        /// Gets Reference to XElementPane
        /// </summary>
        public Pane XElementPane
        {
            get
            {
                if (null == fXElementPane)
                {
                    fXElementPane = new Pane("XElementPane", ElementContentText);
                    fXElementPane.UIMap.Scope = UIASeachScope.Children;
                    fXElementPane.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"XElement");
                }

                return fXElementPane;
            }
        }

        /// <summary>
        /// Gets Reference to XBabyPane
        /// </summary>
        public Pane XBabyPane
        {
            get
            {
                if (null == fXBabyPane)
                {
                    fXBabyPane = new Pane("XBabyPane", XElementPane);
                    fXBabyPane.UIMap.Scope = UIASeachScope.Children;
                    fXBabyPane.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"XBaby");
                }

                return fXBabyPane;
            }
        }

        /// <summary>
        /// Gets Reference to CCSysLinkContenHyperlink
        /// </summary>
        public Hyperlink CCSysLinkContenHyperlink
        {
            get
            {
                if (null == fCCSysLinkContenHyperlink)
                {
                    fCCSysLinkContenHyperlink = new Hyperlink("CCSysLinkContenHyperlink", WindowsSecurityPane);
                    fCCSysLinkContenHyperlink.UIMap.Scope = UIASeachScope.Children;
                    fCCSysLinkContenHyperlink.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"CCSysLink");
                    fCCSysLinkContenHyperlink.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"ContentLink");
                }

                return fCCSysLinkContenHyperlink;
            }
        }

        /// <summary>
        /// Gets Reference to AlwaystrustsoftCheckBox
        /// </summary>
        public CheckBox AlwaystrustsoftCheckBox
        {
            get
            {
                if (null == fAlwaystrustsoftCheckBox)
                {
                    fAlwaystrustsoftCheckBox = new CheckBox("AlwaystrustsoftCheckBox", WindowsSecurityPane);
                    fAlwaystrustsoftCheckBox.UIMap.Scope = UIASeachScope.Children;
                    fAlwaystrustsoftCheckBox.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"CheckBoxGlyph");
                    fAlwaystrustsoftCheckBox.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Always trust software from Hewlett-Packard Company.");
                    fAlwaystrustsoftCheckBox.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"VerificationCheckBox");
                }

                return fAlwaystrustsoftCheckBox;
            }
        }

        /// <summary>
        /// Gets Reference to InstallCCPushBuButton
        /// </summary>
        public Button InstallCCPushBuButton
        {
            get
            {
                if (null == fInstallCCPushBuButton)
                {
                    fInstallCCPushBuButton = new Button("InstallCCPushBuButton", WindowsSecurityPane);
                    fInstallCCPushBuButton.UIMap.Scope = UIASeachScope.Children;
                    fInstallCCPushBuButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"CCPushButton");
                    fInstallCCPushBuButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Install");
                    fInstallCCPushBuButton.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"CommandButton_1");
                }

                return fInstallCCPushBuButton;
            }
        }

        /// <summary>
        /// Gets Reference to DontInstallCCPuButton
        /// </summary>
        public Button DontInstallCCPuButton
        {
            get
            {
                if (null == fDontInstallCCPuButton)
                {
                    fDontInstallCCPuButton = new Button("DontInstallCCPuButton", WindowsSecurityPane);
                    fDontInstallCCPuButton.UIMap.Scope = UIASeachScope.Children;
                    fDontInstallCCPuButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"CCPushButton");
                    fDontInstallCCPuButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Don't Install");
                    fDontInstallCCPuButton.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"CommandButton_2");
                }

                return fDontInstallCCPuButton;
            }
        }

        /// <summary>
        /// Gets Reference to FooterIconElemeImage
        /// </summary>
        public Image FooterIconElemeImage
        {
            get
            {
                if (null == fFooterIconElemeImage)
                {
                    fFooterIconElemeImage = new Image("FooterIconElemeImage", WindowsSecurityPane);
                    fFooterIconElemeImage.UIMap.Scope = UIASeachScope.Children;
                    fFooterIconElemeImage.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Element");
                    fFooterIconElemeImage.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"FooterIcon");
                    fFooterIconElemeImage.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"FootnoteIcon");
                }

                return fFooterIconElemeImage;
            }
        }

        /// <summary>
        /// Gets Reference to YoushouldonlyinHyperlink
        /// </summary>
        public Hyperlink YoushouldonlyinHyperlink
        {
            get
            {
                if (null == fYoushouldonlyinHyperlink)
                {
                    fYoushouldonlyinHyperlink = new Hyperlink("YoushouldonlyinHyperlink", WindowsSecurityPane);
                    fYoushouldonlyinHyperlink.UIMap.Scope = UIASeachScope.Children;
                    fYoushouldonlyinHyperlink.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"CCSysLink");
                    fYoushouldonlyinHyperlink.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"You should only install driver software from publishers you trust.  How can I decide which device software is safe to install?");
                    fYoushouldonlyinHyperlink.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"FootnoteTextLink");
                }

                return fYoushouldonlyinHyperlink;
            }
        }

        /// <summary>
        /// Gets Reference to AHyperlink
        /// </summary>
        public Hyperlink AHyperlink
        {
            get
            {
                if (null == fAHyperlink)
                {
                    fAHyperlink = new Hyperlink("AHyperlink", YoushouldonlyinHyperlink);
                    fAHyperlink.UIMap.Scope = UIASeachScope.Children;
                }

                return fAHyperlink;
            }
        }

        /// <summary>
        /// Gets Reference to WindowsSecurityTitleBar
        /// </summary>
        public TitleBar WindowsSecurityTitleBar
        {
            get
            {
                if (null == fWindowsSecurityTitleBar)
                {
                    fWindowsSecurityTitleBar = new TitleBar("WindowsSecurityTitleBar", WindowsSecurityWindow);
                    fWindowsSecurityTitleBar.UIMap.Scope = UIASeachScope.Children;
                    fWindowsSecurityTitleBar.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Windows Security");
                    fWindowsSecurityTitleBar.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"TitleBar");
                }

                return fWindowsSecurityTitleBar;
            }
        }

        /// <summary>
        /// Gets Reference to SystemMenuBarSyMenuBar
        /// </summary>
        public MenuBar SystemMenuBarSyMenuBar
        {
            get
            {
                if (null == fSystemMenuBarSyMenuBar)
                {
                    fSystemMenuBarSyMenuBar = new MenuBar("SystemMenuBarSyMenuBar", WindowsSecurityTitleBar);
                    fSystemMenuBarSyMenuBar.UIMap.Scope = UIASeachScope.Children;
                    fSystemMenuBarSyMenuBar.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"System Menu Bar");
                    fSystemMenuBarSyMenuBar.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"SystemMenuBar");
                }

                return fSystemMenuBarSyMenuBar;
            }
        }

        /// <summary>
        /// Gets Reference to SystemItem1MenuItem
        /// </summary>
        public MenuItem SystemItem1MenuItem
        {
            get
            {
                if (null == fSystemItem1MenuItem)
                {
                    fSystemItem1MenuItem = new MenuItem("SystemItem1MenuItem", SystemMenuBarSyMenuBar);
                    fSystemItem1MenuItem.UIMap.Scope = UIASeachScope.Children;
                    fSystemItem1MenuItem.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"System");
                    fSystemItem1MenuItem.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"Item 1");
                }

                return fSystemItem1MenuItem;
            }
        }

        /// <summary>
        /// Gets Reference to CloseCloseButton
        /// </summary>
        public Button CloseCloseButton
        {
            get
            {
                if (null == fCloseCloseButton)
                {
                    fCloseCloseButton = new Button("CloseCloseButton", WindowsSecurityTitleBar);
                    fCloseCloseButton.UIMap.Scope = UIASeachScope.Children;
                    fCloseCloseButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Close");
                    fCloseCloseButton.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"Close");
                }

                return fCloseCloseButton;
            }
        }
    }
}