using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.LabConsole
{
    public partial class AboutBox : Form
    {


        public AboutBox()
        {
            InitializeComponent();
            UserInterfaceStyler.Configure(this, FormStyle.FixedDialog);


            this.Text = String.Format(CultureInfo.CurrentCulture, "About {0}", AssemblyProperties.Title);
            var versionInfo = new List<string>();
            versionInfo.Add(AssemblyProperties.Product);
            versionInfo.Add(String.Format(CultureInfo.CurrentCulture, "Version {0}", AssemblyProperties.Version));
            versionInfo.Add(AssemblyProperties.Copyright);
            versionInfo.Add(AssemblyProperties.Company);
            var installedPrograms = InstalledAppHelper.GetInstalledPrograms().OrderBy(x=>x.DisplayName);
            var stb = installedPrograms.FirstOrDefault(x => x.DisplayName.StartsWith("HP Solution Test Bench"));
            if (stb != null)
            {
                versionInfo.Add("Installed Application:");
                versionInfo.Add("  {0}, {1}".FormatWith(stb.DisplayName, stb.DisplayVersion));
            }

            textBoxVersionInfo.Text = string.Join(Environment.NewLine, versionInfo);
            textBoxDescription.Text = AssemblyProperties.Description;
        }

        //#region Assembly Attribute Accessors

        
    }
}
