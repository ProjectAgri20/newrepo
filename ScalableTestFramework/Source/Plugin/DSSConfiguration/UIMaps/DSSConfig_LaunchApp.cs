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

namespace HP.ScalableTest.Plugin.DSSConfiguration.UIMaps
{
    using TopCat.TestApi.GUIAutomation.Controls;
    using TopCat.TestApi.GUIAutomation.Enums;

    /// <summary>
    /// Application which exercises the TopCat Coded C# test API.
    /// </summary>
    public class DSSConfig_LaunchApp
    {
        /// <summary>
        /// Backing field for HPDigitalSendinWindow property
        /// </summary>
        private Window fHPDigitalSendinWindow;

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
        /// Backing field for Image929341fb5dImage property
        /// </summary>
        private Image fImage929341fb5dImage;

        /// <summary>
        /// Backing field for ConfiguretheDSSGroup property
        /// </summary>
        private Group fConfiguretheDSSGroup;

        /// <summary>
        /// Backing field for ConfiguretheDSSText property
        /// </summary>
        private Text fConfiguretheDSSText;

        /// <summary>
        /// Backing field for ThisComputerRadioButton property
        /// </summary>
        private RadioButton fThisComputerRadRadioButton;

        /// <summary>
        /// Backing field for ThisComputerTexText property
        /// </summary>
        private Text fThisComputerTexText;

        /// <summary>
        /// Backing field for AnotherComputerRadioButton property
        /// </summary>
        private RadioButton fAnotherComputerRadioButton;

        /// <summary>
        /// Backing field for AnotherComputerText property
        /// </summary>
        private Text fAnotherComputerText;

        /// <summary>
        /// Backing field for EnterTheNetworkDup0Text property
        /// </summary>
        private Text fEnterTheNetworkDup0Text;

        /// <summary>
        /// Backing field for EnterTheNetworkDup1Text property
        /// </summary>
        private Text fEnterTheNetworkDup1Text;

        /// <summary>
        /// Backing field for ComboBoxF3F9C4AComboBox property
        /// </summary>
        private ComboBox fComboBoxF3F9C4AComboBox;

        /// <summary>
        /// Backing field for TextBoxPARTEdiEdit property
        /// </summary>
        private Edit fTextBoxPARTEdiEdit;

        /// <summary>
        /// Backing field for ScrollViewerPARPane property
        /// </summary>
        private Pane fScrollViewerPARPane;

        /// <summary>
        /// Backing field for ScrollBarVerticScrollBar property
        /// </summary>
        private ScrollBar fScrollBarVerticScrollBar;

        /// <summary>
        /// Backing field for ScrollBarHorizoScrollBar property
        /// </summary>
        private ScrollBar fScrollBarHorizoScrollBar;

        /// <summary>
        /// Backing field for OKButton property
        /// </summary>
        private Button fOKButton61D7E63Button;

        /// <summary>
        /// Backing field for OKTextBlockText property
        /// </summary>
        private Text fOKTextBlockText;

        /// <summary>
        /// Backing field for CancelButtonAD8Button property
        /// </summary>
        private Button fCancelButtonAD8Button;

        /// <summary>
        /// Backing field for CancelTextBlockText property
        /// </summary>
        private Text fCancelTextBlockText;

        /// <summary>
        /// Backing field for HelpButton3D780Button property
        /// </summary>
        private Button fHelpButton3D780Button;

        /// <summary>
        /// Backing field for HelpTextBlockText property
        /// </summary>
        private Text fHelpTextBlockText;

        /// <summary>
        /// Initializes a new instance of the <see cref="DSSConfig_LaunchApp"/> class.
        /// </summary>
        /// <param name="framework">
        /// The framework.
        /// </param>
        public DSSConfig_LaunchApp(UIAFramework framework)
        {
            Desktop = new Desktop(framework);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DSSConfig_LaunchApp"/> class using the default UIAFramework.
        /// </summary>
        public DSSConfig_LaunchApp()
        {
            Desktop = new Desktop();
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
                    fHPDigitalSendinWindow.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Window");
                    fHPDigitalSendinWindow.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"HP Digital Sending Software Configuration");
                }

                return fHPDigitalSendinWindow;
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
                    fHPDigitalSendinTitleBar.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"HP Digital Sending Software Configuration");
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

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Image Image929341fb5dImage
        {
            get
            {
                if (null == fImage929341fb5dImage)
                {
                    fImage929341fb5dImage = new Image("Image929341fb5dImage", HPDigitalSendinWindow);
                    fImage929341fb5dImage.UIMap.Scope = UIASeachScope.Children;
                    fImage929341fb5dImage.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Image");
                    fImage929341fb5dImage.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"929341fb-5de1-4e52-9468-3bb43d4663be");
                }

                return fImage929341fb5dImage;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Group ConfiguretheDSSGroup
        {
            get
            {
                if (null == fConfiguretheDSSGroup)
                {
                    fConfiguretheDSSGroup = new Group("ConfiguretheDSSGroup", HPDigitalSendinWindow);
                    fConfiguretheDSSGroup.UIMap.Scope = UIASeachScope.Children;
                    fConfiguretheDSSGroup.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"GroupBox");
                    fConfiguretheDSSGroup.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Configure the DSS Settings on");
                }

                return fConfiguretheDSSGroup;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text ConfiguretheDSSText
        {
            get
            {
                if (null == fConfiguretheDSSText)
                {
                    fConfiguretheDSSText = new Text("ConfiguretheDSSText", ConfiguretheDSSGroup);
                    fConfiguretheDSSText.UIMap.Scope = UIASeachScope.Children;
                    fConfiguretheDSSText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"TextBlock");
                    fConfiguretheDSSText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Configure the DSS Settings on");
                }

                return fConfiguretheDSSText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public RadioButton ThisComputerRadioButton
        {
            get
            {
                if (null == fThisComputerRadRadioButton)
                {
                    fThisComputerRadRadioButton = new RadioButton("ThisComputerRadioButton", ConfiguretheDSSGroup);
                    fThisComputerRadRadioButton.UIMap.Scope = UIASeachScope.Children;
                    fThisComputerRadRadioButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"RadioButton");
                    fThisComputerRadRadioButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"This Computer");
                }

                return fThisComputerRadRadioButton;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text ThisComputerTexText
        {
            get
            {
                if (null == fThisComputerTexText)
                {
                    fThisComputerTexText = new Text("ThisComputerTexText", ThisComputerRadioButton);
                    fThisComputerTexText.UIMap.Scope = UIASeachScope.Children;
                    fThisComputerTexText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"TextBlock");
                    fThisComputerTexText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"This Computer");
                }

                return fThisComputerTexText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public RadioButton AnotherComputerRadioButton
        {
            get
            {
                if (null == fAnotherComputerRadioButton)
                {
                    fAnotherComputerRadioButton = new RadioButton("AnotherComputerRadioButton", ConfiguretheDSSGroup);
                    fAnotherComputerRadioButton.UIMap.Scope = UIASeachScope.Children;
                    fAnotherComputerRadioButton.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"RadioButton");
                    fAnotherComputerRadioButton.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"408BA503-C84D-4850-BB4C-FDEAE3CA62DB");
                    fAnotherComputerRadioButton.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Another Computer");
                }

                return fAnotherComputerRadioButton;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text AnotherComputerText
        {
            get
            {
                if (null == fAnotherComputerText)
                {
                    fAnotherComputerText = new Text("AnotherComputerText", AnotherComputerRadioButton);
                    fAnotherComputerText.UIMap.Scope = UIASeachScope.Children;
                    fAnotherComputerText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"TextBlock");
                    fAnotherComputerText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Another Computer");
                }

                return fAnotherComputerText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text EnterTheNetworkDup0Text
        {
            get
            {
                if (null == fEnterTheNetworkDup0Text)
                {
                    fEnterTheNetworkDup0Text = new Text("EnterTheNetworkDup0Text", ConfiguretheDSSGroup);
                    fEnterTheNetworkDup0Text.UIMap.Scope = UIASeachScope.Children;
                    fEnterTheNetworkDup0Text.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Text");
                    fEnterTheNetworkDup0Text.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Enter The Network Name of the PC running the DSS Service");
                }

                return fEnterTheNetworkDup0Text;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text EnterTheNetworkDup1Text
        {
            get
            {
                if (null == fEnterTheNetworkDup1Text)
                {
                    fEnterTheNetworkDup1Text = new Text("EnterTheNetworkDup1Text", EnterTheNetworkDup0Text);
                    fEnterTheNetworkDup1Text.UIMap.Scope = UIASeachScope.Children;
                    fEnterTheNetworkDup1Text.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"TextBlock");
                    fEnterTheNetworkDup1Text.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Enter The Network Name of the PC running the DSS Service");
                }

                return fEnterTheNetworkDup1Text;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public ComboBox ComboBoxF3F9C4AComboBox
        {
            get
            {
                if (null == fComboBoxF3F9C4AComboBox)
                {
                    fComboBoxF3F9C4AComboBox = new ComboBox("ComboBoxF3F9C4AComboBox", ConfiguretheDSSGroup);
                    fComboBoxF3F9C4AComboBox.UIMap.Scope = UIASeachScope.Children;
                    fComboBoxF3F9C4AComboBox.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"ComboBox");
                    fComboBoxF3F9C4AComboBox.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"F3F9C4A8-77F4-4bc0-89ED-194B65248B52");
                }

                return fComboBoxF3F9C4AComboBox;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Edit TextBoxPARTEdiEdit
        {
            get
            {
                if (null == fTextBoxPARTEdiEdit)
                {
                    fTextBoxPARTEdiEdit = new Edit("TextBoxPARTEdiEdit", ComboBoxF3F9C4AComboBox);
                    fTextBoxPARTEdiEdit.UIMap.Scope = UIASeachScope.Children;
                    fTextBoxPARTEdiEdit.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"TextBox");
                    fTextBoxPARTEdiEdit.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"PART_EditableTextBox");
                }

                return fTextBoxPARTEdiEdit;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Pane ScrollViewerPARPane
        {
            get
            {
                if (null == fScrollViewerPARPane)
                {
                    fScrollViewerPARPane = new Pane("ScrollViewerPARPane", TextBoxPARTEdiEdit);
                    fScrollViewerPARPane.UIMap.Scope = UIASeachScope.Children;
                    fScrollViewerPARPane.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"ScrollViewer");
                    fScrollViewerPARPane.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"PART_ContentHost");
                }

                return fScrollViewerPARPane;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public ScrollBar ScrollBarVerticScrollBar
        {
            get
            {
                if (null == fScrollBarVerticScrollBar)
                {
                    fScrollBarVerticScrollBar = new ScrollBar("ScrollBarVerticScrollBar", ScrollViewerPARPane);
                    fScrollBarVerticScrollBar.UIMap.Scope = UIASeachScope.Children;
                    fScrollBarVerticScrollBar.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"ScrollBar");
                    fScrollBarVerticScrollBar.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"VerticalScrollBar");
                }

                return fScrollBarVerticScrollBar;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public ScrollBar ScrollBarHorizoScrollBar
        {
            get
            {
                if (null == fScrollBarHorizoScrollBar)
                {
                    fScrollBarHorizoScrollBar = new ScrollBar("ScrollBarHorizoScrollBar", ScrollViewerPARPane);
                    fScrollBarHorizoScrollBar.UIMap.Scope = UIASeachScope.Children;
                    fScrollBarHorizoScrollBar.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"ScrollBar");
                    fScrollBarHorizoScrollBar.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"HorizontalScrollBar");
                }

                return fScrollBarHorizoScrollBar;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Button OKButton
        {
            get
            {
                if (null == fOKButton61D7E63Button)
                {
                    fOKButton61D7E63Button = new Button("OKButton", HPDigitalSendinWindow);
                    fOKButton61D7E63Button.UIMap.Scope = UIASeachScope.Children;
                    fOKButton61D7E63Button.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fOKButton61D7E63Button.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"OK");
                    fOKButton61D7E63Button.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, "61D7E630-0D53-4c49-A644-389E1AAA1544");
                }

                return fOKButton61D7E63Button;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text OKTextBlockText
        {
            get
            {
                if (null == fOKTextBlockText)
                {
                    fOKTextBlockText = new Text("OKTextBlockText", OKButton);
                    fOKTextBlockText.UIMap.Scope = UIASeachScope.Children;
                    fOKTextBlockText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"TextBlock");
                    fOKTextBlockText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"OK");
                }

                return fOKTextBlockText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Button CancelButtonAD8Button
        {
            get
            {
                if (null == fCancelButtonAD8Button)
                {
                    fCancelButtonAD8Button = new Button("CancelButtonAD8Button", HPDigitalSendinWindow);
                    fCancelButtonAD8Button.UIMap.Scope = UIASeachScope.Children;
                    fCancelButtonAD8Button.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fCancelButtonAD8Button.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"AD8E4568-61B2-4d61-AEED-B8A700C61EF1");
                    fCancelButtonAD8Button.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Cancel");
                }

                return fCancelButtonAD8Button;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text CancelTextBlockText
        {
            get
            {
                if (null == fCancelTextBlockText)
                {
                    fCancelTextBlockText = new Text("CancelTextBlockText", CancelButtonAD8Button);
                    fCancelTextBlockText.UIMap.Scope = UIASeachScope.Children;
                    fCancelTextBlockText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"TextBlock");
                    fCancelTextBlockText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Cancel");
                }

                return fCancelTextBlockText;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Button HelpButton3D780Button
        {
            get
            {
                if (null == fHelpButton3D780Button)
                {
                    fHelpButton3D780Button = new Button("HelpButton3D780Button", HPDigitalSendinWindow);
                    fHelpButton3D780Button.UIMap.Scope = UIASeachScope.Children;
                    fHelpButton3D780Button.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"Button");
                    fHelpButton3D780Button.UIMap.SearchProperties.Add(UIASearchProperty.AutomationId, @"3D7805CE-4B40-49a4-91A2-E8272FE97B46");
                    fHelpButton3D780Button.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Help");
                }

                return fHelpButton3D780Button;
            }
        }

        /// <summary>
        /// Gets Reference to desktop
        /// </summary>
        public Text HelpTextBlockText
        {
            get
            {
                if (null == fHelpTextBlockText)
                {
                    fHelpTextBlockText = new Text("HelpTextBlockText", HelpButton3D780Button);
                    fHelpTextBlockText.UIMap.Scope = UIASeachScope.Children;
                    fHelpTextBlockText.UIMap.SearchProperties.Add(UIASearchProperty.ClassName, @"TextBlock");
                    fHelpTextBlockText.UIMap.SearchProperties.Add(UIASearchProperty.Name, @"Help");
                }

                return fHelpTextBlockText;
            }
        }
    }
}
