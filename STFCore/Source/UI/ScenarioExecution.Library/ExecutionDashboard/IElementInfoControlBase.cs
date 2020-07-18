using System;
using System.Windows.Forms;

namespace HP.ScalableTest.UI.SessionExecution
{
    public interface IElementInfoControl
    {
        string GetTitle();

        void RefreshData();
    }
}