using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEngine.Pool;

namespace Ashen.DeliverySystem
{
    /**
     * The StatusEffect is an Effect that wraps a StatusEffectScriptableObject
     * and holds the TickerPack which defines how long the StatusEffect will last
     * and how often it will 'tick' (if applicable). 
     * A tick is a triggered effect i.e. DamageOverTime 'ticks' every time it applies damage
     **/
    [Serializable]
    public class StatusEffectPack : I_Effect, ISerializable
    {
        public StatusEffectScriptableObject Copy;
        public I_Ticker ticker;
        private ExtendedEffectArgumentFiller argumentFiller;
        private bool save;

        public StatusEffectPack(StatusEffectScriptableObject copy, I_Ticker ticker, ExtendedEffectArgumentFiller argumentFiller, bool save)
        {
            this.Copy = copy;
            this.ticker = ticker;
            this.argumentFiller = argumentFiller;
            this.save = save;
        }

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryResultPack targetResultPack, DeliveryArgumentPacks deliveryArguments)
        {
            Logger.DebugLog("Applying " + this.ToString());
            DeliveryArgumentPacks temp = GenericPool<DeliveryArgumentPacks>.Get();
            deliveryArguments.CopyInto(temp);
            argumentFiller.FillArguments(temp);
            ExtendedEffectArgumentsPack extendedArgumentPack = temp.GetPack<ExtendedEffectArgumentsPack>();
            extendedArgumentPack.SetTemp(!save);
            I_ExtendedEffect statusEffect = Copy.Clone(owner, target, temp);
            GenericPool<DeliveryArgumentPacks>.Release(temp);
            if (statusEffect == null)
            {
                //Status effect was cancelled due to tag operation
                return;
            }
            if (ticker != null)
            {
                statusEffect.SetTicker(ticker.Duplicate());
            }
            StatusEffectResult deliveryResult = targetResultPack.GetResult<StatusEffectResult>(DeliveryResultTypes.Instance.STATUS_EFFECT_RESULT_TYPE);
            deliveryResult.AppliedStatusEffects.Add(statusEffect);
            targetResultPack.empty = false;
        }

        public override string ToString()
        {
            return Copy.ToString();
        }

        public StatusEffectPack(SerializationInfo info, StreamingContext context)
        {
            Copy = StaticUtilities.LoadSOFromLibrary<ExtendedEffectLibrary, StatusEffectScriptableObject>(
                info,
                nameof(Copy)
            );
            ticker = StaticUtilities.LoadInterfaceValue<I_Ticker>(info, nameof(ticker));
            argumentFiller = (ExtendedEffectArgumentFiller)info.GetValue(nameof(argumentFiller), typeof(ExtendedEffectArgumentFiller));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StaticUtilities.SaveSOFromLibrary(info, nameof(Copy), Copy);
            StaticUtilities.SaveInterfaceValue(info, nameof(ticker), ticker);
            info.AddValue(nameof(argumentFiller), argumentFiller);
        }
    }
}
