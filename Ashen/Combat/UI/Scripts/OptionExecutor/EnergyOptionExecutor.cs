using Ashen.DeliverySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using Ashen.UISystem;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class EnergyOptionExecutor : MonoBehaviour, I_OptionExecutor
    {
        public CombatOptionUI combatOption;

        public void Deselected(ToolManager source)
        {
            ResourceValueTool rvt = source.Get<ResourceValueTool>();
            rvt.ClearTempValues(ResourceValues.Instance.ACTION_POINT, ThresholdValueTempCategories.Instance.PREVIEW);
        }

        public I_GameState GetGameState(I_GameState parentState)
        {
            return new InfusionChooseOptionPanel(parentState);
        }

        public void InitializeOption(ToolManager source)
        {
            ResourceValueTool rvt = source.Get<ResourceValueTool>();
            int actionPoints = 1;
            ThresholdEventValue value = rvt.GetValue(ResourceValues.Instance.ACTION_POINT);
            int currentValue = rvt.CalculateLimit(ResourceValues.Instance.ACTION_POINT, value.tempValues[(int)ThresholdValueTempCategories.Instance.PROMISED]);
            combatOption.Valid = currentValue >= actionPoints;
        }

        public void Selected(ToolManager source)
        {
            if (!combatOption.Valid)
            {
                return;
            }
            ResourceValueTool rvt = source.Get<ResourceValueTool>();
            int actionPoints = 1;
            rvt.ApplyTempAmount(ResourceValues.Instance.ACTION_POINT, ThresholdValueTempCategories.Instance.PREVIEW, new TempValueContainer(actionPoints));
        }
    }
}