using System.Threading.Tasks;
using Authentication_Identity.API.Model;

namespace Authentication_Identity.API.Service
{
    public interface IEmailService
    {
        Task SendTestemail(UserEmailOptions userEmailOptions);
    }
}