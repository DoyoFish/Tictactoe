using Newtonsoft.Json.Linq;

public class GameDataProfile : IProfile
{
    public bool HasLastGame { get; private set; }
    public ChessBoard ChessBoard { get; set; }
    public int PlayerTurns { get; set; }
    public int AITurns { get; set; }

    public void LoadProfile(string data)
    {
        var json = JObject.Parse(data);
        LoadProfile(json);
    }

    public void LoadProfile(JObject json)
    {
        ChessBoard = new ChessBoard();
        if (json != null)
        {
            var chessboardData = json["chessBoard"];
            if (chessboardData != null)
            {
                var data = json["data"];
                ChessBoard.LoadJson((JObject)data);
            }
            PlayerTurns = json.GetData("playerTurns", 0);
            AITurns = json.GetData("aiTurns", 0);
            HasLastGame = json.GetData("hasLastGame", false);
        }
    }

    public void SaveProgrss()
    {
        PlayerTurns = GameMng.Instance.PlayerTurns;
        AITurns = GameMng.Instance.AITurns;
        ChessBoard.Copy(GameMng.Instance.ChessBoard);
        HasLastGame = true;
    }

    public void Clear()
    {
        PlayerTurns = 0;
        AITurns = 0;
        ChessBoard.Clear();
        HasLastGame = false;
    }

    public string ToJson()
    {
        var json = new JObject();
        json["chessBoard"] = ChessBoard.ToJson();
        json["playerTurns"] = PlayerTurns;
        json["aiTurns"] = AITurns;
        json["hasLastGame"] = HasLastGame;
        return json.ToString();
    }
}
