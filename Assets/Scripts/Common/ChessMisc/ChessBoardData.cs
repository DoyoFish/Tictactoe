using Doyo.Common;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardData
{

    private readonly Dictionary<int, ChessPiecesType> _chessPieces = new Dictionary<int, ChessPiecesType>(GameMng.MaxPiecesCount);

    public ChessPiecesType this[int position]
    {
        get
        {
            return _chessPieces.Get(position);
        }
        set
        {
            _chessPieces[position] = value;
        }
    }

    public void LoadJson(JObject data)
    {
        var keys = data.Properties();
        foreach (var key in keys)
        {
            _chessPieces[key.Name.ParseInt()] = (ChessPiecesType)data.GetData<int>(key.Name);
        }
    }

    public List<int> GetEmptyBoxes()
    {
        var list = new List<int>(GameMng.MaxPiecesCount);
        for (int i = 0; i < GameMng.MaxPiecesCount; ++i)
        {
            if (!_chessPieces.ContainsKey(i) || _chessPieces[i] == ChessPiecesType.None)
            {
                list.Add(i);
            }
        }
        return list;
    }

    public Dictionary<int, ChessPiecesType> GetAllBoxes()
    {
        return _chessPieces;
    }

    public void Clear()
    {
        _chessPieces.Clear();
    }

    public string ToJson()
    {
        var json = new JObject();
        foreach (var pair in _chessPieces)
        {
            if (pair.Value == ChessPiecesType.None)
            {
                continue;
            }
            json[pair.Key.ToString()] = (int)pair.Value;
        }
        return json.ToString();
    }
}