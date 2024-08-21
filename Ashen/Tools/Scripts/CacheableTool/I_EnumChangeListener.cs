using Ashen.EnumSystem;

namespace Ashen.ToolSystem
{
    public interface I_EnumChangeListener<Enum> where Enum : I_EnumSO
    {
        public void OnChange(Enum enumSO);
    }
}
