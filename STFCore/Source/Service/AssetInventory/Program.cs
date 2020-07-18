
namespace HP.ScalableTest.Service.AssetInventory
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (AssetInventoryWindowsService service = new AssetInventoryWindowsService())
            {
                service.Run(args);
            }
        }
    }
}
