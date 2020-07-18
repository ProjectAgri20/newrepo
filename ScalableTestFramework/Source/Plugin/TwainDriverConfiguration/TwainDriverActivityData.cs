using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.TwainDriverConfiguration
{
    /// <summary>
    /// Control to configure data for TwainDriverConfiguration activity.
    /// </summary>

    [DataContract]
    public class TwainDriverActivityData
    {
        /// <summary>
        /// Gets or sets a value indicating whether launch the quickset from App or Homescreen.
        /// </summary>
        [DataMember]
        public string SetupFileName { get; set; }

        /// <summary>
        /// Gets or sets a value for Twin Configuration.
        /// </summary>
        [DataMember]
        public TwainConfiguration TwainConfigurations { get; set; }

        /// <summary>
        /// Gets or sets a value for Twin Configuration.
        /// </summary>
        [DataMember]
        public TwainOperation TwainOperation { get; set; }

        /// <summary>
        /// List of selected PageSides to execute task on
        /// </summary>
        [DataMember]
        public PageSides PageSides { get; set; }

        /// <summary>
        /// List of selected ItemType to execute task on
        /// </summary>
        [DataMember]
        public ItemType ItemType { get; set; }

        /// <summary>
        /// List of selected PageSize to execute task on
        /// </summary>
        [DataMember]
        public PageSize PageSize { get; set; }

        /// <summary>
        /// List of selected Source to execute task on
        /// </summary>
        [DataMember]
        public Source Source { get; set; }

        /// <summary>
        /// List of selected ColorMode to execute task on
        /// </summary>
        [DataMember]
        public ColorMode ColorMode { get; set; }

        /// <summary>
        /// List of selected FileType to execute task on
        /// </summary>
        [DataMember]
        public FileType FileType { get; set; }

        /// <summary>
        /// List of selected SendTo to execute task on
        /// </summary>
        [DataMember]
        public SendTo SendTo { get; set; }

        /// <summary>
        /// Gets or sets a ShortcutSettings.
        /// </summary>
        [DataMember]
        public ShortcutSettings ShortcutSettings { get; set; }

        /// <summary>
        /// Gets or sets the lock timeout.
        /// </summary>
        /// <value>The lock timeout.</value>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets a SharedFolderPart.
        /// </summary>
        [DataMember]
        public string SharedFolderPath { get; set; }

        /// <summary>
        /// Gets or sets a SharedFolderPart.
        /// </summary>
        [DataMember]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Reservation should be enabled or Not.
        /// </summary>
        /// <value><c>true</c> if a Reservation should be used; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool IsReservation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [IsPinRequired] is to be used.
        /// </summary>
        ///   <c>true</c> if [Pin Required]; otherwise, <c>false</c>.
        [DataMember]
        public bool IsPinRequired { get; set; }

        /// <summary>
        /// Gets or sets the PIN
        /// </summary>
        /// <value>Pin to unlock the file.</value>
        [DataMember]
        public string Pin { get; set; }

        public TwainDriverActivityData()
        {
            SetupFileName = string.Empty;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            ItemType = ItemType.Document;
            PageSides = PageSides.OneSided;
            PageSize = PageSize.DetectContentOnPage;
            Source = Source.Flatbed;
            ColorMode = ColorMode.Color;
            FileType = FileType.Pdf;
            SendTo = SendTo.LocalorNetworkfolder;
            ShortcutSettings = ShortcutSettings.SavesAsPdf;
            SharedFolderPath = string.Empty;
            EmailAddress = string.Empty;
            Pin = string.Empty;
            IsReservation = false;
            IsPinRequired = false;
            TwainConfigurations = TwainConfiguration.SavesAsPdf;
            TwainOperation = TwainOperation.Install;
        }
    }
}
