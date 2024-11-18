namespace EventsWebApplication.Domain.Interfaces
{
    public interface IPasswordService
    {
        public string MdHashGenerate(string input);
    }
}
