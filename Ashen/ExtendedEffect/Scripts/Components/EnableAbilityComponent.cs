using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

[Serializable]
public class EnableAbilityComponent : A_SimpleComponent
{
    private AbilitySO ability;

    public EnableAbilityComponent(AbilitySO ability)
    {
        this.ability = ability;
    }

    public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
    {
        DeliveryTool deliveryTool = dse.target as DeliveryTool;
        AbilityHolder abilityHolder = deliveryTool.toolManager.Get<AbilityTool>().AbilityHolder;
        abilityHolder.GrantAbility(dse.key, ability.builder.Build());
    }

    public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
    {
        DeliveryTool deliveryTool = dse.target as DeliveryTool;
        AbilityHolder abilityHolder = deliveryTool.toolManager.Get<AbilityTool>().AbilityHolder;
        abilityHolder.RevokeAbility(dse.key);
    }


    public EnableAbilityComponent(SerializationInfo info, StreamingContext context)
    {
        ability = StaticUtilities.LoadSOFromLibrary<AbilitySOLibrary, AbilitySO>(info, nameof(ability));
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        StaticUtilities.SaveSOFromLibrary(info, nameof(ability), ability);
    }
}
