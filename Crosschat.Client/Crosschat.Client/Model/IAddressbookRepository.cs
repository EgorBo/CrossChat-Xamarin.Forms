using System.Threading.Tasks;

namespace Crosschat.Client.Model
{
    public interface IAddressbookRepository
    {
        Task<Contact[]> GetAllAsync();
    }
}
