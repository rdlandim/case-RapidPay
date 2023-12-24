namespace Core.RapidPay.Options
{
    public class JwtOptions
    {
        public const string Key = "JwtSettings";

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int Lifetime { get; set; }
    }
}
