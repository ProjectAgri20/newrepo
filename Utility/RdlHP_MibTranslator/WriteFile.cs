using System.IO;

namespace HP.RDL.RdlHPMibTranslator
{
	class WriteFile
	{
		public ListOidEnt ListOids;
		public string GetLastError { get; private set; }
		public string SaveDirectory { get; set; }
		public string FileName { get; set; }

		public WriteFile(ListOidEnt listOids, string startDir="")
		{
			ListOids = listOids;
			SaveDirectory = startDir;
		}
		public bool IsError
		{
			get
			{
				return (string.IsNullOrEmpty(GetLastError) == false) ? true : false;
			}
		}
		public void WritePartOInfoFile()
		{
			if (string.IsNullOrEmpty(SaveDirectory))
			{
				SaveDirectory = Directory.GetCurrentDirectory();
			}
			
			string pathName = SaveDirectory +  @"\notFound.csv";
			FileStream fs = null;
			string header = "MCO NAME,OID";

			try
			{
				fs = new FileStream(pathName, FileMode.Create, FileAccess.Write);
				using (StreamWriter sw = new StreamWriter(fs))
				{
					fs = null;
					sw.WriteLine(header);
					foreach (OidEnt oid in ListOids)
					{
						string line = oid.OidName + "," + oid.OidString;
						sw.WriteLine(line);
					}
				}
			}
			catch (FileNotFoundException fe) { GetLastError = fe.JoinAllErrorMessages(); }
			catch (DirectoryNotFoundException fe) { GetLastError = fe.JoinAllErrorMessages(); }
			catch (IOException e) { GetLastError = e.JoinAllErrorMessages(); }
			finally
			{
				if (fs != null) { fs.Dispose(); }
			}
		}
		public void WriteOidGridData()
		{
			if (string.IsNullOrEmpty(SaveDirectory))
			{
				SaveDirectory = Directory.GetCurrentDirectory();
			}
			if (string.IsNullOrEmpty(FileName))
			{
				FileName = "FullOidListing.csv";
			}

			string pathName = @SaveDirectory + @"\" + FileName;
			FileStream fs = null;
			string header = "OID Name,MIB OID,SYNTAX,ENUMERATIONS,HP Device OID,HP Device Value,ACCESS,STATUS,USAGE,Description";
			
			try
			{
				fs = new FileStream(pathName, FileMode.Create, FileAccess.Write);
				using (StreamWriter sw = new StreamWriter(fs))
				{
					fs = null;
					sw.WriteLine(header);
					foreach (OidEnt oid in ListOids)
					{
						string val = Enum<USAGES>.Value(oid.Usage);
						string line = oid.OidName + "," + oid.OidString + "," + oid.SyntaxValue + "," + oid.Enumerations + "," + oid.OidIndex + "," + oid.OidIndexValue + "," + oid.Access + "," + oid.Status + "," + val + "," + oid.Description;
						sw.WriteLine(line);
					}
				}
			}
			catch (FileNotFoundException fe) { GetLastError = fe.JoinAllErrorMessages(); }
			catch (DirectoryNotFoundException fe) { GetLastError = fe.JoinAllErrorMessages(); }
			catch (IOException e) { GetLastError = e.JoinAllErrorMessages(); }
			finally
			{
				if (fs != null) { fs.Dispose(); }
			}
		}
	}
}
