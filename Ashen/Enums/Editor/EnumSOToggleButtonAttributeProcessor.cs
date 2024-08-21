using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;

public class EnumSOToggleButtonAttributeProcessor : OdinAttributeProcessor
{
    public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
    {
        bool hasEnumSOToggleButton = false;
        bool alreadyHasValueToggleButton = false;
        for (int i = 0; i < attributes.Count; i++)
        {
            Type attributeType = attributes[i].GetType();
            if (attributeType == typeof(EnumSOToggleButtonAttribute))
            {
                hasEnumSOToggleButton = true;
                break;
            }
            else if (attributeType == typeof(ValueToggleButtonAttribute))
            {
                alreadyHasValueToggleButton = true;
            }
        }

        if (!alreadyHasValueToggleButton && hasEnumSOToggleButton)
        {
            Type enumType = StaticUtilities.GetSublcassOf(typeof(A_EnumSO<,>), property.Info.TypeOfValue);
            if (enumType != null)
            {
                Type[] genericArguments = enumType.GenericTypeArguments;
                Type exactEnumType = genericArguments[0];
                ValueToggleButtonAttribute attribute = new ValueToggleButtonAttribute("@" + exactEnumType.Name + ".GetList()");
                attributes.Add(attribute);
            }
        }
    }
}
