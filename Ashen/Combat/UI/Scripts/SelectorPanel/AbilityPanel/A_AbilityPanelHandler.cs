using Ashen.AbilitySystem;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public abstract class A_AbilityPanelHandler<T, E> : A_VariablePanel<T, E> where T : A_AbilityPanelHandler<T, E> where E : A_AbilitySelector
    {

        [Hide, FoldoutGroup("Color Scheme"), Title("Valid")]
        public SkillColorTheme validOption;
        [Hide, FoldoutGroup("Color Scheme"), Title("Invalid")]
        public SkillColorTheme invalidOption;

        public IEnumerator LoadAbilities(AbilityHolder abilityHolder)
        {
            //ResourceValueTool rvTool = abilityHolder.toolManager.Get<ResourceValueTool>();
            int x = 0;
            E firstSelector = null;
            int totalSizeCount = Mathf.Max(abilityHolder.GetCount(), numberOfSelectors);
            selectorPanelRect.sizeDelta = new Vector2(selectorPanelRect.sizeDelta.x, (height * numberOfSelectors) + (offsetPerSelector * (numberOfSelectors)) + initialOffset);
            fullContentRect.sizeDelta = new Vector2(fullContentRect.sizeDelta.x, (height * totalSizeCount) + ((totalSizeCount - 1) * offsetPerSelector) + initialOffset + initialOffset);
            foreach (Ability ability in abilityHolder.GetAbilities())
            {
                E selector = null;
                if (selectors.Count > x)
                {
                    selector = selectors[x];
                }
                else
                {
                    GameObject slectorGo = Instantiate(selectorPrefab, selectorHolder.transform);
                    selector = slectorGo.GetComponent<E>();
                    RegisterSelector(selector);
                    selector.SetColorThemes(validOption, invalidOption);
                    selectors.Add(selector);
                    selector.OnCreate();
                    RectTransform selectorRect = selector.transform as RectTransform;
                    selector.transform.localPosition = new Vector3(selector.transform.localPosition.x, -(initialOffset + ((selectorRect.rect.height + offsetPerSelector) * x)));
                }
                if (firstSelector == null)
                {
                    firstSelector = selector;
                }
                selector.ability = ability;
                selector.abilityName.text = ability.name;
                selector.gameObject.SetActive(true);
                yield return null;
                OnLoad(ability, selector);
                x++;
            }
            activeAbilities = x;
            for (; x < selectors.Count; x++)
            {
                selectors[x].gameObject.SetActive(false);
            }
            yield break;
        }

        public abstract void OnLoad(Ability ability, E selector);

        public E GetSelectorForAbility(Ability ability)
        {
            foreach (E selector in selectors)
            {
                if (selector.ability == ability)
                {
                    return selector;
                }
            }
            return null;
        }
    }
}