using Ashen.EnumSystem;

namespace Ashen.ToolSystem
{
    public interface I_CacheableTool<Enum, ReturnType> : I_EnumChangeListener<Enum> where Enum : I_EnumSO
    {
        public ReturnType Get(Enum enumSO, DeliveryArgumentPacks equationArguments);
        public void Cache(I_EnumSO enumSO, I_EnumCacheable toCache);
    }
}