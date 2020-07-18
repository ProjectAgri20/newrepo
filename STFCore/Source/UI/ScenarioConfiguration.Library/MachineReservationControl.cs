using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Automation;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Control for reserving Virtual Machines for standalone use
    /// </summary>
    public partial class MachineReservationControl : ScenarioConfigurationControlBase
    {
        private VirtualResource _reservation = null;
        private MachineReservationMetadata _reservationData = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogCollectorControl"/> class.
        /// </summary>
        public MachineReservationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the configuration object from this control.
        /// </summary>
        public override EntityObject EntityObject
        {
            get { return _reservation; }
        }

        private static void AddMetadataIfMissing(VirtualResource reservation)
        {
            if (reservation.VirtualResourceMetadataSet.Count == 0)
            {
                var resourceType = VirtualResourceType.MachineReservation.ToString();
                var metadata = new VirtualResourceMetadata(resourceType, resourceType);
                metadata.Metadata = LegacySerializer.SerializeXml(new MachineReservationMetadata()).ToString();
                reservation.VirtualResourceMetadataSet.Add(metadata);
            }
        }

        /// <summary>
        /// Gets the resource title used in the base edit control.
        /// </summary>
        public override string EditFormTitle
        {
            get
            {
                return "Virtual Machine Reservation";
            }
        }

        /// <summary>
        /// Initializes this instance for configuration of a new object.
        /// </summary>
        public override void Initialize()
        {
            var reservation = new VirtualResource(VirtualResourceType.MachineReservation.ToString());
            AddMetadataIfMissing(reservation);
            Initialize(reservation);
        }

        /// <summary>
        /// Initializes this instance with the specified object.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ControlTypeMismatchException">
        /// Thrown when an object of incorrect type is passed to this instance.
        ///   </exception>
        public override void Initialize(object entity)
        {
            _reservation = entity as VirtualResource;
            AddMetadataIfMissing(_reservation);

            if (_reservation == null)
            {
                throw new ControlTypeMismatchException(entity, typeof(VirtualResource));
            }

            platform_ComboBox.SetPlatform(_reservation.Platform, VirtualResourceType.MachineReservation);

            var metadata = _reservation.VirtualResourceMetadataSet.FirstOrDefault();
            if (metadata != null)
            {
                _reservationData = LegacySerializer.DeserializeXml<MachineReservationMetadata>(metadata.Metadata);
            }
            else
            {
                _reservationData = new MachineReservationMetadata();
            }

            var package = SoftwareInstallerPackage.CreateSoftwareInstallerPackage(Guid.Empty);
            package.Description = "None";

            package_ComboBox.Items.Add(package);
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                List<SoftwareInstallerPackage> list = new List<SoftwareInstallerPackage>();
                list.Add(package);

                list.AddRange(context.SoftwareInstallerPackages.OrderBy(n => n.Description));
                package_ComboBox.DataSource = list;
                package_ComboBox.DisplayMember = "Description";
                package_ComboBox.ValueMember = "PackageId";

                package_ComboBox.SelectedItem = list.FirstOrDefault(e => e.PackageId == _reservationData.PackageId);
            }

            // Set up data bindings
            name_TextBox.DataBindings.Add("Text", _reservation, "Name");
            description_TextBox.DataBindings.Add("Text", _reservation, "Description");
            platform_ComboBox.DataBindings.Add("SelectedValue", _reservation, "Platform");
            instanceCount_NumericUpDown.DataBindings.Add("Text", _reservation, "InstanceCount");
            package_ComboBox.DataBindings.Add("SelectedValue", _reservationData, "PackageId");
        }

        /// <summary>
        /// Requests that the control finalize any edits that have been made by saving
        /// them from the UI controls to their backing objects.
        /// </summary>
        public override void FinalizeEdit()
        {
            // Change the focused control so that data bindings will update
            name_Label.Focus();

            var metadata = _reservation.VirtualResourceMetadataSet.First();
            if (_reservationData.PackageId != LegacySerializer.DeserializeXml<MachineReservationMetadata>(metadata.Metadata).PackageId)
            {
                metadata.Metadata = LegacySerializer.SerializeXml(_reservationData).ToString();
            }
        }
    }
}
