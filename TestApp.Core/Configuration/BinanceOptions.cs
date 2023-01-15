namespace TestApp.Core.Configuration
{
    public class BinanceOptions
    {
        public static string Section = "Binance";

        public string Name { get; set; }
        public string Key { get; set; }
        public string SecretKey { get; set; }
    }

}
