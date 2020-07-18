using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HP.ScalableTest.Print;

namespace WSPrint
{
    public partial class WSPrintUI : Form
    {
        public WSPrintUI()
        {
            InitializeComponent();

            LocalPrintDevice lpd = new LocalPrintDevice();
        }
    }
}
