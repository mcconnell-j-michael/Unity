using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ListEffectBuilder : I_EffectBuilder
    {
        [ListDrawerSettings(ShowFoldout = false)]
        public List<I_EffectBuilder> effects;

        public ListEffectBuilder()
        {
            effects = new List<I_EffectBuilder>();
        }

        public I_Effect Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            ListEffect listEffect = new ListEffect();
            List<I_Effect> newEffects = listEffect.effects;
            foreach (I_EffectBuilder effect in effects)
            {
                I_Effect resultEffect = effect.Build(owner, target, deliveryArguments);
                if (resultEffect != null)
                {
                    newEffects.Add(resultEffect);
                }
            }
            return listEffect;
        }

        public string visualize(int depth)
        {
            string vis = "";
            for (int x = 0; x < effects.Count; x++)
            {
                vis += effects[x].visualize(depth);
                if (x != effects.Count - 1)
                {
                    vis += "\n";
                }
            }
            return vis;
        }

        public ListEffectBuilder(SerializationInfo info, StreamingContext context)
        {
            effects = StaticUtilities.LoadList(info, nameof(effects), (string name) =>
            {
                return StaticUtilities.LoadInterfaceValue<I_EffectBuilder>(info, name);
            });
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveList(info, nameof(effects), effects, (string name, I_EffectBuilder builder) =>
            {
                StaticUtilities.SaveInterfaceValue(info, name, builder);
            });
        }
    }
}