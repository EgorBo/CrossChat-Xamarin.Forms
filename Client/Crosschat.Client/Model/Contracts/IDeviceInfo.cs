using System.Threading.Tasks;

namespace Crosschat.Client.Model.Contracts
{
    public interface IDeviceInfo
    {
        Task InitAsync();

        string Huid { get; }

        string PushUri { get; }
    }
}