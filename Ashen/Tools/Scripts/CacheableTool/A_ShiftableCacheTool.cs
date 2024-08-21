using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public abstract class A_ShiftableCacheTool<Tool, Configuration, Enum, Base, Current, Shift> :
        A_ConfigurableTool<Tool, Configuration>,
        IEnumerable<Enum>,
        I_Retrievable<Enum, Current>,
        I_CacheHandler<Enum>,
        I_Cacher<Enum>,
        I_Shiftable<Enum, Shift>,
        I_CacheableTool<Enum, Current>
        where Tool : A_ConfigurableTool<Tool, Configuration>
        where Configuration : A_Configuration<Tool, Configuration>
        where Enum : class, I_EnumSO
    {
        protected CachingComponent<Enum> cachingComponent;
        protected ShiftingComponent<Enum, Base, Current, Shift> shiftingComponent;

        protected abstract int GetEnumListSize();
        protected abstract IEnumerator<Enum> GetEnumeratorInternal();
        public abstract A_Shiftable<Base, Current, Shift> GenerateShiftableValues();

        public override void Initialize()
        {
            base.Initialize();
            int size = GetEnumListSize();
            cachingComponent = new CachingComponent<Enum>(this);
            cachingComponent.Initialize(size);
            shiftingComponent = new ShiftingComponent<Enum, Base, Current, Shift>(GenerateShiftableValues(), toolManager, this, this);
        }

        private void Start()
        {
            shiftingComponent.Initialize(GetType().Name, this);
        }

        public void AddShift(Enum enumValue, int priority, ShiftCategory shiftCategory, string source, Shift value)
        {
            if (shiftCategory == null)
            {
                shiftCategory = ShiftCategories.Instance.GetDefault();
            }
            shiftingComponent.AddShift(enumValue, priority, shiftCategory, source, value);
        }

        public void AddShift(Enum enumValue, ShiftCategory shiftCategory, string source, Shift value)
        {
            AddShift(enumValue, 1, shiftCategory, source, value);
        }

        public void AddShift(Enum enumValue, int priority, string source, Shift value)
        {
            AddShift(enumValue, priority, null, source, value);
        }

        public void RemoveShift(Enum enumValue, ShiftCategory shiftCategory, string source)
        {
            shiftingComponent.RemoveShift(enumValue, shiftCategory, source);
        }

        public void RemoveShift(Enum enumValue, string source)
        {
            RemoveShift(enumValue, ShiftCategories.Instance.GetDefault(), source);
        }

        public Current GetAttribute(Enum enumValue)
        {
            return shiftingComponent.GetAttribute(enumValue);
        }

        public Current Get(Enum enumValue, DeliveryArgumentPacks argument = null)
        {
            return shiftingComponent.Get(enumValue, argument);
        }

        public Current GetAttribute(Enum enumValue, AttributeLimiter limiter)
        {
            return shiftingComponent.GetAttribute(enumValue, limiter);
        }

        public Current GetBaseValue(Enum enumValue)
        {
            return shiftingComponent.GetBaseValue(enumValue);
        }

        [ReadOnly, ShowInInspector]
        private Dictionary<Enum, Current> statValues = default;

        [Button]
        private void BuildStatValues()
        {
            if (GetEnumListSize() <= 0)
            {
                statValues = null;
                return;
            }
            if (statValues == null || statValues.Count != GetEnumListSize())
            {
                statValues = new Dictionary<Enum, Current>();
                foreach (Enum enumValue in this)
                {
                    statValues.Add(enumValue, default);
                }
            }
            if (statValues != null && statValues.Count == DerivedAttributes.Count)
            {
                foreach (Enum enumValue in this)
                {
                    statValues[enumValue] = GetAttribute(enumValue);
                }
            }
        }

        public IEnumerator<Enum> GetEnumerator()
        {
            return GetEnumeratorInternal();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool HasCacheables(Enum enumSO)
        {
            return cachingComponent.HasCacheables(enumSO);
        }

        public void Cache(I_EnumSO enumSO, I_EnumCacheable toCache)
        {
            if (enumSO is Enum enumValue)
            {
                cachingComponent.Cache(enumValue, toCache);
            }
        }

        public void UnCache(I_EnumSO enumSO, I_EnumCacheable toUnCache)
        {
            if (enumSO is Enum enumValue)
            {
                cachingComponent.UnCache(enumValue, toUnCache);
            }
        }

        public void OnChange(Enum enumSO)
        {
            cachingComponent.OnChange(enumSO);
        }

        public I_DeliveryTool GetDeliveryTool()
        {
            return toolManager.Get<DeliveryTool>();
        }
    }
}
