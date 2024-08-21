using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using Ashen.ObjectPoolSystem;
using Ashen.ToolSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionExecutable : I_ActionExecutable
{
    public DeliveryPackBuilder builder;
    public ToolManager source;
    public ToolManager target;
    public float?[] effectFloatArguments;

    public bool retargeted;

    private AbilityAction sourceAbility;
    public AbilityDeliveryPackProcessor DeliveryPackProcessor { get; private set; }
    public AbilityHitChanceProcessor HitChanceProcessor { get; private set; }
    public TargetingProcessor TargetingProcessor { get; private set; }
    private bool isFinished = false;
    private List<AbilityTag> tags;

    public ActionExecutable(AbilityAction abilityAction)
    {
        sourceAbility = abilityAction;
        DeliveryPackProcessor = abilityAction.Get<AbilityDeliveryPackProcessor>();
        HitChanceProcessor = abilityAction.Get<AbilityHitChanceProcessor>();
        TargetingProcessor = abilityAction.Get<TargetingProcessor>();
    }

    public IEnumerator Execute(MonoBehaviour runner)
    {
        DeliveryArgumentPacks deliveryArgumentPacks = AGenericPool<DeliveryArgumentPacks>.Get();
        DeliveryPackProcessor.FillDeliveryArguments(deliveryArgumentPacks);
        if (effectFloatArguments != null)
        {
            EffectsArgumentPack effectArgumentsPack = deliveryArgumentPacks.GetPack<EffectsArgumentPack>();
            foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
            {
                if (effectFloatArguments[(int)argument] != null)
                {
                    effectArgumentsPack.SetFloatArgument(argument, ((float)effectFloatArguments[(int)argument]));
                }
            }
        }

        DeliveryTool sDT = source.Get<DeliveryTool>();
        DeliveryTool tDT = target.Get<DeliveryTool>();

        bool hit = true;

        if (HitChanceProcessor != null && HitChanceProcessor.CanMiss())
        {
            float bonusHitChance = HitChanceProcessor.GetBonusHitChance(source, deliveryArgumentPacks);
            float hitChance = 70f + bonusHitChance;
            float roll = Random.Range(0f, 100f);
            hit = roll <= hitChance;
        }

        if (hit)
        {
            DeliveryContainer container = AGenericPool<DeliveryContainer>.Get();

            EquationArgumentPack equationArguments = deliveryArgumentPacks.GetPack<EquationArgumentPack>();
            equationArguments.SetAbilityTags(TargetingProcessor.GetAbilityTags(source));

            container.AddPrimaryEffect(builder.deliveryPack.Build(sDT, tDT, deliveryArgumentPacks));
            if (builder.preFilters != null)
            {
                container.AddPreFilter(new KeyContainer<I_Filter>()
                {
                    source = builder.preFilters.Build(sDT, tDT, deliveryArgumentPacks),
                });
            }
            if (builder.postFilters != null)
            {
                container.AddPostFilter(new KeyContainer<I_Filter>()
                {
                    source = builder.postFilters.Build(sDT, tDT, deliveryArgumentPacks),
                });
            }

            DeliveryUtility.Deliver(container, source.Get<DeliveryTool>(), target.Get<DeliveryTool>(), deliveryArgumentPacks);
            TriggerTool triggerTool = target.Get<TriggerTool>();
            foreach (AbilityTag tag in TargetingProcessor.GetAbilityTags(source))
            {
                if (tag.effectTrigger)
                {
                    triggerTool.Trigger(tag.effectTrigger);
                }
            }
            AGenericPool<DeliveryContainer>.Release(container);
        }
        else
        {
            DamageTool dTool = target.Get<DamageTool>();
            dTool.Report(new(), DamageHitType.Dodged);
        }

        AGenericPool<DeliveryArgumentPacks>.Release(deliveryArgumentPacks);

        isFinished = true;
        yield break;
    }

    public bool IsFinished()
    {
        return isFinished;
    }
}
