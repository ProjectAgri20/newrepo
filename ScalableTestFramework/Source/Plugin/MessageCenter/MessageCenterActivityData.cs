using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace HP.ScalableTest.Plugin.MessageCenter
{
    /// <summary>
    /// Class containing the metadata items to be serialized for the plug-in.
    /// </summary>
    /// <remarks>
    /// The STF framework will use <see cref="NetDataContractSerializer"/>, which stores CLR type
    /// information in the serialized XML. This has the advantage of handling polymorphic types,
    /// but requires it to be more stringent regarding type names, namespaces, and assemblies of
    /// the types being serialized. Changes to the plug-in assembly name, plug-in namespace, or
    /// metadata class name will break existing activity data. Some changes to the members of the
    /// plug-in class may also break existing activity data. Ensure the assembly name, namespace,
    /// and plug-in metadata class are as desired before using the plug-in.
    /// 
    /// •  The metadata class must be decorated with a <c>DataContract</c> attribute.
    /// •  Any data that should be persisted during serialization must be decorated with a
    ///    <c>DataMember</c> attribute. This attribute can be applied to fields or properties; any
    ///    properties with this attribute must have a getter and a setter, though these do not have
    ///    to be public.
    /// •  The new metadata class should not include information about selected assets or
    ///    documents. This data is stored elsewhere and will be automatically persisted by the
    ///    framework.
    /// •  The new metadata class should minimize references to types outside of the plug-in,
    ///    other than .NET native types.
    /// 
    /// <seealso cref="System.Runtime.Serialization.NetDataContractSerializer"/>
    /// </remarks>
    [DataContract]
    public class MessageCenterActivityData
    {
        [DataMember]
        public MessageType Message { get; set; }

        [DataMember]
        public bool Presence { get; set; }

        [DataMember]
        public bool UseEws { get; set; }

        public MessageCenterActivityData()
        {
            UseEws = false;
        }
    }


    /// <summary>
    /// Enums holding the messages shown on message center
    /// </summary>

    public enum MessageType
    {
        [Description("Tray .* open")]
        TrayOpen,
        [Description("Tray .* empty")]
        TrayEmpty,
        [Description("Cartridge Low")]
        CartridgeLow,
        [Description("Cartridge might be empty")]
        EmptyCartridge,
        [Description("Install .* Cartridge")]
        CartridgeMissing,
        [Description("Close Front Door, Close top cover")]
        CartridgeDoorOpen,
        [Description("Close .* Door")]
        JamDoorOpen,
        [Description("Close Door")]
        CloseDoor,
        [Description("Flatbed cover open")]
        FlatBedOpen


    }

    /// <summary>
    /// Class to hold deserialized data of message center
    /// </summary>

    internal class MessageCenterMessages
    {
        [JsonProperty("messages")]
        internal List<MessageCenterNotification> Notifications { get; set; }

        internal MessageCenterMessages()
        {
            Notifications = new List<MessageCenterNotification>();
        }
    }

    /// <summary>
    /// class representing message center notification from EWS
    /// </summary>
    internal class MessageCenterNotification
    {
        //{"messages":[{"type":"notification","msg":"Sleep mode on","priority":"299"}]}

        [JsonProperty("type")]
        internal string Type { get; set; }

        [JsonProperty("msg")]
        internal string Message { get; set; }

        [JsonProperty("priority")]
        internal int Priority { get; set; }
    }

}
