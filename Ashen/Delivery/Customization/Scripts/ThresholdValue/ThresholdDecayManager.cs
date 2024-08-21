using UnityEngine;
using System.Collections;

namespace Ashen.DeliverySystem
{
    public class ThresholdDecayManager : I_ThresholdListener, I_Tickable, I_ThresholdDecayManager
    {
        private A_ThresholdValue thresholdValue;
        private I_DeliveryTool deliveryTool;
        private DerivedAttribute decayDelay;
        private DerivedAttribute decayRate;

        public TimeTicker decay;
        public TimeTicker decayWait;

        public ThresholdDecayManager(DerivedAttribute decayDelay, DerivedAttribute decayRate)
        {
            this.decayDelay = decayDelay;
            this.decayRate = decayRate;
        }

        public void Init(A_ThresholdValue value, I_DeliveryTool deliveryTool)
        {
            this.thresholdValue = value;
            this.deliveryTool = deliveryTool;
            value.RegiserThresholdChangeListener(this);
        }

        public void Reset(TimeTicker decay, TimeTicker decayWait)
        {
            if (this.decay != null)
            {
                this.decay.Disable();
            }
            if (this.decayWait != null)
            {
                this.decayWait.Disable();
            }
            this.decay = decay;
            this.decayWait = decayWait;
            if (decay != null)
            {
                decay.Enable(this);
            }
            else if (decayWait != null)
            {
                decayWait.Enable(this);
            }
        }

        public void OnThresholdEvent(ThresholdEventValue value)
        {
            if (decayRate == null)
            {
                Logger.ErrorLog("Decay rate was not set but ThresholdDecayManager is enabled");
                return;
            }

            if (!value.damageTaken)
            {
                // If threshold value has reached its base value, then stop all decaying processes
                if (value.currentValue == thresholdValue.GetBaseValue())
                {
                    if (decay != null)
                    {
                        decay.Disable();
                        decay = null;
                    }
                    if (decayWait != null)
                    {
                        decayWait.Disable();
                        decayWait = null;
                    }
                }
                // Damage was not taken, no point in continuing
                return;
            }

            // If there is a decay wait and the character is hit. Pause the decay until the decay rate finishes
            if (decayDelay != null)
            {
                if (value.previousValue == thresholdValue.GetBaseValue())
                {
                    decayWait = new TimeTicker(null, (int)decayDelay.Get(deliveryTool));
                    decayWait.Enable(this);
                }
                else
                {
                    if (decayWait != null)
                    {
                        decayWait.Reset();
                    }
                    if (decayWait == null)
                    {
                        decayWait = new TimeTicker(null, (int)decayDelay.Get(deliveryTool));
                        decayWait.Enable(this);
                    }
                    if (decay != null)
                    {
                        decay.Disable();
                        decay = null;
                    }
                }
            }
            else
            {
                // There is no delay, start recovering immediately if we're not already
                if (decay == null)
                {
                    decay = new TimeTicker(null, (int)decayRate.Get(deliveryTool));
                    decay.Enable(this);
                }
            }
        }

        public void Tick()
        { 
            if (decayWait != null)
            {
                // If decayWait ticks, the wait is over and the actua decay should start
                decayWait.Disable();
                decayWait = null;
                decay = new TimeTicker(null, (int)decayRate.Get(deliveryTool));
                decay.Enable(this);
            }
            else if (decay != null)
            {
                // Every time decay ticks remove one from the threshold value
                thresholdValue.RemoveAmount(1);
            }
        }

        public void End()
        {
            // Do nothing
        }

        public void UpdateTime()
        {
            // Do nothing
        }
    }
}