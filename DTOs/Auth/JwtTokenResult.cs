namespace BB.APIs.BeardBoss.Monolithic.DTOs.Auth
{
    public class JwtTokenResult
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
