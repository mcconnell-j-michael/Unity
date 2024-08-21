using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class HeightBinder : MonoBehaviour
{
    public RectTransform bound;
    private RectTransform rT;
    // Start is called before the first frame update
    void Start()
    {
        rT = (RectTransform)this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (bound.rect.height != rT.rect.height)
        {
            rT.sizeDelta = new Vector2(rT.rect.width, bound.rect.height);
        }
    }
}
