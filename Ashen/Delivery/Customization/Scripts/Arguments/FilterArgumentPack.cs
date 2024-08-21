using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    public class FilterArgumentPack : A_DeliveryArgumentPack<FilterArgumentPack>
    {
        private List<KeyContainer<I_Filter>> preFilters;
        private List<KeyContainer<I_Filter>> postFilters;

        public FilterArgumentPack()
        {
            preFilters = new List<KeyContainer<I_Filter>>();
            postFilters = new List<KeyContainer<I_Filter>>();
        }

        public override I_DeliveryArgumentPack Initialize()
        {
            return new FilterArgumentPack();
        }

        public void AddPreFilter(KeyContainer<I_Filter> filter)
        {
            preFilters.Add(filter);
        }

        public void AddPreFilters(List<KeyContainer<I_Filter>> filters)
        {
            if (filters == null || filters.Count == 0)
            {
                return;
            }
            preFilters.AddRange(filters);
        }

        public void AddPostFilter(KeyContainer<I_Filter> filter)
        {
            postFilters.Add(filter);
        }

        public void AddPostFilters(List<KeyContainer<I_Filter>> filters)
        {
            if (filters == null || filters.Count == 0)
            {
                return;
            }
            postFilters.AddRange(filters);
        }

        public List<KeyContainer<I_Filter>> GetPreFilters()
        {
            return preFilters;
        }

        public List<KeyContainer<I_Filter>> GetPostFilters()
        {
            return postFilters;
        }

        public override void Clear()
        {
            preFilters.Clear();
            postFilters.Clear();
        }

        public override void CopyInto(I_DeliveryArgumentPack pack)
        {
            FilterArgumentPack filterAP = pack as FilterArgumentPack;
            filterAP.preFilters.AddRange(preFilters);
            filterAP.postFilters.AddRange(postFilters);
        }
    }
}