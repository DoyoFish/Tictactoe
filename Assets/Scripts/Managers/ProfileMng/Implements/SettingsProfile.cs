using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsProfile : IProfile
{
    public int BgmVolume { get; set; }

    public int EffectVolumn { get; set; }

    public void LoadProfile(JObject json)
    {
        BgmVolume = -1;
        EffectVolumn = -1;
        if (json != null)
        {
            BgmVolume = json.GetData<int>("BgmVolume");
            EffectVolumn = json.GetData<int>("EffectVolumn");
        }
    }

    public string ToJson()
    {
        JObject json = new JObject();
        json["BgmVolume"] = BgmVolume;
        json["EffectVolumn"] = EffectVolumn;
        return json.ToString();
    }
}
