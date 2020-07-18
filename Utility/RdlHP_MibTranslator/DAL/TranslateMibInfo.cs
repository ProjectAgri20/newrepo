using System;
using System.IO;
using System.Linq;

namespace HP.RDL.RdlHPMibTranslator
{
	class TranslateMibInfo
	{
		private string MibFile { get; set; }

		public string GetLastError { get; set; }

        private string[] _titles = { "iso", "org", "dod", "internet", "private", "enterprises", "hp", "nm" };
        private int[] _titleValues = { 1, 3, 6, 1, 4, 1, 11, 2 };

		public TranslateMibInfo(string filePath)
		{
			MibFile = filePath;
			GetLastError = string.Empty;
		}

		public void CreateMibListing(ListMibEnt mibList)
		{
			FileStream fs = null;

			try
			{
				fs = new FileStream(MibFile, FileMode.Open, FileAccess.Read);
				StreamReader sr = new StreamReader(fs);

                GenerateBase(mibList);

				while (sr.Peek() != -1 )
				{
					string line = sr.ReadLine();

					if (line.ToLower().Contains("object identifier") && line.Contains("::="))
					{
						if (IsGenericMib(line))
						{
							MibEnt mib = new MibEnt();
							mib = ParseMibIdentifier(line);
                            if (!FoundMibEnt(mibList, mib))
                            {
                                mibList.Add(mib);
                            }
						}
						else
						{
                            ParseObjectIdent(mibList, line);
						}
					}

				}
			}
			catch (FileNotFoundException fe) { GetLastError = fe.JoinAllErrorMessages(); }
			catch (DirectoryNotFoundException fe) { GetLastError = fe.JoinAllErrorMessages(); }
			catch (IOException e) { GetLastError = e.JoinAllErrorMessages(); }
			finally
			{
				if (fs != null) { fs.Close(); }
			}
		}

        private void GenerateBase(ListMibEnt mibList)
        {
            for(int idx = 0; idx < _titles.Count(); idx++)
            {
                MibEnt ent = new MibEnt();
                
                ent.Title = _titles[idx];
                ent.Value = _titleValues[idx];

                if(idx > 0)
                {
                    ent.Parent = _titles[idx - 1];
                }

                mibList.Add(ent);
            }
        }

        public bool FoundMibEnt(ListMibEnt mibList, MibEnt mib)
        {
            bool bFound = false;
            foreach(MibEnt ent in mibList)
            {
                if(ent.Equals(mib))
                {
                    bFound = true;
                    break;
                }
            }
            return bFound;
        }
  
		private void ParseObjectIdent(ListMibEnt mibListing, string line)
		{
			string[] separators = { " ", "\t", "{", "}" };
			string[] data = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			
			string parent = GetStartingParent(data);
			int start = FindStartOfObjIdent(data);
			GetObjIdentHeader(mibListing, data);

			for (int idx = start; idx < data.Length - 1; idx++)
			{
				MibEnt mib = new MibEnt();
				mib.Title = GetMibStringValue(data[idx]);
				mib.Value = GetMibValue(data[idx]);
				mib.Parent = parent;

				parent = mib.Title;

				mibListing.Add(mib);
			}
		}
  
		private string GetStartingParent(string[] data)
		{
			string parent = string.Empty;
			int idx = FindStartOfObjIdent(data) - 1;

			if (!data[idx].Equals("::="))
			{
				parent = data[idx];
			}
			return parent;
		}

		private int FindStartOfObjIdent(string[] data)
		{
			int start = 0;

			foreach (string str in data)
			{
				if (str.Contains("("))
				{
					break;
				}
				start++;
			}

			return start;
		}

		private void GetObjIdentHeader(ListMibEnt mibListing, string[] data)
		{
			MibEnt mib = new MibEnt();
			int last = data.Length - 2;

			mib.Title = data[0].Trim();
			mib.Value = GetMibValue(data);
			mib.Parent = GetMibStringValue(data[last]);

			mibListing.Add(mib);
		}

		private bool IsGenericMib(string line)
		{
			bool isGeneric = true;
			string[] separators = { " ", "\t", "{", "}" };
			string[] data = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

			if (data.Length > 9)
			{
				isGeneric = false;
			}

			return isGeneric;
		}
  
		private MibEnt ParseMibIdentifier(string line)
		{
			MibEnt mib = new MibEnt();

			string[] separators = { " ", "\t", "{", "}" };
			string[] data = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

			mib.Title = GetMibTitle(data);
			mib.Parent = GetMibParent(data);
			mib.Value = GetMibValue(data);

			return mib;
		}
  
		private string GetMibParent(string[] data)
		{
			string previous = string.Empty;
			int idx = 0;
			int elemments = data.Count() - 1;

			while (!data[idx].Equals("::="))
			{
				idx++;
			}
			// move past the ::= cell
			idx++;

			// the previous title has no spaces
			if (idx == (elemments - 1))
			{
				previous = data[idx];
			}
			else
			{
				// the previous title has spaces. The last value is the current MIB value
				for (int pos = idx; pos < elemments; pos++)
				{
					previous += data[pos];
				}
			}

			return previous;
		}
		private string GetMibStringValue(string value)
		{
			string parent = string.Empty;

			int idx = value.IndexOf("(");
			parent = value.Substring(0, idx);
			return parent;
		}
		private int GetMibValue(string line)
		{
			string[] separators = { " ", "\t", "(", ")" };
			string[] data = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);

			return GetMibValue(data);
		}
		private string GetMibTitle(string[] data)
		{
			string title = string.Empty;
			int idx = 0;
			while (!data[idx].ToLower().Equals("object"))
			{
				title += data[idx];
				idx++;
			}

			return title;
		}

		private int GetMibValue(string[] data)
		{
			int value = 0;
			int idx = data.Count() - 1;

			int.TryParse(data[idx], out value);

			return value;
		}
	}
}
