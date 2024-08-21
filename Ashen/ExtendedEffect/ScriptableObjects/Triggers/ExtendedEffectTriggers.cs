using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * See StatusTrigger
     **/
    [CreateAssetMenu(fileName = nameof(ExtendedEffectTriggers), menuName = "Custom/Enums/" + nameof(ExtendedEffectTriggers) + "/Types")]
    public class ExtendedEffectTriggers : A_EnumList<ExtendedEffectTrigger, ExtendedEffectTriggers>
    {
        public ExtendedEffectTrigger Tick;

        public ExtendedEffectTrigger UpdateTick;
        public ExtendedEffectTrigger TurnStart;
        public ExtendedEffectTrigger TurnEnd;
        public ExtendedEffectTrigger ActionStart;
        public ExtendedEffectTrigger ActionEnd;
        public ExtendedEffectTrigger Targeted;
        public ExtendedEffectTrigger Effected;
        public ExtendedEffectTrigger BuffRecieved;
        public ExtendedEffectTrigger BattleStart;
        public ExtendedEffectTrigger BattleEnd;
    }
}