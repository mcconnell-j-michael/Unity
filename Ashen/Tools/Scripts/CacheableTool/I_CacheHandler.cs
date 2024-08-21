using Ashen.EnumSystem;

namespace Ashen.ToolSystem
{
    public interface I_CacheHandler<Enum>
        where Enum : I_EnumSO
    {
        bool HasCacheables(Enum enumSO);
        void Cache(I_EnumSO enumSO, I_EnumCacheable toCache);
        void UnCache(I_EnumSO enumSO, I_EnumCacheable toUnCache);
        void OnChange(Enum enumSO);
    }
}