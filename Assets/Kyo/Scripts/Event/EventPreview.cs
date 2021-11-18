using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventPreview : MonoBehaviour
{
    [SerializeField] protected Vector3 offset = Vector3.zero;
    [SerializeField] private Vector3 fixOffset = Vector3.zero;
    [SerializeField] protected TMP_Text eventDesc = null;
    [SerializeField] protected TMP_Text moveability = null;
    [SerializeField] protected TMP_Text amaType = null;
    [SerializeField] private TMP_Text pointRange = null;

    public void Init()
    {
        ResetPreview();
        gameObject.SetActive(false);
    }

    public void SetEventDesc(string EventDesc)
    {
        eventDesc.text += EventDesc;
    }

    public void SetMoveability(string Moveability)
    {
        moveability.text = Moveability;
    }

    public void AddMoveability(string Moveability)
    {
        moveability.text += Moveability;
    }

    public void SetAMATypeActive(bool Active)
    {
        amaType.gameObject.SetActive(Active);
    }

    public void SetAMAType(string AMAType)
    {
        amaType.text += AMAType;
    }

    public void SetPointRangeActive(bool Active)
    {
        pointRange.gameObject.SetActive(Active);
    }

    public void SetPointRange(string PointRange)
    {
        pointRange.text += PointRange;
    }

    public virtual void ResetPreview()
    {
        eventDesc.text = "";
        moveability.text = "";
        amaType.text = "";
        pointRange.text = "";
    }

    public void SetPosition(Vector3 Position)
    {
        transform.localPosition = Position + offset;
    }

    public void FixPosition()
    {
        transform.localPosition += fixOffset;
    }
}
