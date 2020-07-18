
namespace HP.ScalableTest.Service.DataLog
{
    class Program
    {
        static void Main(string[] args)
        {
            using (DataLogWindowsService service = new DataLogWindowsService())
            {
                service.Run(args);
            }
        }
    }
}
