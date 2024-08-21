namespace Ashen.CombatSystem
{
    public abstract class A_AbilitySelector : A_Selector
    {
        private SkillColorTheme validOption;
        private SkillColorTheme invalidOption;

        public void SetColorThemes(SkillColorTheme validOption, SkillColorTheme invalidOption)
        {
            this.validOption = validOption;
            this.invalidOption = invalidOption;
        }

        protected override void OnValid()
        {
            background.color = validOption.background;
            gradient.LinearColor1 = validOption.color1;
            gradient.LinearColor2 = validOption.color2;
            abilityName.color = validOption.name;
        }

        protected override void OnInValid()
        {
            background.color = invalidOption.background;
            gradient.LinearColor1 = invalidOption.color1;
            gradient.LinearColor2 = invalidOption.color2;
            abilityName.color = invalidOption.name;
        }
    }
}
