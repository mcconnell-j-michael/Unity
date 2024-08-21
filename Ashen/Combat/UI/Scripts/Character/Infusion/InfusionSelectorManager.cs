using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class InfusionSelectorManager : MonoBehaviour
    {
        private Dictionary<CombatInfusion, InfusionUISelector> selectors;
        [SerializeField]
        private CombatInfusion first;
        public CombatInfusion First { get { return first; } }

        private ToolManager toolManager;

        private bool initialized = false;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (initialized)
            {
                return;
            }
            selectors = new Dictionary<CombatInfusion, InfusionUISelector>();
            InfusionUISelector[] foundSelectors = GetComponentsInChildren<InfusionUISelector>(true);
            foreach (InfusionUISelector selector in foundSelectors)
            {
                selectors.Add(selector.Infusion, selector);
                selector.StopSelection();
            }
            initialized = true;
        }

        public void Register(ToolManager toolManager)
        {
            Initialize();
            Unregister();
            this.toolManager = toolManager;
            foreach (KeyValuePair<CombatInfusion, InfusionUISelector> pair in selectors)
            {
                pair.Value.Register(toolManager);
            }
        }

        public void Unregister()
        {
            Initialize();
            if (toolManager != null)
            {
                foreach (KeyValuePair<CombatInfusion, InfusionUISelector> pair in selectors)
                {
                    pair.Value.UnRegister();
                }
            }
        }

        public void StartSelection(bool infuse)
        {
            Initialize();
            ResourceValueTool rvTool = toolManager.GetComponent<ResourceValueTool>();
            AttributeTool aTool = toolManager.GetComponent<AttributeTool>();
            foreach (KeyValuePair<CombatInfusion, InfusionUISelector> pair in selectors)
            {
                DerivedAttribute currentInfusionLevel = pair.Key.InfusionLevel;
                int currentInfusionLevelValue = (int)aTool.GetAttribute(currentInfusionLevel);
                if (!infuse)
                {
                    pair.Value.StartSelection(currentInfusionLevelValue > 0);
                    continue;
                }
                ThresholdEventValue value = rvTool.GetValue(pair.Key.CurrentSaturationValue);
                int expectedValue = (int)aTool.GetAttribute(pair.Key.SaturationLimit);
                DerivedAttribute maxInfusionLevel = pair.Key.InfusionMaxLevel;
                bool isNotMax = currentInfusionLevelValue < (int)aTool.GetAttribute(maxInfusionLevel);
                pair.Value.StartSelection(value.currentValue >= expectedValue && isNotMax);
            }
        }

        public void StopSelection()
        {
            Initialize();
            foreach (KeyValuePair<CombatInfusion, InfusionUISelector> pair in selectors)
            {
                pair.Value.StopSelection();
            }
        }

        public void Select(CombatInfusion toSelect)
        {
            Initialize();
            if (selectors.TryGetValue(toSelect, out InfusionUISelector selector))
            {
                selector.Select();
            }
        }

        public void Deselect(CombatInfusion toDeselect)
        {
            Initialize();
            if (selectors.TryGetValue(toDeselect, out InfusionUISelector selector))
            {
                selector.Deselect();
            }
        }

        public bool CanBeSubmitted(CombatInfusion toBeSubmitted)
        {
            Initialize();
            if (selectors.TryGetValue(toBeSubmitted, out InfusionUISelector selector))
            {
                return selector.CanBeSubmitted;
            }
            return false;
        }

        public AbilitySO GetInfusionEffect(CombatInfusion resourceValue)
        {
            Initialize();
            if (selectors.TryGetValue(resourceValue, out InfusionUISelector selector))
            {
                return selector.InfusionEffect;
            }
            return null;
        }

        public AbilitySO GetDiffusionEffect(CombatInfusion resourceValue)
        {
            Initialize();
            if (selectors.TryGetValue(resourceValue, out InfusionUISelector selector))
            {
                return selector.DiffusionEffect;
            }
            return null;
        }

        public CombatInfusion GetUp(CombatInfusion current)
        {
            Initialize();
            return selectors[current].OnUp;
        }

        public CombatInfusion GetDown(CombatInfusion current)
        {
            Initialize();
            return selectors[current].OnDown;
        }

        public CombatInfusion GetLeft(CombatInfusion current)
        {
            Initialize();
            return selectors[current].OnLeft;
        }

        public CombatInfusion GetRight(CombatInfusion current)
        {
            Initialize();
            return selectors[current].OnRight;
        }
    }
}
