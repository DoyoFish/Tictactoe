using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<T>();
                if (!_instance)
                {
                    _instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
                }
                if (!_instance)
                {
                    Debug.LogError("未知异常");
                    return null;
                }
                _instance.Construct();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    private static T _instance;

    void OnDestroy()
    {
        Release();
    }

    protected abstract void Construct();

    protected abstract void Release();
}
