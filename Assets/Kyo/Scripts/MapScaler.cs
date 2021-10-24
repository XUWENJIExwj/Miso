using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScaler : Monosingleton<MapScaler>
{
    [SerializeField] private GameObject[] mapPads = null;
    [SerializeField] private float zoomSpeed = 1.0f;
    [SerializeField] private float minScale = 1.0f;
    [SerializeField] private float maxScale = 2.0f;

    public void Zoom()
    {
        float val = Input.GetAxis("Mouse ScrollWheel");

        if (val == 0)
        {
            return;
        }

        // �Y�[������X�P�[�����v�Z
        Vector3 scale = mapPads[0].transform.localScale * (1 + val * zoomSpeed * Time.deltaTime);

        if (scale.x < minScale)
        {
            return;
        }

        if (scale.x > maxScale)
        {
            scale = new Vector2(maxScale, maxScale);
        }

        // �Y�[���v�Z��̃X�P�[����K�p
        mapPads[0].transform.localScale = scale;
        mapPads[1].transform.localScale = scale;

        RouteManager.instance.SetLineWidthWithMapScale(scale.x);
    }
}
