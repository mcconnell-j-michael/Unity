using Ashen.EnumSystem;

namespace Ashen.SaturationSystem
{
    public class Saturation : A_DependentEnumSO<Saturation, Saturations, SaturationLevel, SaturationLevels, SaturationType, SaturationTypes>
    {
        public SaturationLevel level { get { return firstDependency; } }
        public SaturationType type { get { return secondDependency; } }
    }
}