using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    public class RetargetEffect : I_Effect, ISerializable
    {
        private AbilityTag[] abilityTags;

        public RetargetEffect() { }

        public RetargetEffect(AbilityTag[] abilityTags)
        {
            this.abilityTags = abilityTags;
        }

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetDeliveryResult, DeliveryArgumentPacks deliveryArguments)
        {
            ToolManager targetTM = (target as DeliveryTool).toolManager;
            if (owner != target && ExecuteInputState.Instance.currentSubactionProcessor != null)
            {
                SubactionProcessor processor = ExecuteInputState.Instance.currentSubactionProcessor;
                if (targetTM != processor.actionExecutable.target)
                {
                    return;
                }
                List<AbilityTag> actionAbilityTags = processor.actionExecutable.TargetingProcessor.GetAbilityTags(processor.actionExecutable.source);
                foreach (AbilityTag abilityTag in abilityTags)
                {
                    if (actionAbilityTags.Contains(abilityTag))
                    {
                        processor.actionExecutable.target = (owner as DeliveryTool).toolManager;
                        PlayerPartyHolder.Instance.partyManager.GetCurrentBattleContainer().AddProcesor(CombatProcessorTypes.Instance.SUPPORTING_ACTION, new CombatLogProcessor()
                        {
                            message = (owner as DeliveryTool).toolManager.gameObject.name + " defended " + targetTM.gameObject.name + "!",
                        });
                        break;
                    }
                }
            }
        }

        protected RetargetEffect(SerializationInfo info, StreamingContext context)
        {
            int length = info.GetInt32(nameof(abilityTags) + "-Count");
            abilityTags = new AbilityTag[length];
            for (int x = 0; x < length; x++)
            {
                abilityTags[x] = AbilityTags.Instance[info.GetInt32(nameof(abilityTags) + "-" + x)];
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(abilityTags) + "-Count", abilityTags.Length);
            for (int x = 0; x < abilityTags.Length; x++)
            {
                info.AddValue(nameof(abilityTags) + "-" + x, (int)abilityTags[x]);
            }
        }
    }
}
