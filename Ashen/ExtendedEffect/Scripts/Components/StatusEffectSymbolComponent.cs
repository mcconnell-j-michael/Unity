using UnityEngine;
using Ashen.ToolSystem;
using System;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class StatusEffectSymbolComponent : A_SimpleComponent
    {
        private Sprite sprite;

        private StatusSymbolRegistery symbol;

        public StatusEffectSymbolComponent(Sprite sprite)
        {
            this.sprite = sprite;
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            StatusTool st = deliveryTool.toolManager.Get<StatusTool>();
            symbol = st.AddActiveSymbol(sprite, "");
        }

        public override void Trigger(ExtendedEffect dse, ExtendedEffectTrigger statusTrigger, ExtendedEffectContainer container)
        {
            if (statusTrigger == ExtendedEffectTriggers.Instance.UpdateTick)
            {
                float? originalDuration = dse.Ticker.TotalDuration();
                float? timeLeft = dse.Ticker.TimeLeft();

                if (originalDuration != null && timeLeft != null)
                {
                    DeliveryTool deliveryTool = dse.target as DeliveryTool;
                    StatusTool st = deliveryTool.toolManager.Get<StatusTool>();
                    float percentage = (float)timeLeft / (float)originalDuration;
                    st.UpdateActiveSymbol(symbol.id, percentage);
                }
            }
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            StatusTool st = deliveryTool.toolManager.Get<StatusTool>();
            st.RemoveActiveSymbol(symbol.id);
        }

        private new static ExtendedEffectTrigger[] statusTriggers = new ExtendedEffectTrigger[] { ExtendedEffectTriggers.Instance.UpdateTick };
        public override ExtendedEffectTrigger[] GetStatusTriggers()
        {
            return statusTriggers;
        }
    }
}