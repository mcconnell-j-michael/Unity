using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "DescriptionArgumentType", menuName = "Custom/Enums/DescriptionArgumentType/Type")]
public class DescriptionArgumentType : A_EnumSO<DescriptionArgumentType, DescriptionArgumentTypes>
{
    public string argument;
}
