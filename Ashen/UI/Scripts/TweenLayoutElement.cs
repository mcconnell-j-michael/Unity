using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class TweenLayoutElement : MonoBehaviour
{
    public RectTransform parent;
    public RectTransform toExpand;
    public RectTransform toRetract;

    private float originalWidthExpand = 0f;
    private float originalWidthRetract = 0f;

    private float partialRewindBuffer = 30f;

    public void Start()
    {
        originalWidthExpand = toExpand.rect.width;
        originalWidthRetract = toRetract.rect.width;
    }

    [Button]
    public void Play()
    {
        DOTween.To(() => toRetract.sizeDelta.x, x => toRetract.sizeDelta = new Vector2(x, toRetract.sizeDelta.y), 0f, .2f);
        DOTween.To(() => toExpand.sizeDelta.x, x => toExpand.sizeDelta = new Vector2(x, toExpand.sizeDelta.y), parent.rect.width, .2f);
    }

    public void RewindPartial()
    {
        DOTween.To(() => toRetract.sizeDelta.x, x => toRetract.sizeDelta = new Vector2(x, toRetract.sizeDelta.y), originalWidthRetract - partialRewindBuffer, .2f);
        DOTween.To(() => toExpand.sizeDelta.x, x => toExpand.sizeDelta = new Vector2(x, toExpand.sizeDelta.y), originalWidthExpand + partialRewindBuffer, .2f);
    }

    public void RewindFast()
    {
        DOTween.To(() => toRetract.sizeDelta.x, x => toRetract.sizeDelta = new Vector2(x, toRetract.sizeDelta.y), originalWidthRetract, 0f);
        DOTween.To(() => toExpand.sizeDelta.x, x => toExpand.sizeDelta = new Vector2(x, toExpand.sizeDelta.y), originalWidthExpand, 0f);
    }

    [Button]
    public void Rewind()
    {
        DOTween.To(() => toRetract.sizeDelta.x, x => toRetract.sizeDelta = new Vector2(x, toRetract.sizeDelta.y), originalWidthRetract, .2f);
        DOTween.To(() => toExpand.sizeDelta.x, x => toExpand.sizeDelta = new Vector2(x, toExpand.sizeDelta.y), originalWidthExpand, .2f);
    }
}
