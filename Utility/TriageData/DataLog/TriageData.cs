using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace HP.ScalableTestTriageData.Data.DataLog
{
    [Table("TriageData")]
    public partial class TriageData
    {
        public Guid TriageDataId { get; set; }

        [Required]
        [StringLength(50)]
        public string SessionId { get; set; }

        public Guid ActivityExecutionId { get; set; }

        [Required]
        public string ControlIds { get; set; }

        public byte[] ControlPanelImage { get; set; }

        public string DeviceWarnings { get; set; }

        public string UIDumpData { get; set; }

        public byte[] Thumbnail { get; set; }

        public string Reason { get; set; }

        public DateTime TriageDateTime { get; set; }

        public DateTime LocalTriageDateTime
        {
            get
            {
                return new DateTime(TriageDateTime.Ticks, DateTimeKind.Utc).ToLocalTime();
            }
        }

        public static List<string> SessionIds(DataLogContext dlc, DateTime dtStart, DateTime dtEnd) => (from td in dlc.TriageDatas where td.TriageDateTime >= dtStart && td.TriageDateTime <= dtEnd select td.SessionId).Distinct().ToList();

        public static IEnumerable<TriageData> GetTriageDataBySessionId(DataLogContext dlc, string sessionId) => (from td in dlc.TriageDatas where td.SessionId.Equals(sessionId) select td).Take(3000).ToList();
        public static int GetCountBySessionId(DataLogContext dlc, string sessionId) => (from td in dlc.TriageDatas where td.SessionId.Equals(sessionId) select td).Count();

        public static TriageData GetTriageDataById(DataLogContext dl, Guid triageDataId) => (from td in dl.TriageDatas where td.TriageDataId.Equals(triageDataId) select td).FirstOrDefault();


    }
}
