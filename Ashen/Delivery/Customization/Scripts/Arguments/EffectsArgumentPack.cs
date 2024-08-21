using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class EffectsArgumentPack : A_DeliveryArgumentPack<EffectsArgumentPack>
    {
        private static readonly List<I_Effect> EMPTY_EFFECTS = new List<I_Effect>();
        private List<I_Effect> effects;
        private float[] effectScales;
        private bool[] enabledEffectScales;

        public EffectsArgumentPack()
        {
            effects = new List<I_Effect>();
            effectScales = new float[EffectFloatArguments.Count];
            enabledEffectScales = new bool[EffectFloatArguments.Count];
        }

        public float GetFloatScale(A_EffectFloatArgument argument)
        {
            if (!enabledEffectScales[(int)argument])
            {
                return 1f;
            }
            return effectScales[(int)argument];
        }

        public float GetFloatFlat(A_EffectFloatArgument argument)
        {
            if (!enabledEffectScales[(int)argument])
            {
                return 0f;
            }
            return effectScales[(int)argument];
        }

        public void SetFloatArgument(A_EffectFloatArgument argument, float value)
        {
            effectScales[(int)argument] = value;
            enabledEffectScales[(int)argument] = true;
        }

        public void CopyFloatArguments(EffectsArgumentPack other)
        {
            foreach (A_EffectFloatArgument arg in EffectFloatArguments.Instance)
            {
                if (other.enabledEffectScales[(int)arg])
                {
                    SetFloatArgument(arg, other.effectScales[(int)arg]);
                }
            }
        }

        public override I_DeliveryArgumentPack Initialize()
        {
            return new EffectsArgumentPack();
        }

        public List<I_Effect> GetEffects()
        {
            return effects;
        }

        public void AddEffect(I_Effect effect)
        {
            effects.Add(effect);
        }

        public void AddEffects(List<I_Effect> effects)
        {
            this.effects.AddRange(effects);
        }

        public override void Clear()
        {
            effects.Clear();
            foreach (A_EffectFloatArgument argument in EffectFloatArguments.Instance)
            {
                effectScales[(int)argument] = 0;
                enabledEffectScales[(int)argument] = false;
            }
        }

        public override void CopyInto(I_DeliveryArgumentPack pack)
        {
            EffectsArgumentPack eap = pack as EffectsArgumentPack;
            eap.effects.AddRange(effects);
            eap.CopyFloatArguments(this);

        }
    }
}