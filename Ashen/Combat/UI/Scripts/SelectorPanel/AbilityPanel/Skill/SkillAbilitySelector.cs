using Ashen.SkillTreeSystem;
using TMPro;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class SkillAbilitySelector : A_AbilitySelector
    {
        public SkillAbilityPanelHandler handler;
        public TextMeshProUGUI skillCost;
        public TierLevelsManager tierLevelsManager;

        protected override void InternalOnCreate()
        {
            RectTransform skillRect = transform as RectTransform;
            skillCost.rectTransform.sizeDelta = new Vector2(skillRect.rect.width / 2f, skillCost.rectTransform.sizeDelta.y);
        }
        public override void OnSetValid(bool valid)
        {
            if (valid)
            {
                skillCost.color = handler.validCost;
            }
            else
            {
                skillCost.color = handler.invalidCost;
            }
        }
    }
}