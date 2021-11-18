using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OceanAreas
{
    Area_01,
    Area_02,
    Area_03,
    Area_04,
    Area_05,
    Area_06,
    Area_07,
    Area_08,
    Area_09,
    None,
}

public class OceanParts : MonoBehaviour
{
    [SerializeField] protected Image[] parts = null;

    public void Init()
    {
        parts = GetComponentsInChildren<Image>();
    }

    public void Move(Vector2 Offset)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].transform.localPosition += new Vector3(Offset.x, Offset.y, 0.0f);

            // 画面外になったら、ループさせる
            FixPostion(i);
        }
    }

    // 画面外になったら、ループさせる
    public void FixPostion(int Index)
    {
        Vector2 fixPos = parts[Index].transform.localPosition;

        // X軸
        if (parts[Index].transform.localPosition.x < -GlobalInfo.instance.halfMapSize.x)
        {
            fixPos.x = parts[Index].transform.localPosition.x + GlobalInfo.instance.mapSize.x;
        }
        else if (parts[Index].transform.localPosition.x > GlobalInfo.instance.halfMapSize.x)
        {
            fixPos.x = parts[Index].transform.localPosition.x - GlobalInfo.instance.mapSize.x;
        }

        parts[Index].transform.localPosition = fixPos;
    }
}
