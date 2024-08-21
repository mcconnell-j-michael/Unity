using Ashen.ToolSystem;
using Sirenix.Serialization;

namespace Ashen.DeliverySystem
{
    /**
     * Work In Progress
     **/
    public abstract class DisableComponent : A_SimpleComponent
    {
        //private Character_Action_Enum CombatAction;

        public DisableComponent()
        {
            //DerivedStatusEffect = derivedStatusEffect;
            //CombatAction = combatAction;
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            ApplyEffect(dse);
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            RemoveEffect(dse);
        }

        public abstract void ApplyEffect(ExtendedEffect dse);
        public abstract void RemoveEffect(ExtendedEffect dse);
    }
}