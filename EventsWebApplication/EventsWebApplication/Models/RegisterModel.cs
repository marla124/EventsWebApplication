namespace EventsWebApplication.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
