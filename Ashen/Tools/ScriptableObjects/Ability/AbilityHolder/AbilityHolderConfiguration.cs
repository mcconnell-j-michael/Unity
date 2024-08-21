using Ashen.AbilitySystem;
using Ashen.ToolSystem;
using Ashen.WoundSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;

public class AbilityHolderConfiguration : A_Configuration<AbilityTool, AbilityHolderConfiguration>
{
    [OdinSerialize]
    private AbilitySO attackAbility;
    [OdinSerialize]
    private AbilitySO defendAbility;

    [OdinSerialize, HideIf(nameof(IsDefault))]
    private bool enableDefaultOverride;
    [OdinSerialize]
    [ShowIf("@" + nameof(IsDefault) + "() || " + nameof(enableDefaultOverride))]
    private List<AbilitySO> defaultAbilities;

    [OdinSerialize, AutoPopulate, HideIf(nameof(IsDefault)), Title("Additional Abilities")]
    private List<AbilitySO> additionalAbilities;

    public List<AbilitySO> DefaultAbilities
    {
        get
        {
            List<AbilitySO> abilities = new();
            if (this == GetDefault())
            {
                if (defaultAbilities != null)
                {
                    abilities.AddRange(defaultAbilities);
                }
                return abilities;
            }
            if (enableDefaultOverride)
            {
                abilities.AddRange(defaultAbilities);
            }
            else
            {
                abilities.AddRange(GetDefault().defaultAbilities);
            }
            if (additionalAbilities != null)
            {
                abilities.AddRange(additionalAbilities);
            }
            return abilities;
        }
    }

    public AbilitySO AttackAbility
    {
        get
        {
            if (attackAbility == null)
            {
                if (this == GetDefault())
                {
                    return null;
                }
                return GetDefault().attackAbility;
            }
            return attackAbility;
        }
    }

    public AbilitySO DefendAbility
    {
        get
        {
            if (defendAbility == null)
            {
                if (this == GetDefault())
                {
                    return null;
                }
                return GetDefault().defendAbility;
            }
            return defendAbility;
        }
    }
}
