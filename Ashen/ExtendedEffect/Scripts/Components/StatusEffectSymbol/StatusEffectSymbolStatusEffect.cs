using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class StatusEffectSymbolStatusEffect : A_SimpleComponent
    {
        [OdinSerialize, Hide, Title("Status Effect Color")]
        private StatusEffectSymbol statusEffectSymbol;

        private List<I_StatusEffectSymbolListener> listeners;

        public StatusEffectSymbolStatusEffect()
        {
            listeners = new List<I_StatusEffectSymbolListener>();
        }

        public StatusEffectSymbolStatusEffect(StatusEffectSymbol statusEffectSymbol)
        {
            this.statusEffectSymbol = statusEffectSymbol;
            listeners = new List<I_StatusEffectSymbolListener>();
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool deliveryTool = dse.target as DeliveryTool;
            InfoCanvasTool canvasTool = deliveryTool.toolManager.Get<InfoCanvasTool>();
            if (canvasTool)
            {
                StatusEffectSymbolsManager symbolsManager = canvasTool.statusEffectSymbolsManager;
                listeners.Add(symbolsManager.CreateStatusEffectSymbol(statusEffectSymbol));
            }
        }

        public override void Trigger(ExtendedEffect dse, ExtendedEffectTrigger statusTrigger, ExtendedEffectContainer container)
        {
            if (statusTrigger == ExtendedEffectTriggers.Instance.UpdateTick)
            {
                float? originalDuration = dse.Ticker.TotalDuration();
                float? timeLeft = dse.Ticker.TimeLeft();

                if (originalDuration != null && timeLeft != null)
                {
                    float percentage = (float)timeLeft / (float)originalDuration;
                    Report(percentage);
                }
            }
        }

        private new static ExtendedEffectTrigger[] statusTriggers = new ExtendedEffectTrigger[] { ExtendedEffectTriggers.Instance.UpdateTick };
        public override ExtendedEffectTrigger[] GetStatusTriggers()
        {
            if (statusTriggers == null)
            {
                statusTriggers = new ExtendedEffectTrigger[0];
            }
            return statusTriggers;
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            ReportEnd();
        }

        private void Report(float percentage)
        {
            StatusEffectSymbolEventValue value = new StatusEffectSymbolEventValue
            {
                percentage = percentage
            };
            foreach (I_StatusEffectSymbolListener listener in listeners)
            {
                listener.OnStatusEffectSymbolEvent(value);
            }
        }

        private void ReportEnd()
        {
            StatusEffectSymbolEventValue value = new StatusEffectSymbolEventValue
            {
                percentage = -1,
                ended = true
            };
            foreach (I_StatusEffectSymbolListener listener in listeners)
            {
                listener.OnStatusEffectSymbolEvent(value);
            }
        }

        //public override I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        //{
        //    return new StatusEffectSymbolStatusEffect(statusEffectSymbol);
        //}
    }
}