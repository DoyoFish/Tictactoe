using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Doyo.Common;

public sealed class MsgMng
{
    private static readonly MsgMng _instance;

    public static MsgMng Instance { get { return _instance; } }

    static MsgMng()
    {
        _instance = new MsgMng();
    }

    private MsgMng() { }

    private readonly Dictionary<MsgTypeEnum, List<Action<Message>>> _dic = new Dictionary<MsgTypeEnum, List<Action<Message>>>();

    #region Add
    private void _AddListenter(MsgTypeEnum type, Action<Message> action)
    {
        if (!_dic.ContainsKey(type))
        {
            _dic.Add(type, new List<Action<Message>>());
        }
        if (_dic[type].Contains(action))
        {
            Debug.LogErrorFormat("MsgMng: 重复注册了{0}的监听, 类名为{1}, MsgTypeEnum:{2}", action.Method.Name, action.Method.ReflectedType, type.ToString());
            return;
        }
        if (action == null)
        {
            Debug.LogErrorFormat("MsgMng: 添加的监听为空");
            return;
        }

        _dic[type].Add(action);
    }

    public static void AddListener(MsgTypeEnum type, Action<Message> action)
    {
        Instance._AddListenter(type, action);
    }
    #endregion

    #region Remove
    private void _RemoveListener(MsgTypeEnum type, Action<Message> listener)
    {
        if (!_dic.ContainsKey(type) || !_dic[type].Contains(listener))
        {
            Debug.LogErrorFormat("MsgMng: 不包含名为{0}的监听，或没有{1}这个订阅函数", type.ToString(), listener.Method.Name);
            return;
        }
        _dic[type].Remove(listener);
        if (_dic[type].Count == 0)
        {
            _dic.Remove(type);
        }
    }

    public static void RemoveListener(MsgTypeEnum type, Action<Message> listener)
    {
        Instance._RemoveListener(type, listener);
    }
    #endregion

    #region Dispatch

    private void _Dispatch(MsgTypeEnum type, Message msg)
    {
        if (!_dic.ContainsKey(type))
        {
            return;
        }
        var list = new List<Action<Message>>(_dic[type]);
        for (int index = 0; index < list.Count; ++index)
        {
            var listener = list[index];
            if (!_dic.ContainsKey(type) || !_dic[type].Contains(listener))
            {
                Debug.LogErrorFormat("MsgMng: 不包含名为{0}的监听，或没有{1}这个订阅函数", type.ToString(), listener.Method.Name);
                return;
            }
            //内存泄露检测
            if (listener.Target is UnityEngine.Object && (null == listener.Target as UnityEngine.Object))
            {
                string info = string.Format("类名: {0}， 方法名: {1}", listener.Method.ReflectedType, listener.Method.Name);
                Debug.LogErrorFormat("MsgMng: 内存泄露， 请及时移除 MsgTypeEnum = {0} | {1} 监听器 {2}", (int)type, type.ToString(), info);
                _dic[type].Remove(listener);
                continue;
            }
            listener?.Invoke(msg);
        }

        if (_dic.ContainsKey(type) && _dic[type].Count == 0)
        {
            _dic.Remove(type);
            return;
        }
    }

    public static void Dispatch(MsgTypeEnum type, Message msg = null)
    {
        Instance._Dispatch(type, msg);
    }

    public static void Dispatch(MsgTypeEnum type, object obj)
    {
        Instance._Dispatch(type, new Message(obj));
    }
    #endregion
}
