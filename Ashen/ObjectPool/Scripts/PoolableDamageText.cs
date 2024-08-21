using DG.Tweening;
using TMPro;
using UnityEngine;

public class PoolableDamageText : PoolableBehaviour
{
    public DOTweenAnimation fader;
    public DOTweenAnimation mover;
    public TextMeshProUGUI text;

    private float defaultScaleX;
    private float defaultScaleY;

    public void Awake()
    {
        defaultScaleX = transform.localScale.x;
        defaultScaleY = transform.localScale.y;
    }

    public override void Initialize()
    {
        transform.localScale = new Vector3(defaultScaleX, defaultScaleY, 1f);
        gameObject.SetActive(true);
        enabled = true;
    }

    protected override void InternalDisable()
    {
        gameObject.SetActive(false);
        enabled = false;
    }

    public override bool Enabled()
    {
        return enabled;
    }
}
