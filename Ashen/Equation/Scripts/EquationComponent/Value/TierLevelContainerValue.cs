using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.EquationSystem
{
    public class TierLevelContainerValue : A_Value
    {
        public bool useTarget;

        private List<AbilityTag> tags;
        private List<TierLevelValue> tierLevels;

        public override bool Cache(I_DeliveryTool source, Equation equation)
        {
            if (IsCachable())
            {
                if (source == null)
                {
                    return false;
                }
                ToolManager toolManager = (source as DeliveryTool).toolManager;
                ShiftableTierLevelTool stlTool = toolManager.Get<ShiftableTierLevelTool>();
                if (stlTool == null)
                {
                    return false;
                }
                foreach (TierLevelValue tierLevel in tierLevels)
                {
                    tierLevel.Cache(source, equation);
                }
            }
            return false;
        }

        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            I_DeliveryTool deliveryTool = useTarget ? target : source;
            ShiftableTierLevelTool stlTool = (deliveryTool as DeliveryTool).toolManager.Get<ShiftableTierLevelTool>();
            if (!stlTool)
            {
                return 0;
            }
            EquationArgumentPack equationArguments = extraArguments.GetPack<EquationArgumentPack>();
            tags = new List<AbilityTag>();
            tierLevels = new List<TierLevelValue>();
            tags.AddRange(equationArguments.GetAbilityTags());
            int tierLevel = 0;
            foreach (AbilityTag tag in tags)
            {
                tierLevel += stlTool.Get(tag);
                tierLevels.Add(new TierLevelValue()
                {
                    enumSO = tag,
                    useTarget = useTarget,
                });
            }
            return stlTool.LimitTierLevel(tierLevel);
        }

        public override string Representation()
        {
            return "TierLevel";
        }

        public override bool RequiresCaching()
        {
            return true;
        }

        public override bool IsCachable()
        {
            return !useTarget;
        }
    }
}