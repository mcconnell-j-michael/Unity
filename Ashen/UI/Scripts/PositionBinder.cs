using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class PositionBinder : MonoBehaviour
{
    public List<HorizontalOrVerticalLayoutGroup> updateList;

    public bool bindX;
    public bool bindY;
    public bool bindH;
    public bool bindW;

    public RectTransform bound;
    private RectTransform rT;

    void Start()
    {
        rT = (RectTransform)this.transform;
    }
    
    void Update()
    {
        bool update = false;
        if (bindX && (Math.Abs(bound.anchoredPosition.x - rT.anchoredPosition.x) > 0.001f))
        {
            rT.anchoredPosition = new Vector2(bound.anchoredPosition.x, rT.anchoredPosition.y);
            update = true;
        }
        if (bindY && (Math.Abs(bound.anchoredPosition.y - rT.anchoredPosition.y) > 0.001f))
        {
            rT.anchoredPosition = new Vector2(rT.anchoredPosition.x, bound.anchoredPosition.y);
            update = true;
        }
        if (bindH && (Math.Abs(bound.rect.height - rT.rect.height) > 0.001f))
        {
            rT.sizeDelta = new Vector2(rT.rect.width, bound.rect.height);
            update = true;
        }
        if (bindW && (Math.Abs(bound.rect.width - rT.rect.width) > 0.001f))
        {
            rT.sizeDelta = new Vector2(bound.rect.width, rT.rect.height);
            update = true;
        }
        if (update && updateList != null)
        {
            foreach (HorizontalOrVerticalLayoutGroup mb in updateList)
            {
                mb.enabled = false;
                mb.enabled = true;
            }
        }
    }
}
