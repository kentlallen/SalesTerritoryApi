namespace SalesTerritoryApi.Configuration
{
    public static class Ports
    {
        public const int ApiPort = 7004;
        public const int ReactPort = 5173;
        
        public static string ApiUrl => $"https://localhost:{ApiPort}";
        public static string ReactUrl => $"http://localhost:{ReactPort}";
    }
}
