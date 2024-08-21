using Ashen.AbilitySystem;
using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.StateMachineSystem
{
    public class BattleContainer
    {
        private List<I_CombatProcessor>[] processorsByType;

        private List<ActionProcessor>[] primaryActionPerCategory;

        private List<CombatChecker>[] combatCheckerByType;

        public BattleContainer()
        {
            processorsByType = new List<I_CombatProcessor>[CombatProcessorTypes.Count];
            combatCheckerByType = new List<CombatChecker>[CombatProcessorTypes.Count];
            foreach (CombatProcessorType type in CombatProcessorTypes.Instance)
            {
                processorsByType[(int)type] = new List<I_CombatProcessor>();
                combatCheckerByType[(int)type] = new List<CombatChecker>();
            }
            primaryActionPerCategory = new List<ActionProcessor>[AbilitySpeedCategories.Count];
            foreach (AbilitySpeedCategory category in AbilitySpeedCategories.Instance)
            {
                primaryActionPerCategory[(int)category] = new List<ActionProcessor>();
            }
        }

        public void AddPrimaryAction(ActionProcessor actionHolder)
        {
            AbilitySpeedCategory speedCategory = actionHolder.speedCategory;
            List<ActionProcessor> primaryActions = primaryActionPerCategory[(int)speedCategory];
            if (speedCategory.useSpeedCalculation)
            {
                for (int x = 0; x < primaryActions.Count; x++)
                {
                    if (actionHolder.GetSpeed() > primaryActions[x].GetSpeed())
                    {
                        primaryActions.Insert(x, actionHolder);
                        return;
                    }
                }
            }
            primaryActions.Add(actionHolder);
        }

        public void AddInturruptProcessor(CombatProcessorType type, I_CombatProcessor processor)
        {
            processorsByType[(int)type].Insert(0, processor);
        }

        public void AddInturruptProcessors(CombatProcessorType type, List<I_CombatProcessor> processors)
        {
            processorsByType[(int)type].InsertRange(0, processors);
        }

        public void AddProcesor(CombatProcessorType type, I_CombatProcessor processor)
        {
            processorsByType[(int)type].Add(processor);
        }

        public void RegisterCombatChecker(CombatProcessorType type, CombatChecker checker)
        {
            combatCheckerByType[(int)type].Add(checker);
        }

        public List<I_CombatProcessor> GetProcessors(CombatProcessorType type)
        {
            return processorsByType[(int)type];
        }

        public List<CombatChecker> GetCombatCheckers(CombatProcessorType type)
        {
            return combatCheckerByType[(int)type];
        }

        public void PopulateByCategory(AbilitySpeedCategory abilitySpeedCategory)
        {
            processorsByType[(int)CombatProcessorTypes.Instance.PRIMARY_ACTION].AddRange(primaryActionPerCategory[(int)abilitySpeedCategory]);
        }

        public void ClearProcessors(ToolManager toolManager, int actionCount)
        {
            foreach (CombatProcessorType type in CombatProcessorTypes.Instance)
            {
                List<I_CombatProcessor> processors = processorsByType[(int)type];
                for (int x = 0; x < processors.Count; x++)
                {
                    if (processors[x].GetOwner() == toolManager && processors[x].GetActionCount() == actionCount)
                    {
                        processors.RemoveAt(x);
                        x--;
                    }
                }
            }
            foreach (AbilitySpeedCategory category in AbilitySpeedCategories.Instance)
            {
                List<ActionProcessor> primaryActions = primaryActionPerCategory[(int)category];
                for (int x = 0; x < primaryActions.Count; x++)
                {
                    if (primaryActions[x].source == toolManager && primaryActions[x].GetActionCount() == actionCount)
                    {
                        primaryActions.RemoveAt(x);
                        x--;
                    }
                }
            }
        }

        public void ClearProcessors(ToolManager toolManager)
        {
            foreach (CombatProcessorType type in CombatProcessorTypes.Instance)
            {
                List<I_CombatProcessor> processors = processorsByType[(int)type];
                for (int x = 0; x < processors.Count; x++)
                {
                    if (processors[x].GetOwner() == toolManager)
                    {
                        processors.RemoveAt(x);
                        x--;
                    }
                }
            }
            foreach (AbilitySpeedCategory category in AbilitySpeedCategories.Instance)
            {
                List<ActionProcessor> primaryActions = primaryActionPerCategory[(int)category];
                for (int x = 0; x < primaryActions.Count; x++)
                {
                    if (primaryActions[x].source == toolManager)
                    {
                        primaryActions.RemoveAt(x);
                        x--;
                    }
                }
            }
        }

        public void ClearProcessors()
        {
            foreach (CombatProcessorType type in CombatProcessorTypes.Instance)
            {
                ClearProcessors(type);
            }
            foreach (AbilitySpeedCategory category in AbilitySpeedCategories.Instance)
            {
                List<ActionProcessor> primaryActions = primaryActionPerCategory[(int)category];
                primaryActions.Clear();
            }
        }

        public void ClearProcessors(CombatProcessorType type)
        {
            processorsByType[(int)type].Clear();
        }

        public bool HasProcessors(CombatProcessorType type, CombatProcessorInfo info)
        {
            foreach (I_CombatProcessor processor in processorsByType[(int)type])
            {
                if (!processor.IsFinished(info))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            ClearProcessors();
        }
    }
}