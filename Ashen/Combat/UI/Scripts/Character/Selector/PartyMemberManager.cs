using Ashen.DeliverySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using DG.Tweening;
using JoshH.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class PartyMemberManager : A_CharacterSelector
    {
        [HideInInspector]
        public PartyUIManager partyUIManager;

        //public ResourceBarManager healthBarManager;
        //public ResourceBarManager resourceBarManager;
        //public ResourceDialManager concentrationDialManager;

        public TextMeshProUGUI playerName;

        public UIGradient defaultColor;
        public UIGradient selectedColor;
        public UIGradient hitColor;
        public UIGradient targetedColor;

        public DOTweenAnimation buffAnimation;
        public DOTweenAnimation statDownAnimation;

        public StatusEffectSymbolManagerUI symbolManager;

        [SerializeField]
        private ActionPointUIManager actionPointManager;

        private Tween damageTakenTween;

        public GameObject deathFilter;
        private List<I_ResourceManagerUI> resourceManagers;

        [SerializeField]
        private ExtendedEffectTrigger onConcentrationBroken;
        [SerializeField]
        private InfusionSelectorManager inusionSelectorManager;
        public InfusionSelectorManager InfusionSelectorManager { get { return inusionSelectorManager; } }

        protected override void RegisterToolManagerInternal(ToolManager toolManager)
        {
            resourceManagers = new List<I_ResourceManagerUI>();
            resourceManagers.AddRange(gameObject.GetComponentsInChildren<I_ResourceManagerUI>());
            playerName.text = toolManager.gameObject.name;
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            triggerTool.RegisterTriggerListener(ExtendedEffectTriggers.Instance.BuffRecieved, this);
            DOTweenAnimation[] anims = gameObject.GetComponents<DOTweenAnimation>();
            if (anims != null)
            {
                for (int x = 0; x < anims.Length; x++)
                {
                    if (anims[x].id == "Hit")
                    {
                        damageTakenTween = anims[x].tween;
                    }
                }
            }
            StatusTool statusTool = toolManager.Get<StatusTool>();
            statusTool.SymbolUI = symbolManager;
            foreach (I_ResourceManagerUI resourceManager in resourceManagers)
            {
                resourceManager.RegisterToolManager(toolManager);
            }
            actionPointManager.RegisterToolManager(toolManager);
            partyUIManager.Recalculate();
            InfusionSelectorManager.Register(toolManager);
        }

        protected override void UnregisterToolManagerInternal()
        {
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            triggerTool.UnregisterTriggerListener(ExtendedEffectTriggers.Instance.BuffRecieved, this);
            damageTakenTween = null;
            StatusTool statusTool = toolManager.Get<StatusTool>();
            statusTool.SymbolUI = null;
            this.toolManager = null;
            playerName.text = "";
            foreach (I_ResourceManagerUI resourceManager in resourceManagers)
            {
                resourceManager.UnregisterToolManager();
            }
            actionPointManager.UnRegisterToolManager();
            partyUIManager.Recalculate();
            InfusionSelectorManager.Unregister();
        }

        private ExtendedEffectTrigger triggerLock = null;

        protected override void OnTriggerInternal(ExtendedEffectTrigger trigger)
        {
            if (trigger == ExtendedEffectTriggers.Instance.BuffRecieved)
            {
                ListActionBundle bundles = new();
                bundles.Bundles.Add(new DoTweenObjectProcessor()
                {
                    tween = buffAnimation.tween
                });
                PlayerPartyHolder.Instance.partyManager.GetCurrentBattleContainer().AddProcesor(CombatProcessorTypes.Instance.SUPPORTING_ACTION, bundles);
            }
            else if (trigger == onConcentrationBroken)
            {
                ListActionBundle bundles = new();
                bundles.Bundles.Add(new DoTweenObjectProcessor()
                {
                    tween = statDownAnimation.tween
                });
                PlayerPartyHolder.Instance.partyManager.GetCurrentBattleContainer().AddProcesor(CombatProcessorTypes.Instance.SUPPORTING_ACTION, bundles);
            }
        }

        private void HandleActionStart(ExtendedEffectTrigger trigger)
        {
            if (triggerLock == null)
            {
                triggerLock = trigger;
                selectedColor.enabled = true;
            }
        }
        protected override void OnPrimaryActionStart()
        {
            HandleActionStart(primaryActionStart);
        }
        protected override void OnSecondaryActionStart()
        {
            HandleActionStart(secondaryActionStart);
        }
        private void HandleActionEnd(ExtendedEffectTrigger trigger)
        {
            if (
                (trigger == primaryActionEnd && triggerLock == primaryActionStart) ||
                (trigger == secondaryActionEnd && triggerLock == secondaryActionStart)
            )
            {
                selectedColor.enabled = false;
                triggerLock = null;
            }
        }
        protected override void OnPrimaryActionEnd()
        {
            HandleActionEnd(primaryActionEnd);
        }
        protected override void OnSecondaryActionEnd()
        {
            HandleActionEnd(secondaryActionEnd);
        }

        public override void Selected()
        {
            targetedColor.enabled = true;
        }

        public override void Deselected()
        {
            targetedColor.enabled = false;
        }

        public override void TurnSelectionStart()
        {
            selectedColor.enabled = true;
            actionPointManager.Selected();
        }

        public override void TurnSelectionEnd()
        {
            selectedColor.enabled = false;
            actionPointManager.Deselected();
        }

        public override GameObject GetDisabler()
        {
            return gameObject.transform.parent.gameObject;
        }

        public override void SelectedSecondary()
        {
            selectedColor.enabled = true;
        }

        public override void OnDamageEvent(DamageEvent damageEvent)
        {
            int totalDamage = damageEvent.GetTotal(ResourceValues.Instance.health.listenOn);
            if (totalDamage > 0)
            {
                ListActionBundle bundles = new();

                bundles.Bundles.Add(new CombatLogProcessor()
                {
                    message = toolManager.gameObject.name + " suffered " + totalDamage + " damage!",
                });
                bundles.Bundles.Add(new EnableTemporarilyProcessor()
                {
                    toEnable = hitColor,
                    totalTime = damageTakenTween.Duration(),
                });
                bundles.Bundles.Add(new DamageTextProcessor()
                {
                    message = totalDamage + "",
                    location = gameObject.transform,
                    damageTextPrefab = partyUIManager.damageTextPrefab,
                    parent = partyUIManager.damageTextCanvas,
                });
                bundles.Bundles.Add(new DoTweenObjectProcessor()
                {
                    tween = damageTakenTween,
                    waitTime = .25f,
                });

                PlayerPartyHolder.Instance.partyManager.GetCurrentBattleContainer().AddProcesor(CombatProcessorTypes.Instance.SUPPORTING_ACTION, bundles);
            }
            if (totalDamage < 0)
            {
                ListActionBundle bundles = new();

                bundles.Bundles.Add(new DamageTextProcessor()
                {
                    message = totalDamage + "",
                    location = gameObject.transform,
                    damageTextPrefab = partyUIManager.damageTextPrefab,
                    parent = partyUIManager.damageTextCanvas,
                });
            }
        }

        public override void OnThresholdEvent(ThresholdEventValue value)
        {
            ResourceValueTool resourceValueTool = toolManager.Get<ResourceValueTool>();
            if (value.resourceValue == ResourceValues.Instance.ABILITY_RESOURCE)
            {
                if (value.currentValue < value.previousValue)
                {
                    ListActionBundle bundles = new ListActionBundle();

                    bundles.Bundles.Add(new DamageTextProcessor()
                    {
                        message = (value.previousValue - value.currentValue) + "",
                        location = gameObject.transform,
                        damageTextPrefab = partyUIManager.damageTextPrefab,
                        parent = partyUIManager.damageTextCanvas,
                    });
                }
            }
            else if (value.resourceValue == ResourceValues.Instance.health)
            {
                int difference = (value.previousValue - value.currentValue);
                if (value.damageTaken)
                {
                    ListActionBundle bundles = new ListActionBundle();

                    bundles.Bundles.Add(new CombatLogProcessor()
                    {
                        message = toolManager.gameObject.name + " suffered " + difference + " damage!",
                    });
                    bundles.Bundles.Add(new EnableTemporarilyProcessor()
                    {
                        toEnable = hitColor,
                        totalTime = damageTakenTween.Duration(),
                    });
                    bundles.Bundles.Add(new DamageTextProcessor()
                    {
                        message = difference + "",
                        location = gameObject.transform,
                        damageTextPrefab = partyUIManager.damageTextPrefab,
                        parent = partyUIManager.damageTextCanvas,
                    });
                    bundles.Bundles.Add(new DoTweenObjectProcessor()
                    {
                        tween = damageTakenTween,
                        waitTime = .25f,
                    });

                    PlayerPartyHolder.Instance.partyManager.GetCurrentBattleContainer().AddProcesor(CombatProcessorTypes.Instance.SUPPORTING_ACTION, bundles);
                }
                if (!value.damageTaken)
                {
                    ListActionBundle bundles = new ListActionBundle();

                    bundles.Bundles.Add(new DamageTextProcessor()
                    {
                        message = difference + "",
                        location = gameObject.transform,
                        damageTextPrefab = partyUIManager.damageTextPrefab,
                        parent = partyUIManager.damageTextCanvas,
                    });
                }
            }
        }

        public override void OnFacultyChange(Faculty faculty, bool value)
        {
            if (faculty == Faculties.Instance.CONSCIOUS)
            {
                deathFilter.SetActive(!value);
            }
        }

        public void DisplayPlayerText(string text)
        {
            actionPointManager.Expand(text);
        }

        public void HidePlayerText()
        {
            actionPointManager.Collapse();
        }

        public override void Recalculate(Faculty enumValue, I_DeliveryTool deliveryTool)
        {
            FacultyTool fTool = toolManager.GetComponent<FacultyTool>();
            bool value = fTool.Can(enumValue);
            if (enumValue == Faculties.Instance.CONSCIOUS)
            {
                deathFilter.SetActive(!value);
            }
        }
    }
}