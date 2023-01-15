namespace TestApp.Core.Configuration
{
    public class KucoinOptions
    {
        public static string Section = "Kucoin";

        public string Key { get; set; }
        public string Secret { get; set; }
        public string SecretPhrase { get; set; }
    }

}
