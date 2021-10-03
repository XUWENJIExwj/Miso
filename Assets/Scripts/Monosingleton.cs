using UnityEngine;

public abstract class Monosingleton<T> : MonoBehaviour where T : Monosingleton<T>
{
    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
                Debug.Log(typeof(T).ToString() + " is NULL.");

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this as T;
        InitAwake();
    }

    public virtual void InitAwake()
    {
        //optional to override
    }
}
