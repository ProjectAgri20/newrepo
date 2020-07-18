namespace HP.ScalableTest.UI.SessionExecution
{
    partial class ConnectedSessionsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.RadTreeNode radTreeNode1 = new Telerik.WinControls.UI.RadTreeNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectedSessionsControl));
            HP.ScalableTest.UI.SessionExecution.ExecutingSessionInfo executingSessionInfo1 = new HP.ScalableTest.UI.SessionExecution.ExecutingSessionInfo();
            this.sessionExecution_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.executionTreeView_ToolStrip = new System.Windows.Forms.ToolStrip();
            this.connectDispatcher_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.start_ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.sessionStatus_Panel = new System.Windows.Forms.Panel();
            this.splitContainerSessionExecution = new System.Windows.Forms.SplitContainer();
            this.sessionExecution_TreeView = new HP.ScalableTest.UI.SessionExecution.SessionExecutionTreeView();
            this.sessionControl = new HP.ScalableTest.UI.SessionExecution.ControlSessionExecution();
            ((System.ComponentModel.ISupportInitialize)(this.sessionExecution_SplitContainer)).BeginInit();
            this.sessionExecution_SplitContainer.Panel1.SuspendLayout();
            this.sessionExecution_SplitContainer.Panel2.SuspendLayout();
            this.sessionExecution_SplitContainer.SuspendLayout();
            this.executionTreeView_ToolStrip.SuspendLayout();
            this.sessionStatus_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSessionExecution)).BeginInit();
            this.splitContainerSessionExecution.Panel1.SuspendLayout();
            this.splitContainerSessionExecution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sessionExecution_TreeView)).BeginInit();
            this.SuspendLayout();
            // 
            // sessionExecution_SplitContainer
            // 
            this.sessionExecution_SplitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sessionExecution_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessionExecution_SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.sessionExecution_SplitContainer.Name = "sessionExecution_SplitContainer";
            // 
            // sessionExecution_SplitContainer.Panel1
            // 
            this.sessionExecution_SplitContainer.Panel1.Controls.Add(this.sessionExecution_TreeView);
            this.sessionExecution_SplitContainer.Panel1.Controls.Add(this.executionTreeView_ToolStrip);
            // 
            // sessionExecution_SplitContainer.Panel2
            // 
            this.sessionExecution_SplitContainer.Panel2.Controls.Add(this.sessionStatus_Panel);
            this.sessionExecution_SplitContainer.Size = new System.Drawing.Size(1100, 751);
            this.sessionExecution_SplitContainer.SplitterDistance = 363;
            this.sessionExecution_SplitContainer.TabIndex = 1;
            // 
            // executionTreeView_ToolStrip
            // 
            this.executionTreeView_ToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.executionTreeView_ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectDispatcher_ToolStripButton,
            this.start_ToolStripButton});
            this.executionTreeView_ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.executionTreeView_ToolStrip.Name = "executionTreeView_ToolStrip";
            this.executionTreeView_ToolStrip.Size = new System.Drawing.Size(361, 27);
            this.executionTreeView_ToolStrip.TabIndex = 1;
            this.executionTreeView_ToolStrip.Text = "executionTreeView_ToolStrip";
            // 
            // connectDispatcher_ToolStripButton
            // 
            this.connectDispatcher_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("connectDispatcher_ToolStripButton.Image")));
            this.connectDispatcher_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectDispatcher_ToolStripButton.Name = "connectDispatcher_ToolStripButton";
            this.connectDispatcher_ToolStripButton.Size = new System.Drawing.Size(76, 24);
            this.connectDispatcher_ToolStripButton.Text = "Connect";
            this.connectDispatcher_ToolStripButton.Click += new System.EventHandler(this.connectDispatcher_ToolStripButton_Click);
            // 
            // start_ToolStripButton
            // 
            this.start_ToolStripButton.Enabled = false;
            this.start_ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("start_ToolStripButton.Image")));
            this.start_ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.start_ToolStripButton.Name = "start_ToolStripButton";
            this.start_ToolStripButton.Size = new System.Drawing.Size(124, 24);
            this.start_ToolStripButton.Text = "Start New Session";
            this.start_ToolStripButton.Click += new System.EventHandler(this.start_ToolStripButton_Click);
            // 
            // sessionStatus_Panel
            // 
            this.sessionStatus_Panel.Controls.Add(this.splitContainerSessionExecution);
            this.sessionStatus_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessionStatus_Panel.Location = new System.Drawing.Point(0, 0);
            this.sessionStatus_Panel.Name = "sessionStatus_Panel";
            this.sessionStatus_Panel.Size = new System.Drawing.Size(731, 749);
            this.sessionStatus_Panel.TabIndex = 1;
            // 
            // splitContainerSessionExecution
            // 
            this.splitContainerSessionExecution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSessionExecution.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerSessionExecution.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSessionExecution.Name = "splitContainerSessionExecution";
            this.splitContainerSessionExecution.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerSessionExecution.Panel1
            // 
            this.splitContainerSessionExecution.Panel1.Controls.Add(this.sessionControl);
            this.splitContainerSessionExecution.Size = new System.Drawing.Size(731, 749);
            this.splitContainerSessionExecution.SplitterDistance = 150;
            this.splitContainerSessionExecution.TabIndex = 2;
            // 
            // sessionExecution_TreeView
            // 
            this.sessionExecution_TreeView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.sessionExecution_TreeView.Cursor = System.Windows.Forms.Cursors.Default;
            this.sessionExecution_TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessionExecution_TreeView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.sessionExecution_TreeView.ForeColor = System.Drawing.Color.Black;
            this.sessionExecution_TreeView.Location = new System.Drawing.Point(0, 27);
            this.sessionExecution_TreeView.Name = "sessionExecution_TreeView";
            radTreeNode1.Expanded = true;
            radTreeNode1.Image = ((System.Drawing.Image)(resources.GetObject("radTreeNode1.Image")));
            radTreeNode1.ImageKey = "Server";
            radTreeNode1.Name = "dispatcher_Node";
            radTreeNode1.Text = "[Not Connected]";
            this.sessionExecution_TreeView.Nodes.AddRange(new Telerik.WinControls.UI.RadTreeNode[] {
            radTreeNode1});
            this.sessionExecution_TreeView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.sessionExecution_TreeView.RootElement.AccessibleDescription = null;
            this.sessionExecution_TreeView.RootElement.AccessibleName = null;
            this.sessionExecution_TreeView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 27, 150, 250);
            this.sessionExecution_TreeView.SessionManager = null;
            this.sessionExecution_TreeView.Size = new System.Drawing.Size(361, 722);
            this.sessionExecution_TreeView.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.sessionExecution_TreeView.SpacingBetweenNodes = -1;
            this.sessionExecution_TreeView.TabIndex = 0;
            this.sessionExecution_TreeView.Text = "scenarioExecutionTreeView1";
            // 
            // sessionControl
            // 
            this.sessionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sessionControl.Location = new System.Drawing.Point(0, 0);
            this.sessionControl.Name = "sessionControl";
            this.sessionControl.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            executingSessionInfo1.Dispatcher = null;
            executingSessionInfo1.EstimatedEndDate = null;
            executingSessionInfo1.MapElement = null;
            executingSessionInfo1.Name = null;
            executingSessionInfo1.Owner = null;
            executingSessionInfo1.SessionId = "";
            executingSessionInfo1.ShutDownDate = null;
            executingSessionInfo1.StartDate = null;
            executingSessionInfo1.StartupTransition = HP.ScalableTest.Framework.Dispatcher.SessionStartupTransition.None;
            executingSessionInfo1.State = HP.ScalableTest.Framework.Dispatcher.SessionState.Available;
            this.sessionControl.Session = executingSessionInfo1;
            this.sessionControl.Size = new System.Drawing.Size(731, 150);
            this.sessionControl.TabIndex = 0;
            // 
            // ConnectedSessionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sessionExecution_SplitContainer);
            this.Name = "ConnectedSessionsControl";
            this.Size = new System.Drawing.Size(1100, 751);
            this.sessionExecution_SplitContainer.Panel1.ResumeLayout(false);
            this.sessionExecution_SplitContainer.Panel1.PerformLayout();
            this.sessionExecution_SplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sessionExecution_SplitContainer)).EndInit();
            this.sessionExecution_SplitContainer.ResumeLayout(false);
            this.executionTreeView_ToolStrip.ResumeLayout(false);
            this.executionTreeView_ToolStrip.PerformLayout();
            this.sessionStatus_Panel.ResumeLayout(false);
            this.splitContainerSessionExecution.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSessionExecution)).EndInit();
            this.splitContainerSessionExecution.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sessionExecution_TreeView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer sessionExecution_SplitContainer;
        private SessionExecutionTreeView sessionExecution_TreeView;
        private System.Windows.Forms.ToolStrip executionTreeView_ToolStrip;
        private System.Windows.Forms.ToolStripButton connectDispatcher_ToolStripButton;
        private System.Windows.Forms.Panel sessionStatus_Panel;
        private ControlSessionExecution sessionControl;
        private System.Windows.Forms.ToolStripButton start_ToolStripButton;
        private System.Windows.Forms.SplitContainer splitContainerSessionExecution;
    }
}
