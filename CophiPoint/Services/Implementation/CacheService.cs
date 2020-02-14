using CophiPoint.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CophiPoint.Services.Implementation
{
    public class CacheService : ICacheService
    {
        private class PropertiesObject
        {
            public string UrlPath { get; set; }

            public string MediaType { get; set; }

            public string ContentVersion { get; set; }

            public string RequiredVersion { get; set; }
            public string FileName { get; set; }
        }
        
        private const string CachePropretyInfoKey = "CachePropertyInfosStore";

        private Dictionary<string, PropertiesObject> _propertiesInfo = null;

        private string GetFileName(PropertiesObject property) => Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, property.FileName);
        
        private IDictionary<string, PropertiesObject> GetProperties()
        {
            if(_propertiesInfo == null)
            {
                if(Application.Current.Properties.TryGetValue(CachePropretyInfoKey, out var json))
                {
                    var values = JsonConvert.DeserializeObject<List<PropertiesObject>>((string)json);
                    _propertiesInfo = values.ToDictionary(x => x.UrlPath);
                }
                else
                {
                    _propertiesInfo = new Dictionary<string, PropertiesObject>();
                }
            }
            return _propertiesInfo;
        }
        private async Task StoreProperties()
        {
            var json = JsonConvert.SerializeObject(_propertiesInfo.Values.ToList());
            Application.Current.Properties[CachePropretyInfoKey] = json;
            await Application.Current.SavePropertiesAsync();
        }

        public async Task<bool> SetRequiredVersionToAll(string newVersion)
        {
            var result = false;
            foreach (var value in GetProperties())
            {
                if (value.Value.RequiredVersion != newVersion)
                {
                    value.Value.RequiredVersion = newVersion;

                    result = true;
                    File.Delete(GetFileName(value.Value));
                }
            }

            if (result)
            {
                await StoreProperties();
            }
            return result;
        }

        public Task<bool> IsUpToDate(Uri requestUri)
        {
            var result = GetProperties().TryGetValue(requestUri.AbsolutePath, out var value)
                && value.RequiredVersion == value.ContentVersion
                && File.Exists(GetFileName(value));
            
            return Task.FromResult(result);
        }

        public Task<(string content, string mediaType)> GetValue(Uri requestUri)
        {
            if(GetProperties().TryGetValue(requestUri.AbsolutePath, out var value))
            {
                var content = File.ReadAllText(GetFileName(value), Encoding.UTF8);

                return Task.FromResult((content, value.MediaType));
            }
            throw new KeyNotFoundException();
        }

        public async Task SetValue(Uri requestUri, string version, string content, string mediaType)
        {
            var properties = GetProperties();

            if (properties.TryGetValue(requestUri.AbsolutePath, out var value))
            {
                if (value.RequiredVersion != value.ContentVersion)
                {
                    value.ContentVersion = version;
                    value.MediaType = mediaType;

                    File.WriteAllText(GetFileName(value), content);
                }
                else MicroLogger.LogWarning("Cache same version error");
            }
            else
            {
                value = new PropertiesObject
                {
                    UrlPath = requestUri.AbsolutePath,
                    MediaType = mediaType,
                    ContentVersion = version,
                    RequiredVersion = version,
                    FileName = Path.GetRandomFileName()
                };
                properties.Add(requestUri.AbsolutePath, value);

                File.WriteAllText(GetFileName(value), content);
            }

            await StoreProperties();
        }
    }
}
