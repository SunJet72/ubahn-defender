using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
[Serializable]
public class ChildScrollRect : ScrollRect
{
    [SerializeField] ScrollRect parentScrollRect;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        parentScrollRect?.OnBeginDrag(eventData);
        base.OnBeginDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        parentScrollRect?.OnEndDrag(eventData);
        base.OnEndDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        parentScrollRect?.OnDrag(eventData);
        base.OnDrag(eventData);
    }
}
