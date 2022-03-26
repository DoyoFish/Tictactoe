using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIBinding;
using System;
using System.Reflection;
using System.Linq;

public abstract class ModelView : MonoBehaviour
{
    public abstract object Instance { get; }

    private readonly Dictionary<string, Property> _properties = new Dictionary<string, Property>();

    private readonly Dictionary<string, MethodInfo> _methods = new Dictionary<string, MethodInfo>();

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    protected virtual void OnValidate()
    {

    }

    public Property FindProperty(string path, string processPath = null)
    {
        if (string.IsNullOrEmpty(processPath) && string.IsNullOrEmpty(path))
        {
            return null;
        }
        if (string.IsNullOrEmpty(processPath))
        {
            processPath = string.Format("_property{0}", path);
        }

        if (_properties.Count == 0 && Instance != null)
        {
            var fields = Instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToList();
            fields.AddRange(Instance.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
            foreach (var field in fields)
            {
                var property = field.GetValue(Instance) as Property;
                if (property != null)
                {
                    _properties.Add(field.Name, property);
                }
            }
        }
        if (!_properties.ContainsKey(processPath))
        {
            return null;
        }
        return _properties[processPath];
    }

    public MethodInfo FindAction(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("in FindAction: path is null");
            return null;
        }

        var methods = Instance.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
        if (_methods.Count == 0)
        {
            foreach (var method in methods)
            {
                if (_methods.ContainsKey(method.Name))
                {
                    continue;
                }
                _methods.Add(method.Name, method);
            }
        }

        if (!_methods.ContainsKey(path))
        {
            Debug.LogError($"{path} 没有找到 函数");
            return null;
        }
        return _methods[path];
    }

}