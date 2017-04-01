using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Postal;

namespace BeardBook.Identity
{
    public class EmailService : IIdentityMessageService
    {
        private const string DefaultEmail = "Default";
        public Task SendAsync(IdentityMessage message)
        {
            return SendAsync(DefaultEmail, message);
        }

        public Task SendAsync(string emailName, IdentityMessage message)
        {
            dynamic email = new Email(emailName);
            email.To = message.Destination;
            email.Subject = message.Subject;
            email.Body = message.Body;
            email.Send();
            return Task.FromResult(0);
        }
    }
}