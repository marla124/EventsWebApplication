using EventsWebApplication.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace EventsWebApplication.Infrastructure.Services
{
    public class PasswordService(IConfiguration configuration) : IPasswordService
    {
        public string MdHashGenerate(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                var salt = configuration["AppSettings:PasswordSalt"];
                byte[] inputBytes = Encoding.UTF8.GetBytes($"{input}{salt}");
                byte[] HashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(HashBytes);
            }
        }
    }
}
