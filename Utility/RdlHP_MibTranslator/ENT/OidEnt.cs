using System.Collections.Generic;

namespace HP.RDL.RdlHPMibTranslator
{
	public class OidEnt
	{
        /// <summary>
        /// date-and-time
        /// </summary>
		public string OidName { get; set; }
        /// <summary>
        /// 1.3.6.1.4.1.11.2.3.9.4.2.1.1.2.17
        /// </summary>
		public string OidString { get; set; }
        /// <summary>
        /// OCTET STRING
        /// </summary>
		public List<string> Syntax { get; set; }
        /// <summary>
        /// read-write
        /// </summary>
		public string Access { get; set; }
        /// <summary>
        /// optional
        /// </summary>
		public string Status { get; set; }
        /// <summary>
        /// "A C structure containing the following fields: ...
        /// </summary>
		public string Description { get; set; }
        /// <summary>
        /// status-system
        /// </summary>
		public string Parent { get; set; }
        /// <summary>
        /// 17
        /// </summary>
		public string Value { get; set; }
		public string OidIndexValue { get; set; }
		public string OidIndex { get; set; }
		public USAGES Usage { get; set; }

		public OidEnt()
		{
			OidName = string.Empty;
			OidString = string.Empty;
			Access = string.Empty;
			Syntax = new List<string>();
			Status = string.Empty;
			Description = string.Empty;
			Parent = string.Empty;
			Value = string.Empty;
			Usage = USAGES.eUnknown;

			OidIndex = string.Empty;
			OidIndexValue = string.Empty;
		}
		public OidEnt(OidEnt copy)
		{
			this.OidName = copy.OidName;
			this.OidString = copy.OidString;
			this.Access = copy.Access;
			this.Status = copy.Status;
			this.Description = copy.Description;
			this.Parent = copy.Parent;
			this.Value = copy.Value;
			this.OidIndex = copy.OidIndex;
			this.OidIndexValue = copy.OidIndexValue;
			this.Usage = copy.Usage;

			this.Syntax = new List<string>();

			foreach (string str in copy.Syntax)
			{
				this.Syntax.Add(str);
			}
		}
        public override int GetHashCode()
        {
            string myCompare = this.OidName + this.OidString;
            return myCompare.ToLower().GetHashCode();
            //return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var temp = obj as OidEnt;

            return (obj != null && this.OidName.Equals(temp.OidName) && this.OidString.Equals(temp.OidString));
            //return base.Equals(obj);
        }
		public string SyntaxValue
		{
			get
			{
				string value = string.Empty;

				if (Syntax.Count > 0)
				{
					value = Syntax[0];
				}

				return value;
			}
		}
		public string Enumerations
		{
			get
			{
				string value = string.Empty;

				if (Syntax.Count > 1)
				{
					for (int idx = 1; idx < Syntax.Count; idx++)
					{
						if (!string.IsNullOrEmpty(value))
						{
							value += "::" + Syntax[idx];
						}
						else
						{
							value = Syntax[idx];
						}
					}
				}

				return value;
			}
		}
	}
	public enum USAGES
	{
		[EnumValue("Unknown")]
		eUnknown = 0,
		[EnumValue("All")]
		eAll = 1,
		[EnumValue("HP Device")]
		eHPDevice = 2,
		[EnumValue("HP Device & MIB")]
		eHpDeviceMib = 3,
		[EnumValue("Managed Contracts")]
		eMc = 4,
		[EnumValue("MC & MIB")]
		eMcMib = 5,
		[EnumValue("MC & HP Device")]
		eMcHpDevice = 6,
		[EnumValue("MIB")]
		eMib = 7
	}
}
