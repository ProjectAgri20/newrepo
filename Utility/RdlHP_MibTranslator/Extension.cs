using System;

namespace HP.RDL.RdlHPMibTranslator
{
	public static class Extension
	{
		/// <summary>
		/// Get the <see cref="Exception.Message"/>s of the supplied <paramref name="error"/> and all <see cref="Exception.InnerException"/>s.
		/// </summary>
		/// <param name="error">The <see cref="Exception"/>, or <c>null</c>.</param>
		/// <returns>[<see cref="Exception.Message"/>[\r\nInnerexception: <see cref="Exception.Message"/>[...]]]</returns>
		public static string JoinAllErrorMessages(this Exception error)
		{
			if (error != null)
			{
				if (error.InnerException != null)
				{
					return error.Message + Environment.NewLine + "InnerException: " + error.InnerException.JoinAllErrorMessages();
				}
				return error.Message;
			}
			return string.Empty;
		}
	}
}
