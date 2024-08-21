using System;
using System.Collections.Generic;

namespace Ashen.SkillTree
{
    [Serializable]
    public class ScaleDeliveryPack
    {
        [Hide]
        public Dictionary<EffectFloatArgument, float> scale;
    }
}