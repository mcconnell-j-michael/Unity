using Ashen.DeliverySystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
public class EffectBuilderEffect : I_Effect
{
    private I_EffectBuilder builder;

    public EffectBuilderEffect(I_EffectBuilder builder)
    {
        this.builder = builder;
    }

    public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
    {
        I_Effect effect = builder.Build(owner, target, deliveryArguments);
        effect.Apply(owner, target, targetDeliveryResult, deliveryArguments);
    }

    public EffectBuilderEffect(SerializationInfo info, StreamingContext context)
    {
        builder = StaticUtilities.LoadInterfaceValue<I_EffectBuilder>(info, nameof(builder));
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        StaticUtilities.SaveInterfaceValue(info, nameof(builder), builder);
    }
}
