using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Datiss.Budget.Web
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
            where T : class
                => tempData[key] = JsonConvert.SerializeObject(value);

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            //reset
            resetTempData(tempData,key);
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
        private static void resetTempData (ITempDataDictionary tempData, string key)
        {
            foreach (var item in tempData)
            {
                if (item.Key != key)
                    tempData.Remove(item.Key);
            }
        }
    }
}
