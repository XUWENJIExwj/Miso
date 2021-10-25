using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCanvasController : MonoBehaviour
{
    public void Start()
    {
        Init();
    }

    public abstract void Init();
}
