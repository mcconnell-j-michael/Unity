using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class WidthBinder : MonoBehaviour
{
    public RectTransform bound;
    private RectTransform rT;

    public float widthFactor = 1;

    // Start is called before the first frame update
    void Start()
    {
        rT = (RectTransform)this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs((bound.rect.width * widthFactor) - rT.rect.width) > .01f)
        {
            rT.sizeDelta = new Vector2(bound.rect.width * widthFactor, rT.sizeDelta.y);
            LayoutRebuilder.ForceRebuildLayoutImmediate(bound);
        }
    }
}
