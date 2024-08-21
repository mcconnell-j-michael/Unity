using Ashen.DeliverySystem;
using Ashen.ObjectPoolSystem;
using Ashen.PartySystem;
using Ashen.ToolSystem;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.StateMachineSystem
{
    public class EnergyCleanUpState : SingletonScriptableObject<EnergyCleanUpState>, I_GameState
    {
        [SerializeField]
        private List<EnergyCleanUpInformation> energies;
        [OdinSerialize]
        private I_DeliveryValue energyBurn;

        public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
        {
            A_PartyManager playerParty = PlayerPartyHolder.Instance.partyManager;
            A_PartyManager enemyParty = EnemyPartyHolder.Instance.enemyPartyManager;

            foreach (PartyPosition position in playerParty.GetActivePositions())
            {
                ToolManager toolManager = playerParty.GetToolManager(position);
                DeliverForToolManager(toolManager);
            }
            foreach (PartyPosition position in enemyParty.GetActivePositions())
            {
                ToolManager toolManager = enemyParty.GetToolManager(position);
                DeliverForToolManager(toolManager);
            }

            yield return new ExecuteProcessors(ExecuteInputState.Instance.battleContainer, CombatProcessorTypes.Instance.PRIMARY_ACTION).RunState(request, response);

            response.nextState = EndRoundState.Instance;

            yield break;
        }

        private void DeliverForToolManager(ToolManager toolManager)
        {
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            AttributeTool attTool = toolManager.Get<AttributeTool>();
            DeliveryTool dTool = toolManager.Get<DeliveryTool>();
            foreach (EnergyCleanUpInformation cleanUpInformation in energies)
            {
                ThresholdEventValue value = rvTool.GetValue(cleanUpInformation.Value);
                float midWay = attTool.GetAttribute(cleanUpInformation.MidwayValue);
                int diff = Mathf.RoundToInt(value.currentValue - midWay);
                if (diff <= 0)
                {
                    continue;
                }
                DeliveryArgumentPacks deliveryArgumentPacks = AGenericPool<DeliveryArgumentPacks>.Get();
                EffectsArgumentPack effArgs = deliveryArgumentPacks.GetPack<EffectsArgumentPack>();
                effArgs.SetFloatArgument(EffectFloatArguments.Instance.reservedDamageScale, diff);

                DeliveryContainer container = AGenericPool<DeliveryContainer>.Get();

                container.AddPrimaryEffect(new DamagePackBuilder(cleanUpInformation.EnergyDamageType, energyBurn).Build(dTool, dTool, deliveryArgumentPacks));
                rvTool.RemoveAmount(cleanUpInformation.Value, diff);

                DeliveryUtility.Deliver(container, dTool, dTool, deliveryArgumentPacks);
                AGenericPool<DeliveryContainer>.Release(container);
                AGenericPool<DeliveryArgumentPacks>.Release(deliveryArgumentPacks);
            }
        }
    }
}