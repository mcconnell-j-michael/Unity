using Ashen.EnumSystem;
using System;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class CachingComponent<Enum>
        where Enum : I_EnumSO
    {

        private I_Cacher<Enum> cacher;

        [NonSerialized]
        private List<I_EnumCacheable>[] cacheables;

        public CachingComponent(I_Cacher<Enum> cacher)
        {
            this.cacher = cacher;
        }

        public void Initialize(int size)
        {
            cacheables = new List<I_EnumCacheable>[size];
            for (int x = 0; x < size; x++)
            {
                cacheables[x] = new List<I_EnumCacheable>();
            }
        }

        public bool HasCacheables(Enum enumSO)
        {
            return cacheables[enumSO.GetIndex()].Count > 0;
        }

        public void Cache(Enum enumSO, I_EnumCacheable toCache)
        {
            if (!cacheables[enumSO.GetIndex()].Contains(toCache))
            {
                cacheables[enumSO.GetIndex()].Add(toCache);
            }
        }

        public void UnCache(Enum enumSO, I_EnumCacheable toUnCache)
        {
            cacheables[enumSO.GetIndex()].Remove(toUnCache);
        }

        public void OnChange(Enum enumSO)
        {
            cacher.PreOnChange(enumSO);
            foreach (I_EnumCacheable cacheable in cacheables[enumSO.GetIndex()])
            {
                cacheable.Recalculate(enumSO, cacher.GetDeliveryTool());
            }
            cacher.PostOnChange(enumSO);
        }
    }
}