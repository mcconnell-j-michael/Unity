using Ashen.DeliverySystem;
using System;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    /**
     * This MonoBehaviour will manage applying damage to a character
     **/
    public class DamageTool : A_ConfigurableTool<DamageTool, DamageToolConfiguration>
    {
        ResistanceTool resistanceTool;

        private List<I_DamageListener>[] damageTypeListeners;
        private List<I_DamageListener> listeners;

        public override void Initialize()
        {
            base.Initialize();
            listeners = new List<I_DamageListener>();
            damageTypeListeners = new List<I_DamageListener>[DamageTypes.Count];
            for (int x = 0; x < DamageTypes.Count; x++)
            {
                damageTypeListeners[x] = new List<I_DamageListener>();
            }
        }

        private void Start()
        {
            resistanceTool = toolManager.Get<ResistanceTool>();
        }

        public void RegisterListener(DamageType damageType, I_DamageListener listener)
        {
            damageTypeListeners[(int)damageType].Add(listener);
        }

        public void RegisterListener(I_DamageListener listener)
        {
            listeners.Add(listener);
        }

        public void UnRegisterListener(DamageType damageType, I_DamageListener listener)
        {
            damageTypeListeners[(int)damageType].Remove(listener);
        }

        public void UnRegisterListener(I_DamageListener listener)
        {
            listeners.Remove(listener);
        }

        public void Report(List<Tuple<DamageType, int>> damages, DamageHitType hitType)
        {
            Tuple<DamageType, int>[] damageArray = new Tuple<DamageType, int>[DamageTypes.Count];
            foreach (Tuple<DamageType, int> damage in damages)
            {
                damageArray[(int)damage.Item1] = damage;
                if (damageTypeListeners.Length == 0)
                {
                    continue;
                }
                int damageNum = (int)damage.Item1;
                Tuple<DamageType, int>[] individualDamage = new Tuple<DamageType, int>[DamageTypes.Count];
                individualDamage[(int)damage.Item1] = damage;
                DamageEvent individualDamageEvent = new DamageEvent
                {
                    damages = individualDamage,
                    hitType = hitType
                };
                for (int x = 0; x < damageTypeListeners[damageNum].Count; x++)
                {
                    I_DamageListener listener = damageTypeListeners[damageNum][x];
                    listener.OnDamageEvent(individualDamageEvent);
                }
            }
            DamageEvent damageEvent = new DamageEvent
            {
                damages = damageArray,
                hitType = hitType
            };
            for (int x = 0; x < listeners.Count; x++)
            {
                I_DamageListener listener = listeners[x];
                listener.OnDamageEvent(damageEvent);
            }
        }

        public void TakeDamage(List<Tuple<DamageType, int>> damages)
        {
            Report(damages, DamageHitType.Normal);
        }

        public int GetDamage(DamageType damageType, int damage, DeliveryArgumentPacks deliveryArguments)
        {
            float resistanceMult = resistanceTool.GetResistancePercentage(damageType);
            int finalDamage = (int)Math.Round(damage * resistanceMult, 0);
            return finalDamage;
        }
    }
}
