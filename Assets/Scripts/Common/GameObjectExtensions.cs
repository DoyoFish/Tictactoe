using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class GameObjectExtensions
{
    public static T CreateLink<T>(this GameObject parent) where T : Component
    {
        return parent.CreateLink<T>(typeof(T).Name);
    }

    public static T CreateLink<T>(this GameObject parent, string linkerName) where T : Component
    {
        T instance = parent.GetComponent<T>();
        if (!instance)
        {
            instance = new GameObject(linkerName).AddComponent<T>();
            instance.transform.SetParent(parent.transform);
        }
        return instance;
    }

    public static T GetOrAddComponent<T>(this GameObject current) where T : Component
    {
        T t = current.GetComponent<T>();
        if (!t)
        {
            t = current.AddComponent<T>();
        }
        return t;
    }
}