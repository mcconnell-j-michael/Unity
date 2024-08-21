using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace Ashen.WoundSystem
{
    public class WoundScriptableObject : SerializedScriptableObject
    {
        [EnumSODropdown]
        public WoundCategory woundCategory;

        [NonSerialized, OdinSerialize]
        [Hide]
        public WoundBuilder woundBuilder;
    }
}