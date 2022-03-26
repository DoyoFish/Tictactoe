using Newtonsoft.Json.Linq;

public class ChessBoard
{
    public ChessBoardData Data { get; set; }

    public ChessPlayerType CurrentChessPlayer { get; set; }

    public ChessPiecesType PlayerChess { get; set; }

    public ChessPlayerType FirstChessPlayer { get; set; }

    public int Turn { get; set; }

    public ChessBoard()
    {
        Data = new ChessBoardData();
        CurrentChessPlayer = ChessPlayerType.None;
        Turn = 0;
    }

    public void Copy(ChessBoard data)
    {
        CurrentChessPlayer = data.CurrentChessPlayer;
        PlayerChess = data.PlayerChess;
        FirstChessPlayer = data.FirstChessPlayer;
        Data = data.Data;
    }

    public void LoadJson(string data)
    {
        var json = JObject.Parse(data);
        LoadJson(json);
    }

    public void LoadJson(JObject json)
    {
        if (json == null)
        {
            return;
        }
        Data.LoadJson(JObject.Parse(json["data"].ToString()));
        var turn = json.GetData<int>("turn");
        Turn = turn;
        var currentPlayer = json.GetData<int>("currentPlayer");
        CurrentChessPlayer = (ChessPlayerType)currentPlayer;
        var firstPlayer = json.GetData<int>("firstPlayer");
        FirstChessPlayer = (ChessPlayerType)firstPlayer;
        var playerChess = json.GetData<int>("playerChess");
        PlayerChess = (ChessPiecesType)playerChess;
    }
    
    public void Clear()
    {
        Turn = 0;
        Data.Clear();
    }

    public string ToJson()
    {
        var json = new JObject();
        var data = Data.ToJson();
        json["data"] = data;
        json["currentPlayer"] = (int)CurrentChessPlayer;
        json["firstPlayer"] = (int)FirstChessPlayer;
        json["playerChess"] = (int)PlayerChess;
        json["turn"] = Turn;
        return json.ToString();
    }
}