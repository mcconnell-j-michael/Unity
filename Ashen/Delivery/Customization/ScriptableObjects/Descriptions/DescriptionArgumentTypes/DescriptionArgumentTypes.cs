using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "DescriptionArgumentTypes", menuName = "Custom/Enums/DescriptionArgumentType/Types")]
public class DescriptionArgumentTypes : A_EnumList<DescriptionArgumentType, DescriptionArgumentTypes>
{
    public DescriptionArgumentType EQUATION_AVERAGE;
    public DescriptionArgumentType EQUATION_RANGE;
    public DescriptionArgumentType EQUATION_DEFINITION;
    public DescriptionArgumentType STRING;
}
