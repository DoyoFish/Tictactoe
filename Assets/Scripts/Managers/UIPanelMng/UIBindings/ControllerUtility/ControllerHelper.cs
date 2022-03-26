using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ControllerHelper
{

    private static Dictionary<string, string> m_cacheForFound = new Dictionary<string, string>();
    private static List<string> m_cachedNodes = new List<string>();

    public static T GetController<T>(this Transform node, string controllerName) where T : Component
    {
        Transform trans = GetController(node, controllerName);
        if (!trans)
        {
            Debug.LogError($"{node.name} doesn't have {controllerName} child");
            return null;
        }
        T t = trans.GetComponent<T>();
        if (!t)
        {
            Debug.LogError($"{trans.name} doesn't have {typeof(T).Name} component");
            return null;
        }
        return t;
    }

    public static Transform GetController(this Transform node, string controllerName)
    {
        if (!node)
        {
            return null;
        }

        SaveToCache(node, node);

        string nodeName = node.name;
        string key = string.Concat(nodeName, "+", controllerName);

        // 尝试直接从子级寻找
        Transform child = node.Find(controllerName);
        if (child)
        {
            return child;
        }

        // 尝试从内存中寻找
        child = FindFromCache(key, node);
        if (child)
        {
            return child;
        }

        Transform grandson;
        string path;
        for (int i = 0; i < node.childCount; ++i)
        {
            child = node.GetChild(i);
            grandson = GetController(child, controllerName);
            if (grandson)
            {
                path = grandson.PathToTarget(node);
                if (!m_cacheForFound.ContainsKey(key))
                {
                    m_cacheForFound.Add(key, path);
                }
                return grandson;
            }
        }
        return null;
    }

    private static void SaveToCache(Transform root, Transform node)
    {
        if (!root || !node)
        {
            Debug.LogErrorFormat("SaveToCacheError: root({0}), node({1})", root ? "not null" : "null", node ? "not null" : "null");
            return;
        }
        if (m_cachedNodes.Contains(root.name))
        {
            return;
        }

        if (node.childCount != 0)
        {
            string key;
            string path;
            string rootName = root.name;
            Transform childNode;
            Transform grandsonChild;
            int childCount = node.childCount;
            int grandsonCount;
            for (int childIndex = 0; childIndex < childCount; ++childIndex)
            {
                childNode = node.GetChild(childIndex);
                grandsonCount = childNode.childCount;
                for (int grandsonIndex = 0; grandsonIndex < grandsonCount; ++grandsonIndex)
                {
                    grandsonChild = childNode.GetChild(grandsonIndex);
                    path = grandsonChild.PathToTarget(root);
                    key = string.Concat(rootName, "+", grandsonChild.name);

                    if (!m_cacheForFound.ContainsKey(key))
                    {
                        m_cacheForFound.Add(key, path);
                    }
                }
                SaveToCache(root, childNode);
            }
            m_cachedNodes.Add(rootName);
        }
    }

    private static Transform FindFromCache(string key, Transform parent)
    {
        Transform child;
        if (m_cacheForFound.ContainsKey(key))
        {
            child = parent.Find(m_cacheForFound[key]);
            if (!child)
            {
                m_cacheForFound.Remove(key);
            }
            return child;
        }
        return null;
    }

    public static string PathToTarget(this Transform self, Transform target)
    {
        if (!self)
        {
            return string.Empty;
        }
        if (self == target)
        {
            return self.name;
        }
        bool belongTo = false;
        string path = self.name;
        Transform parent = self.parent;
        while (parent)
        {
            path = string.Concat(parent.name, "/", path);
            if (parent == target)
            {
                belongTo = true;
                break;
            }
            parent = parent.parent;
        }

        if (belongTo)
        {
            return path;
        }

        return string.Empty;
    }
}
