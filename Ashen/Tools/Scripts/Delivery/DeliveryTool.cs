using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.ObjectPoolSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static Ashen.DeliverySystem.FilterComponentBuilder;

namespace Ashen.ToolSystem
{
    /**
     * This Monobehaviour manages the defensive and offensive filters for a character
     **/
    public class DeliveryTool : A_ConfigurableTool<DeliveryTool, DeliveryToolConfiguration>, I_DeliveryTool
    {
        private List<KeyContainer<I_Filter>> preDefensiveFilters;
        private List<KeyContainer<I_Filter>> preOffensiveFilters;
        private List<KeyContainer<I_Filter>> postDefensiveFilters;
        private List<KeyContainer<I_Filter>> postOffensiveFilters;
        [ShowInInspector]
        public List<KeyContainer<I_Filter>> PreDefensiveFilters
        {
            get
            {
                return preDefensiveFilters;
            }
        }
        [ShowInInspector]
        public List<KeyContainer<I_Filter>> PostDefensiveFilters
        {
            get
            {
                return postDefensiveFilters;
            }
        }
        [ShowInInspector]
        public List<KeyContainer<I_Filter>> PreOffensiveFilters
        {
            get
            {
                return preOffensiveFilters;
            }
        }
        [ShowInInspector]
        public List<KeyContainer<I_Filter>> PostOffensiveFilters
        {
            get
            {
                return postOffensiveFilters;
            }
        }

        public List<KeyContainer<I_Filter>> GetPreDefensiveFilters()
        {
            return preDefensiveFilters;
        }
        public List<KeyContainer<I_Filter>> GetPostDefensiveFilters()
        {
            return postDefensiveFilters;
        }
        public List<KeyContainer<I_Filter>> GetPreOffensiveFilters()
        {
            return preOffensiveFilters;
        }
        public List<KeyContainer<I_Filter>> GetPostOffensiveFilters()
        {
            return postOffensiveFilters;
        }

        public void RemoveFilter(FilterType type, string key)
        {
            List<KeyContainer<I_Filter>> filters = null;
            switch (type)
            {
                case FilterType.POST_DEFENSIVE:
                    filters = GetPostDefensiveFilters();
                    break;
                case FilterType.POST_OFFENSIVE:
                    filters = GetPostOffensiveFilters();
                    break;
                case FilterType.PRE_DEFENSIVE:
                    filters = GetPreDefensiveFilters();
                    break;
                case FilterType.PRE_OFFENSIVE:
                    filters = GetPreOffensiveFilters();
                    break;
            }
            for (int x = 0; x < filters.Count; x++)
            {
                KeyContainer<I_Filter> container = filters[x];
                if (container.key.Equals(container.key))
                {
                    filters.RemoveAt(x);
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            postDefensiveFilters = new List<KeyContainer<I_Filter>>();
            preDefensiveFilters = new List<KeyContainer<I_Filter>>();
            preOffensiveFilters = new List<KeyContainer<I_Filter>>();
            postOffensiveFilters = new List<KeyContainer<I_Filter>>();
        }

        public void Deliver(AbilitySO ability, ToolManager toolManager)
        {

        }

        [Button]
        public void Deliver(DeliveryPackScriptableObject deliveryPack, ToolManager owner)
        {
            Deliver(deliveryPack.deliveryPack, owner);
        }

        public void Deliver(I_EffectBuilder builder, ToolManager owner)
        {
            DeliveryContainer container = AGenericPool<DeliveryContainer>.Get();
            DeliveryArgumentPacks packs = AGenericPool<DeliveryArgumentPacks>.Get();
            container.AddPrimaryEffect(builder.Build(owner.Get<DeliveryTool>(), this, packs));
            DeliveryUtility.Deliver(container, owner.Get<DeliveryTool>(), this, packs);
            AGenericPool<DeliveryArgumentPacks>.Release(packs);
            AGenericPool<DeliveryContainer>.Release(container);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
