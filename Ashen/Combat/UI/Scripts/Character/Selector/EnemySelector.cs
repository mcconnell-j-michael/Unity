using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.PartySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using DG.Tweening;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class EnemySelector : A_CharacterSelector, I_TriggerListener, I_DamageListener
    {
        private EnemyArrow arrow;
        private EnemyArrowSecondary arrowSecondary;
        private EnemyHealthCanvasHolder healthCanvasHolder;

        public GameObject attack;
        public AbilitySO attackAbility;

        private Tween turnStartTween;
        private Tween damageTakenTween;

        protected override void RegisterToolManagerInternal(ToolManager toolManager)
        {
            DOTweenAnimation[] anims = toolManager.gameObject.GetComponents<DOTweenAnimation>();
            arrow = GetComponentInChildren<EnemyArrow>();
            arrowSecondary = GetComponentInChildren<EnemyArrowSecondary>();
            healthCanvasHolder = GetComponentInChildren<EnemyHealthCanvasHolder>();
            if (anims != null)
            {
                for (int x = 0; x < anims.Length; x++)
                {
                    if (anims[x].id == "ActionStart")
                    {
                        turnStartTween = anims[x].tween;
                    }
                    else if (anims[x].id == "DamageTaken")
                    {
                        damageTakenTween = anims[x].tween;
                    }
                }
            }
        }

        protected override void UnregisterToolManagerInternal()
        {
            turnStartTween = null;
            damageTakenTween = null;
            arrow = null;
            arrowSecondary = null;
            healthCanvasHolder = null;
            this.toolManager = null;
        }

        public override void Selected()
        {
            if (arrow != null)
            {
                arrow.selector.SetActive(true);
            }
            if (healthCanvasHolder != null)
            {
                healthCanvasHolder.canvas.gameObject.SetActive(true);
            }
        }

        public override void SelectedSecondary()
        {
            if (arrowSecondary != null)
            {
                arrowSecondary.selector.SetActive(true);
            }
            if (healthCanvasHolder != null)
            {
                healthCanvasHolder.canvas.gameObject.SetActive(true);
            }
        }

        public override void Deselected()
        {
            if (arrow != null)
            {
                arrow.selector.SetActive(false);
            }
            if (arrowSecondary != null)
            {
                arrowSecondary.selector.SetActive(false);
            }
            if (healthCanvasHolder != null)
            {
                healthCanvasHolder.canvas.gameObject.SetActive(false);
            }
        }

        private void HandleActionStart()
        {
            EnemyPartyHolder.Instance.enemyPartyManager.GetCurrentBattleContainer().AddProcesor(CombatProcessorTypes.Instance.SUPPORTING_ACTION,
                    new DoTweenObjectProcessor()
                    {
                        tween = turnStartTween,
                        waitTime = 1f,
                    }
                );
        }
        protected override void OnPrimaryActionStart()
        {
            HandleActionStart();
        }
        protected override void OnSecondaryActionStart()
        {
            HandleActionStart();
        }

        public void OnMiss()
        {

        }

        public override void OnDamageEvent(DamageEvent damageEvent)
        {
            int totalDamage = damageEvent.GetTotal(ResourceValues.Instance.health.listenOn);
            if (damageEvent.hitType == DamageHitType.Dodged || damageEvent.hitType == DamageHitType.Blocked)
            {
                ListActionBundle listBundle = new();
                listBundle.Bundles.Add(new CombatLogProcessor()
                {
                    message = toolManager.gameObject.name + " " + (damageEvent.hitType == DamageHitType.Dodged ? "doged" : "blocked") + " the attack!",
                });
                DamageTextProcessor dtp = new DamageTextProcessor()
                {
                    location = toolManager.gameObject.transform,
                    damageTextPrefab = PoolManager.Instance.damageText
                };
                if (damageEvent.hitType == DamageHitType.Dodged)
                {
                    dtp.message = "Miss";
                    dtp.big = false;
                }
                else
                {
                    dtp.message = "Block";
                    dtp.big = false;
                }
                listBundle.Bundles.Add(dtp);

                EnemyPartyHolder.Instance.enemyPartyManager.GetCurrentBattleContainer().AddProcesor(
                    CombatProcessorTypes.Instance.SUPPORTING_ACTION,
                    listBundle
                );
            }
            else if (totalDamage > 0)
            {
                ListActionBundle listBundle = new ListActionBundle();
                listBundle.Bundles.Add(new CombatLogProcessor()
                {
                    message = toolManager.gameObject.name + " suffered " + totalDamage + " damage!",
                });
                listBundle.Bundles.Add(new DoTweenObjectProcessor()
                {
                    tween = damageTakenTween,
                });
                DamageTextProcessor dtp = new DamageTextProcessor()
                {
                    location = toolManager.gameObject.transform,
                    damageTextPrefab = PoolManager.Instance.damageText
                };
                if (damageEvent.hitType == DamageHitType.Crit)
                {
                    dtp.message = totalDamage + "!";
                    dtp.big = true;
                }
                else
                {
                    dtp.message = totalDamage + "";
                    dtp.big = false;
                }
                listBundle.Bundles.Add(dtp);

                EnemyPartyHolder.Instance.enemyPartyManager.GetCurrentBattleContainer().AddProcesor(
                    CombatProcessorTypes.Instance.SUPPORTING_ACTION, listBundle);
                //PoolableDamageText dtPool = PoolManager.Instance.GetPoolManager(PoolManager.Instance.damageText).GetObject() as PoolableDamageText;
                //dtPool.transform.position = toolManager.transform.position;
                //ExecuteInputState.Instance.AddProcess(new DoTweenAnimationProcessor()
                //{
                //    tween = dtPool.mover,
                //});
                //ExecuteInputState.Instance.AddProcess(new DoTweenObjectProcessor()
                //{
                //    tween = dtPool.fader.tween,
                //});
            }
        }

        public override void OnFacultyChange(Faculty faculty, bool value)
        {

        }

        public override void OnThresholdEvent(ThresholdEventValue value)
        {
            //if (value.resourceValue == ResourceValues.Instance.health)
            //{
            //    int totalDamageTaken = value.previousValue - value.currentValue;
            //    if (value.damageTaken)
            //    {
            //        ListActionBundle listBundle = new ListActionBundle();
            //        listBundle.Bundles.Add(new CombatLogProcessor()
            //        {
            //            message = toolManager.gameObject.name + " suffered " + totalDamageTaken + " damage!",
            //        });
            //        listBundle.Bundles.Add(new DoTweenObjectProcessor()
            //        {
            //            tween = damageTakenTween,
            //        });
            //        DamageTextProcessor dtp = new DamageTextProcessor()
            //        {
            //            location = toolManager.gameObject.transform,
            //            damageTextPrefab = PoolManager.Instance.damageText
            //        };
            //        //if (damageEvent.hitType == DamageHitType.Crit)
            //        //{
            //        //    dtp.message = damageEvent.damageAmount + "!";
            //        //    dtp.big = true;
            //        //}
            //        //else
            //        //{
            //        dtp.message = totalDamageTaken + "";
            //        dtp.big = false;
            //        //}
            //        listBundle.Bundles.Add(dtp);

            //        EnemyPartyHolder.Instance.enemyPartyManager.GetCurrentBattleContainer().AddProcesor(
            //            CombatProcessorTypes.Instance.SUPPORTING_ACTION, listBundle);
            //        //PoolableDamageText dtPool = PoolManager.Instance.GetPoolManager(PoolManager.Instance.damageText).GetObject() as PoolableDamageText;
            //        //dtPool.transform.position = toolManager.transform.position;
            //        //ExecuteInputState.Instance.AddProcess(new DoTweenAnimationProcessor()
            //        //{
            //        //    tween = dtPool.mover,
            //        //});
            //        //ExecuteInputState.Instance.AddProcess(new DoTweenObjectProcessor()
            //        //{
            //        //    tween = dtPool.fader.tween,
            //        //});
            //    }
            //}
        }

        public override void Recalculate(Faculty enumValue, I_DeliveryTool deliveryTool)
        {
            FacultyTool fTool = toolManager.GetComponent<FacultyTool>();
            bool value = fTool.Can(enumValue);
            if (enumValue == Faculties.Instance.CONSCIOUS && !value)
            {
                SpriteRenderer renderer = toolManager.gameObject.GetComponent<SpriteRenderer>();
                ListActionBundle listActionBundle = new ListActionBundle();
                listActionBundle.Bundles.Add(new BlockingProcessor());
                listActionBundle.Bundles.Add(
                    new DisableSpriteProcessor()
                    {
                        spriteRenderer = renderer
                    }
                );
                EnemyPartyHolder.Instance.enemyPartyManager.GetCurrentBattleContainer().AddInturruptProcessor(CombatProcessorTypes.Instance.COMBAT_ACTION, listActionBundle);
            }
        }
    }
}