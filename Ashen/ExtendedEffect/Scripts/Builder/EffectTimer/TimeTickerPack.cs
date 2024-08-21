using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    /**
     * A TimeTickerPack knows how to create an instance of 
     * a TimeTicker. The difference between the two, is that
     * a status effect will have a TimeTickerPack, and whenever
     * the status is applied, that time ticker pack will build
     * a Ticker that will manage the StatusEffect for the individual
     * character it is applied to. I.E. if the same Status effect
     * is applied to 5 characters, then there are 5 Tickers that 
     * are created.
     **/
    [Serializable]
    public class TimeTickerPack : I_TickerPack
    {
        [HorizontalGroup("TimeTicker"), Title("Duration"), OdinSerialize, HideLabel]
        private I_DeliveryValue duration;
        [HorizontalGroup("TimeTicker"), Title("Frequency"), OdinSerialize, HideLabel]
        private I_DeliveryValue frequency;

        public TimeTickerPack() { }
        public TimeTickerPack(I_DeliveryValue duration, I_DeliveryValue frequency)
        {
            this.duration = duration;
            this.frequency = frequency;
        }

        public I_Ticker Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            int? calculatedDuration = null;
            int? calculatedFrequency = null;

            if (duration != null)
            {
                calculatedDuration = (int)duration.Build(owner, target, deliveryArguments);
            }

            if (frequency != null)
            {
                calculatedFrequency = (int)frequency.Build(owner, target, deliveryArguments);
            }

            return new TimeTicker(calculatedDuration, calculatedFrequency, TimeRegistry.Instance.turnBased);
        }

        public TimeTickerPack(SerializationInfo info, StreamingContext context)
        {
            duration = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(duration));
            frequency = StaticUtilities.LoadInterfaceValue<I_DeliveryValue>(info, nameof(frequency));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveInterfaceValue(info, nameof(duration), duration);
            StaticUtilities.SaveInterfaceValue(info, nameof(frequency), frequency);
        }
    }
}
