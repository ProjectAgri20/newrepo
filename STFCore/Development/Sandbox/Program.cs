using System;
using System.Net.Mail;
using System.Threading.Tasks;
using HP.ScalableTest.Core;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Development
{
#pragma warning disable 0162

    internal class Program
    {
        static Program()
        {
            HP.ScalableTest.Framework.Logger.Initialize(new SystemTraceLogger(typeof(HP.ScalableTest.Framework.Logger)));
            TraceFactory.Logger.Initialize();
        }

        /// <summary>
        /// Note: Because of the public nature of this class, if you will be checking in changes to the sandbox project,
        /// please put your code in a separate method that can be called from Main().  By following this process there
        /// is a better chance that your test code will be preserved between check ins by other developers.  Thanks.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            KellyTest test = new KellyTest();
            try
            {
                test.Go();
                //test.PrintNewId();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
            }

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static string GetInput(string promptText)
        {
            string response = string.Empty;

            while (string.IsNullOrEmpty(response))
            {
                Console.WriteLine(promptText);
                response = Console.ReadLine();
            }

            return response;
        }

    }

#pragma warning restore
}