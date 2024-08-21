using Ashen.AbilitySystem;
using System;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ShiftableTierLevelTool : A_ShiftableCacheTool<ShiftableTierLevelTool, ShiftableTierLevelToolConfiguration, AbilityTag, int, int, int>
    {
        private DerivedAttribute abilityTierLevelLimit;
        private AttributeTool attributeTool;

        public override void Initialize()
        {
            base.Initialize();
            abilityTierLevelLimit = Config.AbilityTierLevelLimit;
            attributeTool = toolManager.Get<AttributeTool>();
        }

        public override A_Shiftable<int, int, int> GenerateShiftableValues()
        {
            IntShiftableValues values = new(GetEnumListSize());
            for (int x = 0; x < GetEnumListSize(); x++)
            {
                values.SetDefault(x, 0);
            }
            return values;
        }

        protected override IEnumerator<AbilityTag> GetEnumeratorInternal()
        {
            return AbilityTags.Instance.GetEnumerator();
        }

        protected override int GetEnumListSize()
        {
            return AbilityTags.Count;
        }

        public int CalculateTierLevel(List<AbilityTag> tags)
        {
            int tierLevel = 0;
            foreach (AbilityTag tag in tags)
            {
                tierLevel += Get(tag);
            }
            return LimitTierLevel(tierLevel);
        }

        public int LimitTierLevel(int tierLevel)
        {
            int limit = (int)attributeTool.Get(abilityTierLevelLimit);
            if (tierLevel < 0)
            {
                return Math.Max(tierLevel, -limit);
            }
            return Math.Min(tierLevel, limit);
        }
    }
}