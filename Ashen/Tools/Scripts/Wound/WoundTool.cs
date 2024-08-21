using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.ObjectPoolSystem;
using Ashen.WoundSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class WoundTool : A_ConfigurableTool<WoundTool, WoundToolConfiguration>, I_Saveable, I_EnumCacheable
    {
        [ShowInInspector]
        private List<WoundScriptableObject> enabledWounds;
        private List<WoundScriptableObject>[] availableWoundsPerCategory;
        private List<int>[] availableIndexes;
        private List<I_ExtendedEffect>[] activeWoundEffects;

        private DerivedAttribute[] woundResistancePerCategory;
        private EffectFloatArgument woundScale;
        private ResourceValue woundResource;

        private DeliveryTool deliveryTool;
        private ResourceValueTool resourceValueTool;

        public override void Initialize()
        {
            base.Initialize();
            enabledWounds = new List<WoundScriptableObject>();
            availableWoundsPerCategory = new List<WoundScriptableObject>[WoundCategories.Count];
            availableIndexes = new List<int>[WoundCategories.Count];
            activeWoundEffects = new List<I_ExtendedEffect>[WoundCategories.Count];
            for (int x = 0; x < WoundCategories.Count; x++)
            {
                availableWoundsPerCategory[x] = new List<WoundScriptableObject>();
                availableIndexes[x] = new List<int>();
                activeWoundEffects[x] = new List<I_ExtendedEffect>();
            }
            List<WoundScriptableObject> wounds = Config.AvailableWounds;
            foreach (WoundScriptableObject wound in wounds)
            {
                int index = (int)wound.woundCategory;
                availableWoundsPerCategory[index].Add(wound);
                availableIndexes[index].Add(availableWoundsPerCategory[index].Count - 1);
                activeWoundEffects[index].Add(null);
            }
            woundResistancePerCategory = new DerivedAttribute[WoundCategories.Count];
            foreach (WoundCategory category in WoundCategories.Instance)
            {
                woundResistancePerCategory[(int)category] = Config.GetWoundResistance(category);
            }
            woundResource = Config.WoundResource;
            woundScale = Config.WoundScale;
        }

        private void Start()
        {
            deliveryTool = toolManager.Get<DeliveryTool>();
            resourceValueTool = toolManager.Get<ResourceValueTool>();
            AttributeTool attributeTool = toolManager.Get<AttributeTool>();
            foreach (DerivedAttribute attr in woundResistancePerCategory)
            {
                attributeTool.Cache(attr, this);
            }
        }

        public WoundScriptableObject ApplyRandomWound()
        {
            List<WoundCategory> availableCategories = new();
            foreach (WoundCategory category in WoundCategories.Instance)
            {
                if (availableIndexes[(int)category].Count > 0)
                {
                    availableCategories.Add(category);
                }
            }
            int categoryInRange = UnityEngine.Random.Range(0, availableCategories.Count);
            return ApplyRandomWound(availableCategories[categoryInRange]);
        }

        public WoundScriptableObject ApplyRandomWound(WoundCategory category)
        {
            List<int> woundIndexes = availableIndexes[(int)category];
            int numInRange = UnityEngine.Random.Range(0, woundIndexes.Count);
            int woundIndex = woundIndexes[numInRange];
            WoundScriptableObject woundSO = availableWoundsPerCategory[(int)category][woundIndex];
            ApplyWound(woundSO);
            return woundSO;
        }

        public void ApplyWound(WoundScriptableObject woundSO)
        {
            int categoryIndex = (int)woundSO.woundCategory;
            int woundIndex = availableWoundsPerCategory[categoryIndex].IndexOf(woundSO);
            if (activeWoundEffects[categoryIndex][woundIndex] != null)
            {
                return;
            }
            List<int> woundIndexes = availableIndexes[categoryIndex];
            woundIndexes.Remove(woundIndex);

            DeliveryArgumentPacks packs = AGenericPool<DeliveryArgumentPacks>.Get();
            EffectsArgumentPack effPacks = packs.GetPack<EffectsArgumentPack>();
            AttributeTool attTool = toolManager.Get<AttributeTool>();
            effPacks.SetFloatArgument(woundScale, attTool.Get(woundResistancePerCategory[(int)woundSO.woundCategory]));

            I_ExtendedEffect effect = woundSO.woundBuilder.Build(deliveryTool, deliveryTool, packs);
            effect.Enable();
            activeWoundEffects[categoryIndex][woundIndex] = effect;
            enabledWounds.Add(woundSO);
            resourceValueTool.SetAmount(woundResource, enabledWounds.Count);
        }

        public void RemoveWound(WoundScriptableObject woundSO)
        {
            int categoryIndex = (int)woundSO.woundCategory;
            int woundIndex = availableWoundsPerCategory[categoryIndex].IndexOf(woundSO);
            I_ExtendedEffect woundEffect = activeWoundEffects[categoryIndex][woundIndex];
            if (woundIndex < 0 || woundEffect == null)
            {
                return;
            }
            availableIndexes[categoryIndex].Add(woundIndex);
            activeWoundEffects[categoryIndex][woundIndex] = null;
            woundEffect.Disable(false);
            enabledWounds.Remove(woundSO);
            resourceValueTool.SetAmount(woundResource, enabledWounds.Count);
        }

        public void RemoveAllWounds()
        {
            while (enabledWounds.Count > 0)
            {
                RemoveWound(enabledWounds[0]);
            }
        }

        public object CaptureState()
        {
            List<string> woundKeys = new();
            foreach (WoundScriptableObject woundSO in enabledWounds)
            {
                woundKeys.Add(woundSO.name);
            }
            return new WoundSaveData
            {
                enabledWoundKeys = woundKeys
            };
        }

        public void RestoreState(object state)
        {
            WoundSaveData woundSaveData = (WoundSaveData)state;
            WoundLibrary woundLibrary = WoundLibrary.Instance;
            foreach (string wound in woundSaveData.enabledWoundKeys)
            {
                WoundScriptableObject woundSO = woundLibrary.GetScriptableObject(wound);
                ApplyWound(woundSO);
            }
        }

        public void PrepareRestoreState()
        {
            RemoveAllWounds();
        }

        public void Recalculate(I_EnumSO enumValue, I_DeliveryTool deliveryTool)
        {
            if (enumValue is DerivedAttribute attribute)
            {
                List<WoundScriptableObject> wounds = new();
                wounds.AddRange(enabledWounds);
                bool[] woundCategories = new bool[WoundCategories.Count];
                for (int x = 0; x < woundResistancePerCategory.Length; x++)
                {
                    if (attribute == woundResistancePerCategory[x])
                    {
                        woundCategories[x] = true;
                    }
                }
                foreach (WoundScriptableObject wound in wounds)
                {
                    if (woundCategories[(int)wound.woundCategory])
                    {
                        RemoveWound(wound);
                        ApplyWound(wound);
                    }
                }
            }
        }

        [Serializable]
        private struct WoundSaveData
        {
            public List<string> enabledWoundKeys;
        }
    }
}