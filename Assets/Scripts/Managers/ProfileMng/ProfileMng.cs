using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileMng : MonoSingleton<ProfileMng>
{
    protected override void Construct()
    {
    }

    protected override void Release()
    {
    }


    public string UserName
    {
        get { return PlayerPrefs.GetString("UserName", string.Empty); }
        set
        {
            PlayerPrefs.SetString("UserName", value);
            PlayerPrefs.Save();
        }
    }

    public string UserSign
    {
        get { return PlayerPrefs.GetString("UserSign", string.Empty); }
        set
        {
            PlayerPrefs.SetString("UserSign", value);
            PlayerPrefs.Save();
        }
    }

    public string UserAvatar
    {
        get { return PlayerPrefs.GetString("UserAvatar", "avatar_01"); }
        set
        {
            PlayerPrefs.SetString("UserAvatar", value);
            PlayerPrefs.Save();
        }
    }

    public SettingsProfile SettingsProfile
    {
        get
        {
            if(_settingsProfile == null)
            {
                _settingsProfile = ProfileHelper.GetLocalProfile<SettingsProfile>("LocalSettings");
            }
            return _settingsProfile;
        }
    }
    private SettingsProfile _settingsProfile = null;

    public GameDataProfile GameDataProfile
    {
        get
        {
            if (_gameDataProfile == null)
            {
                _gameDataProfile = ProfileHelper.GetLocalProfile<GameDataProfile>("GameData");
            }
            return _gameDataProfile;
        }
    }
    private GameDataProfile _gameDataProfile;

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LocalSettings", SettingsProfile.ToJson());
        PlayerPrefs.SetString("GameData", GameDataProfile.ToJson());
        PlayerPrefs.Save();
    }
}
