namespace HP.ScalableTest.Service.Lock
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (LockWindowsService service = new LockWindowsService())
            {
                service.Run(args);
            }
        }
    }
}
