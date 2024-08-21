using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InfoCanvasToolConfiguration", menuName = "Custom/Tool/InfoCanvasToolConfiguration")]
public class InfoCanvasToolConfiguration : A_Configuration<InfoCanvasTool, InfoCanvasToolConfiguration>
{
    [OdinSerialize]
    private Dictionary<DamageType, BarInfo> barConfigurations = default;

    [OdinSerialize]
    private bool useBarInitialValue = default;
    [OdinSerialize, ShowIf("useBarInitialValue")]
    private float barInitialValue = default;
    public float BarInitialValue
    {
        get
        {
            if (useBarInitialValue)
            {
                return barInitialValue;
            }
            return GetDefault().barInitialValue;
        }
    }

    [OdinSerialize]
    private bool useBarGrowthValue = default;

    [OdinSerialize, ShowIf("useBarGrowthValue")]
    private float barGrowthValue = default;
    public float BarGrowthValue
    {
        get
        {
            if (useBarGrowthValue)
            {
                return barGrowthValue;
            }
            return GetDefault().barGrowthValue;
        }
    }

    public BarInfo[] DerivedBarConfigurations
    {
        get
        {
            if (barConfigurations == null)
            {
                return GetDefault().DerivedBarConfigurations;
            }
            else
            {
                BarInfo[] derivedBarConfigurations = new BarInfo[DamageTypes.Count];
                foreach (DamageType damageType in DamageTypes.Instance)
                {
                    if (barConfigurations.ContainsKey(damageType))
                    {
                        derivedBarConfigurations[(int)damageType] = barConfigurations[damageType];
                    }
                    else
                    {
                        if (this != GetDefault())
                        {
                            derivedBarConfigurations[(int)damageType] = GetDefault().barConfigurations[damageType];
                        }
                    }
                }
                return derivedBarConfigurations;
            }
        }
    }

    [OdinSerialize]
    private List<ResourceValue> defaultResourceValues = default;
    public List<ResourceValue> DefaultResourceValues
    {
        get
        {
            if (GetDefault() == this)
            {
                return defaultResourceValues;
            }
            return GetDefault().defaultResourceValues;
        }
    }

    [OdinSerialize]
    private bool useSymbolInitialValue = default;
    [OdinSerialize, ShowIf("useSymbolInitialValue")]
    private float symbolInitialValue = default;
    public float SymbolInitialValue
    {
        get
        {
            if (useSymbolInitialValue)
            {
                return symbolInitialValue;
            }
            return GetDefault().symbolInitialValue;
        }
    }

    [OdinSerialize]
    private bool useSymbolGrowthValue = default;

    [OdinSerialize, ShowIf("useSymbolGrowthValue")]
    private float symbolGrowthValue = default;
    public float SymbolGrowthValue
    {
        get
        {
            if (useSymbolGrowthValue)
            {
                return symbolGrowthValue;
            }
            return GetDefault().symbolGrowthValue;
        }
    }
}