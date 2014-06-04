using System.Threading.Tasks;

namespace Crosschat.Client.Model.Contracts
{
    public interface IPhotoPicker
    {
        Task<byte[]> PickPhoto();
    }
}
