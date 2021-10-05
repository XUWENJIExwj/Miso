using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButton : MonoBehaviour
{
    public void Move(Vector2 Offset, Vector2 MapSize, Vector2 HalfMapSize)
    {
        transform.localPosition += new Vector3(Offset.x, Offset.y, 0.0f);

        // 画面外になったら、ループさせる
        FixPostion(MapSize, HalfMapSize);
    }

    // 画面外になったら、ループさせる
    public void FixPostion(Vector2 MapSize, Vector2 HalfMapSize)
    {
        Vector2 fixPos = transform.localPosition;

        // X軸
        if (transform.localPosition.x < -HalfMapSize.x)
        {
            fixPos.x = transform.localPosition.x + MapSize.x;
        }
        else if (transform.localPosition.x > HalfMapSize.x)
        {
            fixPos.x = transform.localPosition.x - MapSize.x;
        }

        // Y軸
        if (transform.localPosition.y < -HalfMapSize.y)
        {
            fixPos.y = transform.localPosition.y + MapSize.y;
        }
        else if (transform.localPosition.y > HalfMapSize.y)
        {
            fixPos.y = transform.localPosition.y - MapSize.y;
        }

        transform.localPosition = fixPos;
    }
}
