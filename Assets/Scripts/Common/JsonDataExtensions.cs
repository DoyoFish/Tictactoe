using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class JsonDataExtensions
{
    public static T GetData<T>(this JObject json, string key, T defaultValue = default(T))
    {
        JToken value;
        if (json.TryGetValue(key, out value))
        {
            return value.ToObject<T>();
        }
        return defaultValue;
    }
}


