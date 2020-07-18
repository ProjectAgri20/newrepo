using System;
using System.Runtime.Serialization;

namespace HP.RDL.EDT.Common
{
	[DataContract]
	public class DDSResult
	{
		[DataMember]
		public string ErrorMessage { get; set; }
		
		public bool IsError => !string.IsNullOrEmpty(ErrorMessage);

	    [DataMember]
		public Guid EventId { get; set; }
		
		public String EventIdToString { get { return EventId.ToString(); } }

		[DataMember]
		public String Value { get; set; }

		public DDSResult()
		{
			ErrorMessage = String.Empty;
			EventId = Guid.Empty;
			Value = String.Empty;
		}
	}
}
