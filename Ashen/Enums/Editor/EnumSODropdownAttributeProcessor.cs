using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;

[ResolverPriority(-99999)]
public class EnumSODropdownAttributeProcessor : OdinAttributeProcessor
{
    public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
    {
        bool hasEnumSODropdown = false;
        bool alreadyHasValueDropdown = false;
        for (int i = 0; i < attributes.Count; i++)
        {
            Type attributeType = attributes[i].GetType();
            if (attributeType == typeof(EnumSODropdownAttribute))
            {
                hasEnumSODropdown = true;
            }
            else if (attributeType == typeof(ValueDropdownAttribute))
            {
                alreadyHasValueDropdown = true;
            }
        }

        if (!alreadyHasValueDropdown && hasEnumSODropdown)
        {
            Type listEnumType = StaticUtilities.GetSublcassOf(typeof(List<>), property.Info.TypeOfValue);
            Type enumType = null;
            if (listEnumType != null)
            {
                enumType = StaticUtilities.GetSublcassOf(typeof(A_EnumSO<,>), listEnumType.GenericTypeArguments[0]);
            }
            else
            {
                enumType = StaticUtilities.GetSublcassOf(typeof(A_EnumSO<,>), property.Info.TypeOfValue);
            }
            if (enumType != null)
            {
                Type[] genericArguments = enumType.GenericTypeArguments;
                Type exactEnumType = genericArguments[0];
                ValueDropdownAttribute attribute = new ValueDropdownAttribute("@" + exactEnumType.Name + ".GetList()")
                {
                    IsUniqueList = true
                };
                attributes.Add(attribute);
            }
        }
    }
}
