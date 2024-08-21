using Ashen.DeliverySystem;
using Ashen.EnumSystem;

namespace Ashen.ToolSystem
{
    public abstract class A_CacheableTool<Tool, Configuration, Enum, ReturnType> :
        A_ConfigurableTool<Tool, Configuration>,
        I_CacheableTool<Enum, ReturnType>,
        I_Cacher<Enum>,
        I_CacheHandler<Enum>
        where Tool : A_ConfigurableTool<Tool, Configuration>
        where Configuration : A_Configuration<Tool, Configuration>
        where Enum : I_EnumSO
    {

        protected CachingComponent<Enum> cachingComponent;

        public override void Initialize()
        {
            base.Initialize();
            int size = GetEnumListSize();
            cachingComponent = new CachingComponent<Enum>(this);
            cachingComponent.Initialize(size);
        }

        protected abstract int GetEnumListSize();

        public abstract ReturnType Get(Enum enumSO, DeliveryArgumentPacks equationArguments);

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

        public bool HasCacheables(Enum enumSO)
        {
            return cachingComponent.HasCacheables(enumSO);
        }

        public I_DeliveryTool GetDeliveryTool()
        {
            return toolManager.Get<DeliveryTool>();
        }

        protected virtual void PreOnChange(Enum enumSO) { }
        protected virtual void PostOnChange(Enum enumSO) { }
    }
}