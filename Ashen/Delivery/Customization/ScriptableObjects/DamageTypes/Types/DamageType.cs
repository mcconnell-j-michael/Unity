using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.DeliverySystem
{
    /**
     * A damage type defines what resistances an attack must be compared to when applied to a character
     **/
    public class DamageType : A_EnumSO<DamageType, DamageTypes>
    {
        [OdinSerialize, EnumSODropdown]
        private List<ReservedEffectFloatArgument> reservedScales;
        [OdinSerialize, EnumSODropdown]
        private List<ReservedEffectFloatArgument> reservedFlat;

        public IEnumerable<ReservedEffectFloatArgument> GetScaleEnumerator()
        {
            if (reservedScales != null)
            {
                for (int x = 0; x < reservedScales.Count; x++)
                {
                    yield return reservedScales[x];
                }
            }
        }

        public IEnumerable<ReservedEffectFloatArgument> GetFlatEnumerator()
        {
            if (reservedFlat != null)
            {
                for (int x = 0; x < reservedFlat.Count; x++)
                {
                    yield return reservedFlat[x];
                }
            }
        }

        public bool HasScale(ReservedEffectFloatArgument arg)
        {
            if (reservedScales == null)
            {
                return false;
            }
            return reservedScales.Contains(arg);
        }

        public bool HasFlat(ReservedEffectFloatArgument arg)
        {
            if (reservedFlat == null)
            {
                return false;
            }
            return reservedFlat.Contains(arg);
        }
    }
}