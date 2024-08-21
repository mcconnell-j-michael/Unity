using Sirenix.OdinInspector;

namespace Ashen.SkillTree
{
    public struct PassiveContainer
    {
        public SkillNodeEffectBuilder builder;
        [ShowInInspector]
        private float?[] scaleDeliveryPacks;

        public float?[] ScaleDeliveryPacks
        {
            get
            {
                if (scaleDeliveryPacks == null)
                {
                    scaleDeliveryPacks = new float?[EffectFloatArguments.Count];
                }
                return scaleDeliveryPacks;
            }
        }
    }
}