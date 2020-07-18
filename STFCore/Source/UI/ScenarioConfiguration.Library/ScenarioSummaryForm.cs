using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
	/// <summary>
	/// A form that displays configuration details for a given scenario.
	/// </summary>
	public partial class ScenarioSummaryForm : Form
	{
		private const string REPEAT_COUNT_COLUMN = "RepeatCount";
		private const string EXECUTION_MODE_COLUMN = "ExecutionMode";
		private const string DURATION_TIME_COLUMN = "DurationTime";

		private Guid _scenarioId;
		private EnterpriseTestContext _context;
		private List<VirtualResource> _resources;

		private IQueryable<VirtualResourceMetadataAssetUsage> _assetMetadataUsages;
		private IQueryable<VirtualResourceMetadataPrintQueueUsage> _printQueueMetadataUsages;

		private BulkDeviceList _bulkDeviceList;
		private BulkDeviceControl _bulkDeviceControl;

		private BulkPrintQueueList _bulkPrintQueueList;
		private BulkPrintQueueControl _bulkPrintQueueControl;

		public EnterpriseScenario ModifiedScenario;


		/// <summary>
		/// Initializes a new instance of the <see cref="ScenarioSummaryForm"/> class
		/// </summary>
		/// <param name="scenarioId">The EnterpriseScenarioId of the scenario to be displayed</param>
		public ScenarioSummaryForm(Guid scenarioId, int pageIndex)
		{
			_scenarioId = scenarioId;

			InitializeComponent();

			// Office Workers
			LoadVirtualResources();
			LoadDataByVirtualResourceType();

			// Scenario Devices
			GetScenarioResources();
			BulkDeviceAddNewPage();
			BulkPrintQueueAddNewPage();

			// set tab view based on user selection
			scenario_radPageView.SelectedPage = this.scenario_radPageView.Pages[pageIndex];
		}

		/// <summary>
		/// Adds the Bulk device tab to the page view.
		/// </summary>
		private void BulkDeviceAddNewPage()
		{
			_bulkDeviceControl = new BulkDeviceControl(_bulkDeviceList);

			RadPageViewPage newPage = new RadPageViewPage();
			newPage.Controls.Add(_bulkDeviceControl);
			newPage.Text = "Bulk Replace";

			scenario_radPageView.Pages.Add(newPage);
		}
		/// <summary>
		/// Adds the BulkPrintQueue tab to the page view.
		/// </summary>
		private void BulkPrintQueueAddNewPage()
		{
			_bulkPrintQueueControl = new BulkPrintQueueControl(_bulkPrintQueueList);

			RadPageViewPage newPage = new RadPageViewPage();
			newPage.Controls.Add(_bulkPrintQueueControl);
			newPage.Text = "Bulk PrintQueue Replace";

			scenario_radPageView.Pages.Add(newPage);
		}

		/// <summary>
		/// Gets the devices utilized by the scenario
		/// </summary>
		private void GetScenarioResources()
		{
			_assetMetadataUsages = _context.VirtualResourceMetadataAssetUsages.Where(n => n.VirtualResourceMetadata.VirtualResource.EnterpriseScenarioId == _scenarioId);
			_printQueueMetadataUsages = _context.VirtualResourceMetadataPrintQueueUsages.Where(n => n.VirtualResourceMetadata.VirtualResource.EnterpriseScenarioId == _scenarioId);

			SetBulkDevice();
			SetBulkPrintQueues();


		}

		private void SetBulkDevice()
		{
			_bulkDeviceList = new BulkDeviceList();
			foreach (VirtualResourceMetadataAssetUsage vrmau in _assetMetadataUsages)
			{
				XElement asd = XElement.Parse(vrmau.AssetSelectionData);
				AssetSelectionData assetSelectionData = Serializer.Deserialize<AssetSelectionData>(asd);

				foreach (string assetId in assetSelectionData.SelectedAssets)
				{
					if (AssetNotInList(assetId))
					{
						BulkDeviceEnt bulkDevice = new BulkDeviceEnt();

						bulkDevice.CurrentDevice = assetId;
						bulkDevice.Active = IsActiveAsset(assetId, assetSelectionData.InactiveAssets);
						bulkDevice.VirtualResourceMetadataId = vrmau.VirtualResourceMetadataId;

						_bulkDeviceList.Add(bulkDevice);
					}
				}
			}
		}
		private void SetBulkPrintQueues()
		{
			_bulkPrintQueueList = new BulkPrintQueueList();
			foreach (VirtualResourceMetadataPrintQueueUsage vrmpqu in _printQueueMetadataUsages)
			{
				XElement pqsd = XElement.Parse(vrmpqu.PrintQueueSelectionData);
				PrintQueueSelectionData printQueueSelectionData = Serializer.Deserialize<PrintQueueSelectionData>(pqsd);

                var allRemote = ConfigurationServices.AssetInventory.GetRemotePrintQueues();
				foreach (RemotePrintQueueDefinition printQueueId in printQueueSelectionData.SelectedPrintQueues.Where(x => x.GetType() == typeof(RemotePrintQueueDefinition)))
				{
					var remoteQueues = allRemote.Where(x => x.PrintQueueId == printQueueId.PrintQueueId).Select(x => new { x.QueueName, x.ServerHostName });

                    if (remoteQueues.Count() == 0)
                    {
                        continue;
                    }

					if (PrintQueueNotInList(printQueueId.PrintQueueId.ToString()))
					{
						BulkPrintQueueEnt bulkqueue = new BulkPrintQueueEnt();
                        bulkqueue.OldHostName = remoteQueues.FirstOrDefault().ServerHostName;
						bulkqueue.CurrentQueue = remoteQueues.FirstOrDefault().QueueName;
						bulkqueue.Active = true;
						bulkqueue.VirtualResourceMetadataId = vrmpqu.VirtualResourceMetadataId;

						if (!_bulkPrintQueueList.Select(x =>x.CurrentQueue).Contains(bulkqueue.CurrentQueue))
						{
							_bulkPrintQueueList.Add(bulkqueue);
						}
					}
				}



			}
		}

		private bool PrintQueueNotInList(string queueId)
		{
			bool notInList = true;
			foreach (BulkPrintQueueEnt queue in _bulkPrintQueueList)
			{
				if(queue.CurrentQueue.Equals(queueId))
				{
					notInList = false;
					break;
				}
			}

			return notInList;
		}



		private bool AssetNotInList(string assetId)
		{
			bool notInList = true;

			foreach(BulkDeviceEnt device in _bulkDeviceList)
			{
				if(device.CurrentDevice.Equals(assetId))
				{
					notInList = false;
					break;
				}
			}

			return notInList;
		}
		


		private bool IsActiveAsset(string assetId, AssetIdCollection inActiveAssetIds)
		{
			bool isActive = true;

			foreach(string id in inActiveAssetIds)
			{
				if(id.Equals(assetId))
				{
					isActive = false;
					break;
				}
			}
			return isActive;
		}

		private void LoadVirtualResources()
		{
			_context = new EnterpriseTestContext();

			ModifiedScenario = EnterpriseScenario.Select(_context, _scenarioId);
			
			_resources = ModifiedScenario.VirtualResources.ToList();

			this.Text = ModifiedScenario.Name;
		}

		private List<string> GetResourceTypes()
		{
			return (from r in _resources select r.ResourceType).Distinct().ToList();
		}

		private void LoadDataByVirtualResourceType()
		{
			foreach (string resourceType in GetResourceTypes())
			{
				List<dynamic> listOfType = new List<dynamic>();

				_resources.Where(r => r.ResourceType.Equals(resourceType)).ToList().ForEach(t =>
				{
					Convert.ChangeType(t, t.GetType());
					listOfType.Add(t);
				});

				CreateNewPageWithGridView(listOfType, resourceType);
			}
		}

		private void CreateNewPageWithGridView<T>(List<T> list, string resourceType)
		{
			RadGridView newGrid = GridViewFactory(list, resourceType);

			RadPageViewPage newPage = new RadPageViewPage();
			newPage.Text = resourceType;
			newPage.Controls.Add(newGrid);

			scenario_radPageView.Pages.Add(newPage);
		}

		private RadGridView GridViewFactory<T>(List<T> list, string resourceType)
		{
			RadGridView newGrid = new RadGridView();

			newGrid.SelectionMode = GridViewSelectionMode.CellSelect;
			newGrid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
			newGrid.MultiSelect = true;
			newGrid.EnableFiltering = true;
			newGrid.AllowAddNewRow = false;
			newGrid.AllowDeleteRow = false;
			newGrid.EnableGrouping = false;
			newGrid.ReadOnly = false;
			newGrid.AutoSize = true;
			newGrid.ShowRowHeaderColumn = false;

			newGrid.Name = resourceType;
			newGrid.DataSource = list;

			newGrid.Resize += newGrid_Resize;
			newGrid.DataBindingComplete += newGrid_DataBindingComplete;

			newGrid.CellFormatting += newGrid_CellFormatting;
			newGrid.CellEndEdit += newGrid_CellEndEdit;
			newGrid.CommandCellClick += newGrid_CommandCellClick;
			newGrid.ToolTipTextNeeded += newGrid_ToolTipTextNeeded;

			PopulateMedatdataRows(newGrid);

			return newGrid;
		}

		private void PopulateMedatdataRows(RadGridView gridView)
		{
			GridViewTemplate metadataTemplate = new GridViewTemplate();
			metadataTemplate.DataSource = _resources.SelectMany(r => r.VirtualResourceMetadataSet).Select(n => n);

			metadataTemplate.Columns.Add(CreateMetadataCommandColumn());
			metadataTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
			metadataTemplate.Caption = "Metadata";
			metadataTemplate.AllowAddNewRow = false;
			metadataTemplate.ReadOnly = false;
			metadataTemplate.ShowRowHeaderColumn = false;

			HideColumns(metadataTemplate);
			SetColumnsAsReadOnly(metadataTemplate);

			GridViewRelation metadataRelation = new GridViewRelation(gridView.MasterTemplate);

			metadataRelation.ChildTemplate = metadataTemplate;
			metadataRelation.RelationName = "VirtualResourceId";
			metadataRelation.ParentColumnNames.Add("VirtualResourceId");
			metadataRelation.ChildColumnNames.Add("VirtualResourceId");

			gridView.Relations.Add(metadataRelation);
			gridView.ShowChildViewCaptions = true;
			gridView.Templates.Add(metadataTemplate);
		}

		private GridViewCommandColumn CreateMetadataCommandColumn()
		{
			GridViewCommandColumn viewMetadata = new GridViewCommandColumn();
			viewMetadata.Name = "ViewMetadata";
			viewMetadata.FieldName = "View Metadata";
			viewMetadata.HeaderText = "View Metadata";
			viewMetadata.UseDefaultText = true;
			viewMetadata.DefaultText = "Click to View Metadata";
			viewMetadata.MaxWidth = 200;

			return viewMetadata;
		}

		private void newGrid_CellFormatting(object sender, CellFormattingEventArgs e)
		{
			if (ReadOnlyProperties().Contains(e.CellElement.ColumnInfo.HeaderText))
			{
				DisableCellEdit(e, String.Empty);
			}
			else if(e.CellElement.ColumnInfo.Name == "MaxActivityDelay")
			{
				FormatDelayCells(e, "RandomizeActivityDelay", "MaxActivityDelay");
			}
			else if (e.CellElement.ColumnInfo.Name == "MaxStartupDelay")
			{
				FormatDelayCells(e, "RandomizeStartupDelay", "MaxStartupDelay");
			}
			else if (e.CellElement.ColumnInfo.Name == REPEAT_COUNT_COLUMN)
			{
				FormatExecutionModeCells(e, REPEAT_COUNT_COLUMN);
			}
			else if (e.CellElement.ColumnInfo.Name == DURATION_TIME_COLUMN)
			{
				FormatExecutionModeCells(e, DURATION_TIME_COLUMN);
			}
			else
			{
				EnableCellEdit(e, String.Empty, false);
			}
		}

		private void FormatExecutionModeCells(CellFormattingEventArgs e, string affectedColumn)
		{
			if (e.Row.Cells[EXECUTION_MODE_COLUMN] != null)
			{
				ExecutionMode selected = (ExecutionMode)e.Row.Cells[EXECUTION_MODE_COLUMN].Value;

				if ((selected == ExecutionMode.RateBased) ||
					(affectedColumn == REPEAT_COUNT_COLUMN && selected == ExecutionMode.Iteration) ||
					(affectedColumn == DURATION_TIME_COLUMN && selected == ExecutionMode.Duration) ||
                    (selected == ExecutionMode.SetPaced))
				{
					EnableCellEdit(e, affectedColumn);
				}
				else
				{
					DisableCellEdit(e, affectedColumn);
				}
			}
		}

		private void FormatDelayCells(CellFormattingEventArgs e, string triggerColumn, string affectedColumn)
		{
			if (e.Row.Cells[triggerColumn] != null)
			{
				bool randomize = (bool)e.Row.Cells[triggerColumn].Value;
				if (randomize)
				{
					EnableCellEdit(e, affectedColumn);
				}
				else
				{
					DisableCellEdit(e, affectedColumn);
				}
			}
		}

		private void DisableCellEdit(CellFormattingEventArgs e, string affectedColumn)
		{
			e.CellElement.DrawFill = true;
			e.CellElement.BackColor = Color.LightGray;
			e.CellElement.NumberOfColors = 1;
			e.CellElement.ForeColor = Color.Gray;

			if (e.Row.Cells[affectedColumn] != null)
			{
				e.Row.Cells[affectedColumn].ReadOnly = true;
			}
		}

		private void EnableCellEdit(CellFormattingEventArgs e, string affectedColumn, bool resetReadOnly = true)
		{
			e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
			e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
			e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
			e.CellElement.ResetValue(LightVisualElement.NumberOfColorsProperty, ValueResetFlags.Local);

			if (resetReadOnly && e.Row.Cells[affectedColumn] != null)
			{
				e.Row.Cells[affectedColumn].ReadOnly = false;
			}
		}

		private void newGrid_CellEndEdit(object sender, GridViewCellEventArgs e)
		{
			var originalType = e.Column.GetType();
			object newValue = e.Value;

			GridViewEditManager manager = (GridViewEditManager)sender;
			RadGridView grid = manager.GridViewElement.GridControl;

			if (e.Column.HeaderText == "Max Workers Per VM" && newValue != null) // Special case for Office Workers, set value to default
			{
                int maxAllowedResources = _context.ResourceTypes.First(n => n.Name == "OfficeWorker").MaxResourcesPerHost;
                
				int num = (int)newValue;
                if (num > maxAllowedResources || num < 1)
				{
                    newValue = maxAllowedResources;
				}
			}
			else if (originalType == typeof(GridViewDecimalColumn) && (newValue == null || (int)newValue < 1)) // Prevents the user from inputting a negative value
			{
				newValue = 1;
			}

			foreach (var info in grid.SelectedCells)
			{
				if (info.ColumnInfo.Name == e.Column.Name)
				{
					info.Value = newValue;
				}

				if (info.RowInfo.GetType() != typeof(GridViewFilteringRowInfo)) // Do not allow filtering cells to be filled in
				{
					if (info.ColumnInfo.Name == e.Column.Name)
					{
						info.Value = newValue;
					}
				}
			}
		}

		private void newGrid_DataBindingComplete(object sender, GridViewBindingCompleteEventArgs e)
		{
			RadGridView grid = sender as RadGridView;

			if (grid.Columns.Count > 0)
			{
				HideColumns(grid.MasterTemplate);
				SetColumnsAsReadOnly(grid.MasterTemplate);
				CustomColumnSort(grid.MasterTemplate);

				if (grid.Name == "OfficeWorker") // Office workers will be used the most with this form, so these are special changes for them
				{
					GridViewDataColumn col = grid.Columns.Where(c => c.Name == "InstanceCount").FirstOrDefault();
					if (col != null)
					{
						col.HeaderText = "Total Workers";
					}

					col = grid.Columns.Where(c => c.Name == "RepeatCount").FirstOrDefault();
					if (col != null)
					{
						col.HeaderText = "Iterations";
					}

					GridViewDecimalColumn maxWorkersCol = grid.Columns.Where(c => c.Name == "ResourcesPerVM").FirstOrDefault() as GridViewDecimalColumn;
					if (maxWorkersCol != null)
					{
						maxWorkersCol.IsVisible = true;
						maxWorkersCol.HeaderText = "Max Workers Per VM";
						maxWorkersCol.DecimalPlaces = 0;

						grid.Rows.Select(r => r.Cells["ResourcesPerVM"]).ToList().ForEach(c =>
						{
							if (c.Value == null)
							{
                                c.Value = _context.ResourceTypes.First(n => n.Name == "OfficeWorker").MaxResourcesPerHost;
							}
						});
					}
				}

                UserInterfaceStyler.Configure(grid, GridViewStyle.Display);
				foreach (var c in grid.Columns)
				{
					c.AutoSizeMode = BestFitColumnMode.AllCells;
					c.BestFit();
					c.TextAlignment = ContentAlignment.MiddleCenter;
				}
			}           

			foreach (var template in grid.Templates) // Accounts for child tables
			{
				HideColumns(template);
				CustomColumnSort(template);
				SetColumnsAsReadOnly(template);
			}
		}

		private void newGrid_ToolTipTextNeeded(object sender, ToolTipTextNeededEventArgs e)
		{
			GridVirtualizedCellElement cell = sender as GridVirtualizedCellElement;

			if (cell != null)
			{
				if (cell.RowInfo.Cells[cell.ColumnInfo.Name].ReadOnly)
				{
					e.ToolTipText = "Cell cannot be edited";
				}
				else if (cell.ColumnInfo.Name.Contains("Delay"))
				{
					e.ToolTipText = "Column values are in seconds";
				}
				else if (cell.ColumnInfo.Name.Contains("Duration"))
				{
					e.ToolTipText = "Column values are in minutes";
				}
			}
		}

		private void newGrid_CommandCellClick(object sender, EventArgs e)
		{
			GridViewCellEventArgs args = e as GridViewCellEventArgs;

            using (XmlDisplayDialog dialog = new XmlDisplayDialog(XDocument.Parse(args.Row.Cells["Metadata"].Value.ToString())))
			{
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{

				}
			}
		}

		private void newGrid_Resize(object sender, EventArgs e)
		{
			RadGridView grid = sender as RadGridView;
			int additionalWidth = 40;

			foreach (var c in grid.Columns)
			{
				c.AutoSizeMode = BestFitColumnMode.AllCells;
				c.BestFit();
				c.TextAlignment = ContentAlignment.MiddleCenter;
			}
			
			foreach (var p in scenario_radPageView.Pages)
			{
				if (p.Width < grid.Width + additionalWidth)
				{
					p.Width = grid.Width + additionalWidth;
				}

				if (this.Width < p.Width + additionalWidth)
				{
					this.Width = p.Width + additionalWidth;
					grid.AutoSize = false;
					grid.Dock = DockStyle.Fill;
					grid.Resize -= newGrid_Resize;
				}
			}
		}

		private void HideColumns(GridViewTemplate template)
		{
			HiddenProperties().ForEach(c => 
			{
				if (template.Columns[c] != null)
				{
					template.Columns[c].IsVisible = false;
				}
			});
		}

		private List<string> HiddenProperties()
		{
			return new List<string>(){ 
				"EnterpriseScenario", "ResourcesPerVM", "FolderId", "ResourceType", "DurationString",
				"Runtime", "ExpandedDefinitions", "VirtualResourceId", 
				"EnterpriseScenarioId", "VirtualResourceMetadataSet",
				"ActivityExceptionSettings", "ExecutionSchedule", "SecurityGroups",
				"VirtualResource", "ResourceUsages", "ExecutionPlan",
				"VirtualResourceMetadataId", "Metadata", "Deleted",
				"DBRunMode", "ActivityDelay", "StartupDelay", "TestCaseId", "Description",
				"DBWorkerRunMode", "PublishedApp", "UserPool"
			};
		}

		private void SetColumnsAsReadOnly(GridViewTemplate template)
		{
			ReadOnlyProperties().ForEach(c =>
			{
				if (template.Columns[c] != null)
				{
					template.Columns[c].ReadOnly = true;
				}
			});
		}
		
		private List<string> ReadOnlyProperties() // Users should not edit these because it may cause errors
		{
			return new List<string>() { "Platform", "MetadataType" }; 
		}

		private void CustomColumnSort(GridViewTemplate template)
		{
			MoveColumn(template, "Name", 0);
			MoveColumn(template, "Enabled", 1);
			MoveColumn(template, "MetadataType", 1);
			MoveColumn(template, "Platform", 2);
			MoveColumn(template, "InstanceCount", 3);
			MoveColumn(template, "ResourcesPerVM", 4);
			MoveColumn(template, "RandomizeActivities", 5);
			MoveColumn(template, "ExecutionMode", 6);
			MoveColumn(template, "RepeatCount", 7);
			MoveColumn(template, "DurationTime", 8);
		}

		private void MoveColumn(GridViewTemplate template, string columnName, int newIndex)
		{
			if (template.Columns[columnName] != null)
			{
				int index = template.Columns[columnName].Index;
				template.Columns.Move(index, newIndex);
			}
		}

		/// <summary>
		/// Handles the Click event of the ok_Button control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ok_Button_Click(object sender, EventArgs e)
		{
			if(_bulkDeviceControl.BulkDeviceListChange)
			{
				UpdateVirtualResourceMetadataAssetUsage();
			}
			if (_bulkPrintQueueControl.BulkPrintQueueListChange)
			{
				UpdateVirtualResourceMetadataPrintQueueUsage();
			}

			this.DialogResult = DialogResult.OK;

			_context.SaveChanges();
		}

		/// <summary>
		/// Checks the list to see if any devices have been changed. If so calls to update the metadata.
		/// </summary>
		/// <param name="virtualResourceMetatdataId">The virtual resource metatdata identifier.</param>
		/// <param name="currentResourceId">The current resource identifier.</param>
		/// <param name="newResourceId">The new resource identifier.</param>
		private void UpdateVirtualResourceMetadataAssetUsage()
		{
			foreach(BulkDeviceEnt device in _bulkDeviceList)
			{
				if(device.DeviceChanged)
				{
					UpdateVirtualResourceMetadataAssetUsage(device.CurrentDevice, device.NewDevice);
				}
			}

		}

		private void UpdateVirtualResourceMetadataPrintQueueUsage()
		{
			foreach (BulkPrintQueueEnt queue in _bulkPrintQueueList)
			{
				if (queue.QueueChanged)
				{
					UpdateVirtualResourceMetadataPrintQueueUsage(queue.CurrentQueue, queue.NewQueue, queue.OldHostName, queue.NewHostName);
				}
			}
		}

		private void AddQueueToAll(string newQueue)
		{
            var newGuidId = ConfigurationServices.AssetInventory.GetRemotePrintQueues().Where(x => x.QueueName == newQueue).Select(x => x.PrintQueueId).FirstOrDefault();

            foreach (VirtualResourceMetadataPrintQueueUsage vrmpqu in _printQueueMetadataUsages)
            {
                XElement asd = XElement.Parse(vrmpqu.PrintQueueSelectionData);
                PrintQueueSelectionData printQueueSelectionData = Serializer.Deserialize<PrintQueueSelectionData>(asd);

                if (printQueueSelectionData.SelectedPrintQueues.Where(x => ((RemotePrintQueueDefinition)x).PrintQueueId == newGuidId).Count() == 0)
                {
                    printQueueSelectionData.SelectedPrintQueues.Add(new RemotePrintQueueDefinition(newGuidId));
                }
                XElement newAsd = Serializer.Serialize(printQueueSelectionData);
                vrmpqu.PrintQueueSelectionData = newAsd.ToString();
            }
            int result = _context.SaveChanges();

        }

        /// <summary>
        /// Updates the virtual resource printqueueusage metadata.
        /// </summary>
        /// <param name="oldQueue">Old Queue</param>
        /// <param name="newQueue">New Queue</param>
        private void UpdateVirtualResourceMetadataPrintQueueUsage(string oldQueue, string newQueue, string oldHostName, string newHostName)
		{
			if (oldQueue == "N/A")
			{
				AddQueueToAll(newQueue);
				return;
			}

			foreach(VirtualResourceMetadataPrintQueueUsage vrmpqu in _printQueueMetadataUsages)
			{
				XElement asd = XElement.Parse(vrmpqu.PrintQueueSelectionData);
				PrintQueueSelectionData printQueueSelectionData = Serializer.Deserialize<PrintQueueSelectionData>(asd);
				if (printQueueSelectionData.SelectedPrintQueues.Count == 0)
				{
					continue;
				}
				var queues = printQueueSelectionData.SelectedPrintQueues.Where(x => x.GetType() == typeof(RemotePrintQueueDefinition));

				List<Guid> remotequeues = new List<Guid>();
				foreach (var q in queues)
				{
					remotequeues.Add(((RemotePrintQueueDefinition)q).PrintQueueId);
				}


                var oldGuidId = ConfigurationServices.AssetInventory.GetRemotePrintQueues().Where(x => x.QueueName == oldQueue && x.ServerHostName == oldHostName).FirstOrDefault();
                var newGuidId = ConfigurationServices.AssetInventory.GetRemotePrintQueues().Where(x => x.QueueName == newQueue && x.ServerHostName == newHostName ).FirstOrDefault();

                if (remotequeues.Contains(oldGuidId.PrintQueueId))
                {
                    var theRest = remotequeues.Where(x => x != oldGuidId.PrintQueueId);
                    var changes = remotequeues.Where(x => x == oldGuidId.PrintQueueId).Select(x => x = newGuidId.PrintQueueId);

                    printQueueSelectionData.SelectedPrintQueues.Clear();

                    foreach (var queue in changes)
                    {
                        printQueueSelectionData.SelectedPrintQueues.Add(new RemotePrintQueueDefinition(queue));
                    }
                    foreach (var queue in theRest)
                    {
                        if (null == printQueueSelectionData.SelectedPrintQueues.Where(x => ((RemotePrintQueueDefinition)x).PrintQueueId == queue))
                        {
                            printQueueSelectionData.SelectedPrintQueues.Add(new RemotePrintQueueDefinition(queue));
                        }
                    }
                }

                XElement newAsd = Serializer.Serialize(printQueueSelectionData);
                vrmpqu.PrintQueueSelectionData = newAsd.ToString();


            }
			int result = _context.SaveChanges();
		}

		/// <summary>
		/// Updates the virtual resource metadata asset usage.
		/// </summary>
		/// <param name="oldAsset">The old asset.</param>
		/// <param name="newAsset">The new asset.</param>
		private void UpdateVirtualResourceMetadataAssetUsage(string oldAsset, string newAsset)
		{
			foreach (VirtualResourceMetadataAssetUsage vrmau in _assetMetadataUsages)
			{
				XElement asd = XElement.Parse(vrmau.AssetSelectionData);
				AssetSelectionData assetSelectionData = Serializer.Deserialize<AssetSelectionData>(asd);

				if(assetSelectionData.SelectedAssets.Contains(oldAsset))
				{
					var changes = assetSelectionData.SelectedAssets.Select(str => str.Replace(oldAsset, newAsset)).ToList();
					assetSelectionData.SelectedAssets.Clear();
					foreach (var asset in changes)
					{
						assetSelectionData.SelectedAssets.Add(asset);
					}

					XElement newAsd = Serializer.Serialize(assetSelectionData);
					vrmau.AssetSelectionData = newAsd.ToString();
				}
			}
			int result = _context.SaveChanges();
		}

		private void cancel_Button_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;

			this.Close();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_context.Dispose();
			}

			base.Dispose(disposing);
		}

	}
}
