using System;
using System.IO;
using System.Linq;
using Crosschat.Server.Application.Contracts;
using Crosschat.Utils.Logging;
using ImageResizer;

namespace Crosschat.Server.Infrastructure.FileSystem
{
    public class FileStorage : IFileStorage
    {
        private static int _lastPhoto = 1;
        private static readonly object SyncObj = new object();
        private readonly ISettings _settings;
        private readonly ResizeSettings _resizeSettings;
        private readonly ILogger _logger = LogFactory.GetLogger<FileStorage>();

        private const string FullsizedImagesFolder = "l";
        private const string SmallImagesFolder = "s";
        private const string ImageQuality = "75";

        public FileStorage(ISettings settings)
        {
            _settings = settings;
            
            _resizeSettings = new ResizeSettings {
                    MaxWidth = settings.ThumbnailSize,
                    MaxHeight = settings.ThumbnailSize, 
                    Format = "jpg"
                };
            _resizeSettings.Add("quality", ImageQuality);

            //create FullsizedImagesFolder & SmallImagesFolder subfolders
            string largeFilesFolder = Path.Combine(settings.ImagesLocalFolder, FullsizedImagesFolder);
            string smallFilesFolder = Path.Combine(settings.ImagesLocalFolder, SmallImagesFolder);

            if (!Directory.Exists(largeFilesFolder))
                Directory.CreateDirectory(largeFilesFolder);
            if (!Directory.Exists(smallFilesFolder))
                Directory.CreateDirectory(smallFilesFolder);
            
            _lastPhoto = Directory
                .GetFiles(largeFilesFolder, "*.jpg")
                .Select(i => int.Parse(Path.GetFileNameWithoutExtension(i).ToLower().Replace(".jpg", "")))
                .OrderByDescending(i => i)
                .FirstOrDefault();

            if (_lastPhoto < 1)
                _lastPhoto = 1;
        }

        public int AppendFile(byte[] file)
        {
            lock (SyncObj)
            {
                _lastPhoto++;
                var thumbnail = CreateThumbnail(file);
                string filePath = Path.Combine(_settings.ImagesLocalFolder, FullsizedImagesFolder, _lastPhoto + ".jpg");
                string thumbnailPath = Path.Combine(_settings.ImagesLocalFolder, SmallImagesFolder, _lastPhoto + ".jpg");
                File.WriteAllBytes(filePath, file);
                File.WriteAllBytes(thumbnailPath, thumbnail);
                return _lastPhoto;
            }
        }

        private byte[] CreateThumbnail(byte[] file)
        {
            using (var srcStream = new MemoryStream(file))
            {
                using (var destStream = new MemoryStream())
                {
                    ImageBuilder.Current.Build(srcStream, destStream, _resizeSettings);
                    return destStream.ToArray();
                }
            }
        }

        public void ShrinkStorage(int[] actualIds)
        {
            lock (SyncObj)
            {
                var actualIdDictionary = actualIds.ToDictionary(i => i);

                string largeFilesFolder = Path.Combine(_settings.ImagesLocalFolder, FullsizedImagesFolder);
                string smallFilesFolder = Path.Combine(_settings.ImagesLocalFolder, SmallImagesFolder);
                string[] folders = { largeFilesFolder, smallFilesFolder };

                foreach (var folder in folders)
                {
                    foreach (var file in Directory.GetFiles(folder, "*.jpg"))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        int id;
                        if (int.TryParse(fileName, out id))
                        {
                            if (!actualIdDictionary.ContainsKey(id))
                            {
                                SafeFileDelete(file);
                            }
                        }
                        else
                        {
                            SafeFileDelete(file);
                        }
                    }
                } 
            }
        }

        private void SafeFileDelete(string file)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception exc)
            {
                _logger.Exception(exc, "File: {0}", file);
            }
        }
    }
}
