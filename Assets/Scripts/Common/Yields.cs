using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public static class Yielders
{
    public static bool Enabled { get; set; }

    public static int InternalCounter { get; set; }

    public static FloatComparer FloatComparer = new FloatComparer(0.01f);

    private static readonly WaitForEndOfFrame _endOfFram = new WaitForEndOfFrame();

    private static readonly WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsYielders = new Dictionary<float, WaitForSeconds>(100, Yielders.FloatComparer);

    static Yielders()
    {
        Enabled = true;
        InternalCounter = 0;
    }

    public static WaitForEndOfFrame EndOfFrame
    {
        get
        {
            InternalCounter++;
            if (!Enabled)
            {
                return new WaitForEndOfFrame();
            }
            return _endOfFram;
        }
    }

    public static WaitForFixedUpdate FixedUpdate
    {
        get
        {
            InternalCounter++;
            if (!Enabled)
            {
                return new WaitForFixedUpdate();
            }
            return _fixedUpdate;
        }
    }

    public static WaitForSeconds GetWaitForSeconds(float seconds)
    {
        InternalCounter++;
        if (!Enabled)
        {
            return new WaitForSeconds(seconds);
        }
        if (!_waitForSecondsYielders.ContainsKey(seconds))
        {
            _waitForSecondsYielders.Add(seconds, new WaitForSeconds(seconds));
        }
        return _waitForSecondsYielders[seconds];
    }

    public static WaitForSecondsRealtime GetWaitForSecondsRealtime(float seconds)
    {
        InternalCounter++;
        return new WaitForSecondsRealtime(seconds);
    }

    public static void ClearWaitForSeconds()
    {
        _waitForSecondsYielders.Clear();
    }
}