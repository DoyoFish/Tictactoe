using System.Collections;
using System.Collections.Generic;
using UIBinding;
using UnityEngine;

public class ChessPlayerInfo : ItemContext
{
    public string HeadImage
    {
        get { return _propertyHeadImg.GetValue(); }
        set { _propertyHeadImg.SetValue(value); }
    }
    private readonly Property<string> _propertyHeadImg = new Property<string>();

    public string Alias
    {
        get { return _propertyAlias.GetValue(); }
        set { _propertyAlias.SetValue(value); }
    }
    private readonly Property<string> _propertyAlias = new Property<string>();

    public string Sign
    {
        get { return _propertySign.GetValue(); }
        set { _propertySign.SetValue(value); }
    }
    private readonly Property<string> _propertySign = new Property<string>();

    public int Turn
    {
        get { return _propertyTurn.GetValue(); }
        set { _propertyTurn.SetValue(value); }
    }
    private readonly Property<int> _propertyTurn = new Property<int>();

    public string ChessType
    {
        get { return _propertyChessType.GetValue(); }
        set { _propertyChessType.SetValue(value); }
    }
    private readonly Property<string> _propertyChessType = new Property<string>();


}
