using System.IO;
using System.Threading.Tasks;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.WinPhone.Infrastructure;
using Microsoft.Phone.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPicker))]

namespace Crosschat.Client.WinPhone.Infrastructure
{
    public class PhotoPicker : IPhotoPicker
    {
        public Task<byte[]> PickPhoto()
        {
            var taskCompletionSource = new TaskCompletionSource<byte[]>();
            var task = new PhotoChooserTask();
            task.ShowCamera = false;
            task.PixelWidth = 300;
            task.PixelHeight = 300;
            task.Completed += (s, e) =>
                {
                    if (e.TaskResult == TaskResult.OK && e.ChosenPhoto != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            e.ChosenPhoto.CopyTo(ms);
                            ms.Position = 0;
                            taskCompletionSource.TrySetResult(ms.ToArray());
                        }
                    }
                    else
                    {
                        taskCompletionSource.TrySetResult(null);
                    }
                };
            task.Show();

            return taskCompletionSource.Task;
        }
    }
}
