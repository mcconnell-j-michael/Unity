using Ashen.WoundSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class WoundToolConfiguration : A_Configuration<WoundTool, WoundToolConfiguration>
    {
        [OdinSerialize, HideIf(nameof(IsDefault))]
        private bool enableDefaultOverride;
        [OdinSerialize, AutoPopulate]
        [ShowIf("@" + nameof(IsDefault) + "() || " + nameof(enableDefaultOverride))]
        private List<WoundScriptableObject> defaultAvailableWounds;
        private List<WoundScriptableObject> DefaultAvailableWounds
        {
            get
            {
                if (defaultAvailableWounds == null)
                {
                    defaultAvailableWounds = new List<WoundScriptableObject>();
                }
                return defaultAvailableWounds;
            }
        }
        [OdinSerialize, AutoPopulate, HideIf(nameof(IsDefault))]
        private List<WoundScriptableObject> additionalAvailableWounds;
        private List<WoundScriptableObject> AdditionalAvailableWounds
        {
            get
            {
                if (additionalAvailableWounds == null)
                {
                    additionalAvailableWounds = new List<WoundScriptableObject>();
                }
                return additionalAvailableWounds;
            }
        }

        [OdinSerialize]
        private Dictionary<WoundCategory, DerivedAttribute> woundResistancePerCategory;
        [OdinSerialize, ShowIf(nameof(IsDefault))]
        private ResourceValue woundResource;
        [OdinSerialize, ShowIf(nameof(IsDefault))]
        private EffectFloatArgument woundScale;

        public List<WoundScriptableObject> AvailableWounds
        {
            get
            {
                List<WoundScriptableObject> availableWounds = new();
                if (!enableDefaultOverride)
                {
                    availableWounds.AddRange(GetDefault().DefaultAvailableWounds);
                }
                else
                {
                    availableWounds.AddRange(DefaultAvailableWounds);
                }
                availableWounds.AddRange(AdditionalAvailableWounds);
                return availableWounds;
            }
        }

        public DerivedAttribute GetWoundResistance(WoundCategory category)
        {
            if (IsDefault())
            {
                return woundResistancePerCategory[category];
            }
            if (woundResistancePerCategory == null)
            {
                return GetDefault().GetWoundResistance(category);
            }
            if (woundResistancePerCategory.TryGetValue(category, out DerivedAttribute woundResistance))
            {
                return woundResistance;
            }
            return GetDefault().GetWoundResistance(category);
        }

        public ResourceValue WoundResource
        {
            get
            {
                if (IsDefault())
                {
                    return woundResource;
                }
                return GetDefault().WoundResource;
            }
        }

        public EffectFloatArgument WoundScale
        {
            get
            {
                if (IsDefault())
                {
                    return woundScale;
                }
                return GetDefault().WoundScale;
            }
        }

    }
}