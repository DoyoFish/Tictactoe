using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProfileHelper
{
    public static T GetLocalProfile<T>(string prefsKey) where T : IProfile, new()
    {
        var data = PlayerPrefs.GetString(prefsKey, "{}");
        var json = JObject.Parse(data);
        var t = new T();
        t.LoadProfile(json);
        return t;
    }
}