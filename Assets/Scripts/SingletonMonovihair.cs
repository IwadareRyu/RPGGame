using System;
using UnityEngine;

public abstract class SingletonMonovihair<T> : MonoBehaviour where T : MonoBehaviour
{
    protected abstract bool _dontDestroyOnLoad { get; }

    private static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                Type t = typeof(T);
                instance = (T)FindObjectOfType(t);
                if (!instance)
                {
                    Debug.LogError(t + "はありません。");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
