using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint.Services.Implementation
{
    public class CacheService : ICacheService
    {
        private class PropertiesObject
        {
            public string MediaType { get; set; }
            public string Version { get; set; }
        }

        public CacheService()
        {
            Version = Application.Current.Properties.Values
                .Where(x => x is PropertiesObject)
                .Select(x => x as PropertiesObject)
                .Min(x => x.Version);
        }

        public string Version { get; private set; }

        public async Task Clear()
        {
            foreach(var value in Application.Current.Properties)
            {
                if(value.Value is PropertiesObject)
                {
                    File.Delete(GetFileName(value.Key));
                }
            }
            Application.Current.Properties.Clear();
            await Application.Current.SavePropertiesAsync();
        }
        
        private string GetFileName(Uri requestUri) => GetFileName(requestUri.AbsolutePath);

        private string GetFileName(string requestUri)
        {
            var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(localFolder, requestUri.Substring(1));
        }

        public bool Contains(Uri requestUri)
        {
            return Application.Current.Properties.ContainsKey(requestUri.AbsolutePath)
                && File.Exists(GetFileName(requestUri));
        }

        public CachedItem Get(Uri key)
        {
            if(Application.Current.Properties.TryGetValue(key.AbsolutePath, out var value))
            {
                var propValue = (PropertiesObject)value;
                var content = File.ReadAllText(GetFileName(key), Encoding.UTF8);

                return new CachedItem
                {
                    MediaType = propValue.MediaType,
                    Version = propValue.Version,
                    Content = content
                };
            }
            return null;
        }

        public async Task Set(Uri key, CachedItem value)
        {
            Application.Current.Properties.Add(key.AbsolutePath, new PropertiesObject
            {
                MediaType = value.MediaType,
                Version = value.Version
            });

            var fileName = GetFileName(key);
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            File.WriteAllText(fileName, value.Content, Encoding.UTF8);

            await Application.Current.SavePropertiesAsync();
        }
    }
}
