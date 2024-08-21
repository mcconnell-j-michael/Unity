using UnityEngine;

namespace Ashen.DeliverySystem
{

    [CreateAssetMenu(fileName = nameof(ExtendedEffectTag), menuName = "Custom/Enums/" + nameof(ExtendedEffectTags) + "/Type")]
    public class ExtendedEffectTag : A_EnumSO<ExtendedEffectTag, ExtendedEffectTags>
    { }
}