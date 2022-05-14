using Newtonsoft.Json;
using System;

namespace ricaun.Revit.Github.Services
{
    /// <summary>
    /// JsonService
    /// </summary>
    public class JsonService
    {
        private readonly JsonSerializerSettings settings;
        /// <summary>
        /// JsonService
        /// </summary>
        public JsonService()
        {
            settings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Get JsonSerializerSettings
        /// </summary>
        /// <returns></returns>
        public JsonSerializerSettings GetSettings() => settings;

        /// <summary>
        /// SerializeObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SerializeObject<T>(T value)
        {
            return JsonConvert.SerializeObject(value, settings);
        }

        /// <summary>
        /// DeserializeObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}
