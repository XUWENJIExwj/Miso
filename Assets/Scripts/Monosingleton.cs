<<<<<<< .merge_file_a07520
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
=======
﻿using UnityEngine;
>>>>>>> .merge_file_a15840

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
        Init();
    }

    public virtual void Init()
    {
        //optional to override
    }
}
