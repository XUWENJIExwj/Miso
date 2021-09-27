using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCanvasController : MonoBehaviour
{
    public void Awake()
    {
        Init();
    }

    public abstract void Init();
}
