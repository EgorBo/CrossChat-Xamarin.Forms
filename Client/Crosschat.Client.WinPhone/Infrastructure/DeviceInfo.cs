using System.Threading.Tasks;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.WinPhone.Infrastructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceInfo))]

namespace Crosschat.Client.WinPhone.Infrastructure
{
    public class DeviceInfo : IDeviceInfo
    {
        public Task InitAsync()
        {
            return Task.FromResult(false);
        }

        public string Huid { get { return "TODO:HUID"; }}
        public string PushUri { get; private set; }
    }
}
