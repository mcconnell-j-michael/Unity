using Ashen.DeliverySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

[HideLabel, BoxGroup("Ticker")]
[Serializable]
public class SkillTickerPack : I_TickerPack
{
    [FoldoutGroup("Frequency", expanded: true), OdinSerialize, HideLabel]
    private I_DeliveryValue frequency = default;

    public I_Ticker Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks extraArguments)
    {
        return new TimeTicker(null, (int)frequency.Build(owner, target, extraArguments), TimeRegistry.Instance.turnBased);
    }

    public SkillTickerPack(SerializationInfo info, StreamingContext context)
    {
        frequency = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(frequency));
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        StaticUtilities.SaveInterfaceValue(info, nameof(frequency), frequency);
    }
}