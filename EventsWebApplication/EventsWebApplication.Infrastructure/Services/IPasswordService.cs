namespace EventsWebApplication.Infrastructure.Services
{
    public interface IPasswordService
    {
        public string MdHashGenerate(string input);
    }
}
