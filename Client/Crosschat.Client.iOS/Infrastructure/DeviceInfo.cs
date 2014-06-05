using System.Threading.Tasks;
using Crosschat.Client.iOS.Infrastructure;
using Crosschat.Client.Model.Contracts;
using Xamarin.Forms;

[assembly: Dependency(implementorType: typeof(DeviceInfo))]

namespace Crosschat.Client.iOS.Infrastructure
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
