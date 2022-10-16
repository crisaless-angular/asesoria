using System.Threading.Tasks;
using Web.Models;

namespace Web.Business.Interfaces
{
    public interface IMail
    {
        Task SendEmailAsync(JsonMail mailRequest);
    }
}