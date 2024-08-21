using UnityEngine;
using UnityEngine.UI;

public class StatusEffectSymbolUI : MonoBehaviour
{
    public PoolableBehaviour poolableBehaviour;
    public Image greyScaleImage;
    public Image filler;

    public void SetFill(float percentage)
    {
        filler.fillAmount = percentage;
    }
}
