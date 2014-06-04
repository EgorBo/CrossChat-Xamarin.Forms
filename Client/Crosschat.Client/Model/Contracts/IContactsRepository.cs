using System.Threading.Tasks;
using Crosschat.Client.Model.Entities;

namespace Crosschat.Client.Model.Contracts
{
    public interface IContactsRepository
    {
        Task<Contact[]> GetAllAsync();
    }
}
