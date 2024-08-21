using Ashen.DeliverySystem;
using Ashen.EquationSystem;
using Ashen.ObjectPoolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ResourceValueTool : A_CacheableTool<ResourceValueTool, ResourceValueToolConfiguration, ResourceValue, float>, I_Saveable, I_ThresholdListener
    {
        private DamageTool damageTool;
        private A_ThresholdValue[] thresholdValues;

        private AbilityResourceConfig abilityResourceConfig;
        public AbilityResourceConfig AbilityResourceConfig
        {
            get
            {
                return abilityResourceConfig;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            thresholdValues = new A_ThresholdValue[ResourceValues.Count];
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                thresholdValues[(int)resourceValue] = resourceValue.threshold.BuildThresholdValue(toolManager, resourceValue);
            }
            abilityResourceConfig = Config.AbilityResourceConfig;
        }

        private void Start()
        {
            damageTool = toolManager.Get<DamageTool>();
            if (damageTool)
            {
                foreach (ResourceValue resourceValue in ResourceValues.Instance)
                {
                    foreach (DamageType damageType in resourceValue.listenOn)
                    {
                        damageTool.RegisterListener(damageType, thresholdValues[(int)resourceValue]);
                    }

                }
            }
            foreach (A_ThresholdValue value in thresholdValues)
            {
                value.Init(toolManager);
            }
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                thresholdValues[(int)resourceValue].RegiserThresholdChangeListener(this);
            }
        }

        protected override int GetEnumListSize()
        {
            return ResourceValues.Count;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (damageTool)
            {
                foreach (ResourceValue resourceValue in ResourceValues.Instance)
                {
                    foreach (DamageType damageType in resourceValue.listenOn)
                    {
                        damageTool.UnRegisterListener(damageType, thresholdValues[(int)resourceValue]);
                    }
                }
            }
            foreach (ResourceValue resourceValue in ResourceValues.Instance)
            {
                thresholdValues[(int)resourceValue].UnRegesterThresholdChangeListener(this);
            }
        }

        public void ApplyAmount(ResourceValue resourceValue, int total)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.ApplyAmount(total);
        }

        public void RemoveAmount(ResourceValue resourceValue, int total)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.RemoveAmount(total);
        }

        public void SetAmount(ResourceValue resourceValue, int total)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.SetAmount(total);
        }

        public void ApplyTempAmount(ResourceValue resourceValue, ThresholdValueTempCategory category, TempValueContainer tempValue)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.ApplyTempAmount(category, tempValue);
        }

        public void RemoveTempAmount(ResourceValue resourceValue, ThresholdValueTempCategory category, TempValueContainer tempValue)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.RemoveTempAmount(category, tempValue);
        }

        public void ClearTempValues(ResourceValue resourceValue)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.ClearTempValues();
        }

        public void ClearTempValues(ResourceValue resourceValue, ThresholdValueTempCategory category)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.ClearTempValues(category);
        }

        public void DeleteTempValue(ResourceValue resourceValue, ThresholdValueTempCategory category, TempValueContainer tempValue)
        {
            A_ThresholdValue value = thresholdValues[(int)resourceValue];
            value.DeleteTempValue(category, tempValue);
        }

        public override float Get(ResourceValue enumSO, DeliveryArgumentPacks equationArguments)
        {
            ThresholdEventValue value = GetValue(enumSO);
            return value.currentValue;
        }

        public ThresholdEventValue GetValue(ResourceValue resourceValue)
        {
            return thresholdValues[(int)resourceValue].GetCurrentState();
        }

        public int CalculateLimit(ResourceValue resourceValue, int tempValue)
        {
            return thresholdValues[(int)resourceValue].CalculateValueWithLimit(tempValue);
        }

        public void RegisterThresholdMetListener(ResourceValue resourceValue, I_ThresholdListener listener)
        {
            thresholdValues[(int)resourceValue].RegisterThresholdMetListener(listener);
        }

        public void UnRegesterThresholdMetListener(ResourceValue resourceValue, I_ThresholdListener listener)
        {
            thresholdValues[(int)resourceValue].UnRegesterThresholdMetListener(listener);
        }

        public void RegiserThresholdChangeListener(ResourceValue resourceValue, I_ThresholdListener listener)
        {
            thresholdValues[(int)resourceValue].RegiserThresholdChangeListener(listener);
            DeliveryArgumentPacks packs = AGenericPool<DeliveryArgumentPacks>.Get();
            thresholdValues[(int)resourceValue].Recalculate(toolManager.Get<DeliveryTool>(), packs.GetPack<EquationArgumentPack>());
            AGenericPool<DeliveryArgumentPacks>.Release(packs);
        }

        public void UnRegesterThresholdChangeListener(ResourceValue resourceValue, I_ThresholdListener listener)
        {
            thresholdValues[(int)resourceValue].UnRegesterThresholdChangeListener(listener);
        }

        public float GetCurrentPercentage(ResourceValue resourceValue)
        {
            return thresholdValues[(int)resourceValue].Percentage();
        }

        public void DisableThresholdValue(ResourceValue resourceValue)
        {
            thresholdValues[(int)resourceValue].Disable();
        }

        public void EnableThresholdValue(ResourceValue resourceValue)
        {
            thresholdValues[(int)resourceValue].Enable();
        }

        public void ClearThresholdValue(ResourceValue resourceValue)
        {
            thresholdValues[(int)resourceValue].ClearDamage();
        }

        public void OnThresholdEvent(ThresholdEventValue value)
        {
            OnChange(value.resourceValue);
        }

        public object CaptureState()
        {
            ThresholdValueSaveData[] thresholdDatas = new ThresholdValueSaveData[thresholdValues.Length];
            ThresholdValueDecayManagerSaveData[] decayManagerDatas = new ThresholdValueDecayManagerSaveData[thresholdValues.Length];
            for (int x = 0; x < thresholdValues.Length; x++)
            {
                A_ThresholdValue value = thresholdValues[x];
                thresholdDatas[x] = new ThresholdValueSaveData
                {
                    currentValue = value.currentValue,
                    enabled = value.enabled,
                };
                I_ThresholdDecayManager manager = value.decayManager;
                if (manager is ThresholdDecayManager decayManager)
                {
                    decayManagerDatas[x] = new ThresholdValueDecayManagerSaveData
                    {
                        decay = decayManager.decay,
                        decayWait = decayManager.decayWait,
                    };
                }
                else
                {
                    decayManagerDatas[x] = new ThresholdValueDecayManagerSaveData { };
                }
            }

            return new ResourceValueSaveData
            {
                thresholdValues = thresholdDatas,
                decayManagerValues = decayManagerDatas,
            };
        }

        public void RestoreState(object state)
        {
            ResourceValueSaveData saveData = (ResourceValueSaveData)state;
            for (int x = 0; x < thresholdValues.Length; x++)
            {
                ThresholdValueSaveData thresholdValue = saveData.thresholdValues[x];
                ThresholdValueDecayManagerSaveData managerValue = saveData.decayManagerValues[x];
                A_ThresholdValue value = thresholdValues[x];
                value.Reset(thresholdValue.currentValue, thresholdValue.enabled);
                if (value.decayManager is ThresholdDecayManager manager)
                {
                    manager.Reset(managerValue.decay, managerValue.decayWait);
                }
            }
        }

        public void PrepareRestoreState()
        { }

        [Serializable]
        public struct ResourceValueSaveData
        {
            public ThresholdValueSaveData[] thresholdValues;
            public ThresholdValueDecayManagerSaveData[] decayManagerValues;
        }

        [Serializable]
        public struct ThresholdValueSaveData
        {
            public int currentValue;
            public bool enabled;
        }

        [Serializable]
        public struct ThresholdValueDecayManagerSaveData
        {
            public TimeTicker decay;
            public TimeTicker decayWait;
        }

        [ShowInInspector]
        private Dictionary<ResourceValue, float> resourceValues;
        [Button]
        private void CalculateResourceValues()
        {
            resourceValues = new Dictionary<ResourceValue, float>();
            foreach (ResourceValue value in ResourceValues.Instance)
            {
                resourceValues.Add(value, this.GetValue(value).currentValue);
            }
        }
        [Button]
        private void TestApplyAmount(ResourceValue rv, int value)
        {
            if (value > 0)
            {
                ApplyAmount(rv, value);
            }
            else
            {
                ApplyAmount(rv, -value);
            }
        }
    }
}