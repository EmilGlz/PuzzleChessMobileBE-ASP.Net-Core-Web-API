namespace ChessMobileBE
{ 
    public static class MyConfigurationManager
    {
        public static IConfiguration AppSetting { get; }
        static MyConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
    }
}
