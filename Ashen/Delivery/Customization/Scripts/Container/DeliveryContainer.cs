using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    /**
     * A delivery container is used to deliver a series of
     * primary and secondary DeliveryPacks. A primary pack
     * is one that does not rely on any information other than
     * the target of the delivery. A secondary pack is one that
     * is based on the primary packs. A secondary pack might
     * deal 10% of all physical damage done as bonus fire damage.
     * All secondary packs are only based upon the primary 
     * packs. If there are two or more secondary packs, the second
     * (or any following thereafter) will not be effected by the
     * previous secondary delivery pack(s). The order of events for
     * a DeliveryContainer are as follows: 
     * 1) DeliveryContainer's preFilters
     * 2) Owners preOffensiveFilters
     * 3) Target's preDefensiveFilters
     * 4) PrimaryDeliveryPacks
     * 5) SecondaryDeliveryPacks
     * 6) DeliveryContainer's PostFilters
     * 7) Owners postOffensiveFilters
     * 8) Damage is calculated
     * 9) Target's postDefensiveFilters
     **/
    public class DeliveryContainer : I_DeliveryPack, I_Poolable
    {
        private static DeliveryProcessorContainer deliveryProcessorContainer = default;

        private static DeliveryProcessorContainer DeliveryProcessorContainer
        {
            get
            {
                if (deliveryProcessorContainer == null)
                {
                    deliveryProcessorContainer = Resources.FindObjectsOfTypeAll<DeliveryProcessorContainer>().FirstOrDefault();
                }
                return deliveryProcessorContainer;
            }
        }

        private List<I_Effect> primaryEffects;
        public List<I_Effect> PrimaryEffects
        {
            get
            {
                return primaryEffects;
            }
            set
            {
                if (value != null)
                {
                    primaryEffects = value;
                }
            }
        }

        private List<KeyContainer<I_Filter>> preFilters;
        public List<KeyContainer<I_Filter>> PreFilters
        {
            get
            {
                return preFilters;
            }
            set
            {
                if (value != null)
                {
                    preFilters = value;
                }
            }
        }
        private List<KeyContainer<I_Filter>> postFilters;
        public List<KeyContainer<I_Filter>> PostFilters
        {
            get
            {
                return postFilters;
            }
            set
            {
                if (value != null)
                {
                    postFilters = value;
                }
            }
        }

        public DeliveryContainer()
        {
            primaryEffects = new List<I_Effect>();
            preFilters = new List<KeyContainer<I_Filter>>();
            postFilters = new List<KeyContainer<I_Filter>>();
        }

        public void Apply(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            foreach (DeliveryProcess process in DeliveryProcessorContainer.processors)
            {
                process.processor.Process(owner, target, deliveryArguments);
            }
        }

        public void FillArguments(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            EffectsArgumentPack effectsPack = deliveryArguments.GetPack<EffectsArgumentPack>();
            effectsPack.AddEffects(primaryEffects);
            FilterArgumentPack filterPack = deliveryArguments.GetPack<FilterArgumentPack>();
            filterPack.AddPreFilters(preFilters);
            filterPack.AddPostFilters(postFilters);
        }

        public void AddPrimaryEffect(I_Effect deliveryPack)
        {
            PrimaryEffects.Add(deliveryPack);
        }

        public void RemovePrimaryEffect(I_Effect deliveryPack)
        {
            PrimaryEffects.Remove(deliveryPack);
        }

        public void AddPreFilter(KeyContainer<I_Filter> filter)
        {
            PreFilters.Add(filter);
        }

        public void RemoverPreFilter(KeyContainer<I_Filter> filter)
        {
            PreFilters.Remove(filter);
        }

        public void AddPostFilter(KeyContainer<I_Filter> filter)
        {
            PostFilters.Add(filter);
        }

        public void RemovePostFilter(KeyContainer<I_Filter> filter)
        {
            PostFilters.Remove(filter);
        }

        public void Clear()
        {
            PrimaryEffects.Clear();
            PreFilters.Clear();
            PostFilters.Clear();
        }

        public bool Empty()
        {
            return PrimaryEffects.Count == 0 && PreFilters.Count == 0 && PostFilters.Count == 0;
        }

        public void Disable()
        { }

        public void Initialize()
        {
            Clear();
        }
    }
}