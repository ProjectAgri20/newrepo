using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{

    /// <summary>
    /// Generic Map element control, which displays the status for virtual resource, session, etc.
    /// </summary>

    public partial class ElementInfoContainerControl : Control
    {
        string _sessionId;
        SessionInfo _sessionInfo = null;
        SessionMapElement _element = null;
        ControlSessionExecution _sessionExecutionControl = null;
        ElementInfoControlBase _elementInfoControl = null;

        IElementInfoControl ElementControlInterface { get { return _elementInfoControl as IElementInfoControl; } }

        /// <summary>
        /// constructor
        /// </summary>
        public ElementInfoContainerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes this instance with the specified <see cref="SessionMapElement" />.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="sessionControl">The session execution control used to start/stop etc.</param>
        /// <exception cref="ControlTypeMismatchException">
        /// Thrown when an object of incorrect type is passed to this instance.
        /// </exception>
        public void Initialize(SessionMapElement element, ControlSessionExecution sessionControl)
        {
            try
            {
                if (element == null || sessionControl == null)
                {
                    throw new ArgumentNullException("Element and SessionControl arguments must not be null");
                }

                _element = element;
                _sessionId = element.SessionId;
                RefreshSessionInfo();

                _sessionExecutionControl = sessionControl;
                _sessionExecutionControl.RefreshRequested += _sessionExecutionControl_RefreshRequested;

                Task.Factory.StartNew(() => LoadElementInfoControl(element, _sessionInfo));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Initialize", ex);
            }
        }

        public void Refresh(SessionControlRefreshEventArgs args)
        {
            if (args != null && ElementControlInterface != null)
            {
                TraceFactory.Logger.Debug("_sessionExecutionControl_RefreshRequested");
                Task.Factory.StartNew(() => RefreshElementControlData(_elementInfoControl));
            }
        }

        /// <summary>
        /// Loads the element information control into the display
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="sessionInfo">The session information.</param>
        private void LoadElementInfoControl(SessionMapElement element, SessionInfo sessionInfo)
        {
            _elementInfoControl = null;
            var title = "{0} {1}".FormatWith(element.ElementSubtype, element.Name);

            groupBox_ElementInfo.InvokeIfRequired(c =>
                {
                    radPanel_ElementInfoHolder.Text = string.Empty;
                    groupBox_ElementInfo.Text = "Loading {0}...".FormatWith(title);
                });

            var elementInfoControl = ObjectFactory.Create<ElementInfoControlBase>(element.ElementType);
            elementInfoControl.Initialize(element, sessionInfo);

            var revisedTitle = elementInfoControl.GetTitle();
            radPanel_ElementInfoHolder.InvokeIfRequired(c =>
                {
                    this.SuspendLayout();
                    elementInfoControl.Dock = DockStyle.Fill;
                    radPanel_ElementInfoHolder.Controls.Add(elementInfoControl);

                    if (!string.IsNullOrEmpty(revisedTitle) && revisedTitle != title)
                    {
                        groupBox_ElementInfo.Text = revisedTitle;
                    }
                    else
                    {
                        groupBox_ElementInfo.Text = title;
                    }
                    this.ResumeLayout();
                }
            );
            _elementInfoControl = elementInfoControl;
        }

        protected virtual void _sessionExecutionControl_RefreshRequested(object sender, SessionControlRefreshEventArgs e)
        {
            if (e != null && ElementControlInterface != null)
            {
                TraceFactory.Logger.Debug("_sessionExecutionControl_RefreshRequested");
                Task.Factory.StartNew(() => RefreshElementControlData(_elementInfoControl));
            }
        }

        /// <summary>
        /// Refreshes the control data.
        /// </summary>
        /// <param name="control">The control.</param>
        private void RefreshElementControlData(ElementInfoControlBase control)
        {
            try
            {
                RefreshSessionInfo();
                if (control != null && control is IElementInfoControl)
                {
                    control.InvokeIfRequired(c => control.RefreshData());
                }
            }
            catch (ObjectDisposedException) { } // ignore
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// refreshes data from Database
        /// </summary>
        protected void RefreshSessionInfo()
        {
            using (DataLogContext context = DbConnect.DataLogContext())
            {
                _sessionInfo = context.Sessions.FirstOrDefault(s => s.SessionId == _sessionId);
            }
        }
    }
}
