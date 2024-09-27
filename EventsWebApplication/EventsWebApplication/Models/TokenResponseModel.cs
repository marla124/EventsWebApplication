namespace EventsWebApplication.Models
{
    public class TokenResponseModel
    {
        public string JwtToken { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
