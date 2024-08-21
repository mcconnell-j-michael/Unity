using System;
using System.Runtime.Serialization;

namespace Ashen.DeliverySystem
{
    /**
     * This class groups together DamageTypes (i.e. ElementalDamage)
     **/
    [Serializable]
    public class DamageContainer : A_ExistsContainer<DamageType, DamageTypes>
    {
        public DamageContainer(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}