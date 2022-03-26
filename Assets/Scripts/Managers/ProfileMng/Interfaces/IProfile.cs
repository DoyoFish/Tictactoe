using Newtonsoft.Json.Linq;

public interface IProfile 
{
    void LoadProfile(JObject json);

    string ToJson();
}
