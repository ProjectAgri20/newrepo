using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains details and availability information for a card reader used in an enterprise test.
    /// </summary>
    [DataContract]
    [AssetHost("CardReaderHost")]
    public class CardReaderDetail : AssetDetail
    {
        /// <summary>
        /// Gets or sets the address at which the card swipe controller can be contacted.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the port at which the card swipe controller can be contacted.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        [DataMember]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the controller serial number.
        /// </summary>
        /// <value>
        /// The controller serial number.
        /// </value>
        [DataMember]
        public int ControllerSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the controller index of the appropriate servo.
        /// </summary>
        /// <value>
        /// The index of the servo.
        /// </value>
        [DataMember]
        public int ServoIndex { get; set; }

        /// <summary>
        /// Gets or sets the card reader position (Left, Right, etc.)
        /// </summary>
        /// <value>
        /// The card reader position.
        /// </value>
        [DataMember]
        public string CardReaderPosition { get; set; }

        /// <summary>
        /// Gets or sets the card id.
        /// </summary>
        /// <value>
        /// The card id.
        /// </value>
        [DataMember]
        public string CardId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardReaderDetail"/> class.
        /// </summary>
        public CardReaderDetail()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardReaderDetail"/> class.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="availabilityEndTime">The availability end time.</param>
        /// <param name="description">The description.</param>
        public CardReaderDetail(string deviceId, DateTime availabilityStartTime, DateTime availabilityEndTime)
            : base(deviceId, availabilityStartTime, availabilityEndTime)
        {
            Address = string.Empty;
            Port = 0;
            ControllerSerialNumber = -1;
            ServoIndex = -1;
            CardReaderPosition = string.Empty;
            CardId = string.Empty;
        }
    }
}