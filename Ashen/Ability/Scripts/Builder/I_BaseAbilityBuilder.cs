namespace Ashen.AbilitySystem
{
    public interface I_BaseAbilityBuilder
    {
        I_AbilityProcessor Build(Ability ability);
        string GetTabName();
        I_BaseAbilityBuilder CloneBuilder();
    }
}