namespace TestApp.Core.Configuration
{
    public class KunaOptions
    {
        public static string Section => "Kuna";
        public string Key { get; set; }
        public string SecretKey { get; set; }
        public string PrivateKey { get; set; }
    }

}
