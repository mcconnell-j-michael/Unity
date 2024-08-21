using Ashen.DeliverySystem;
using Ashen.StateMachineSystem;
using System;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class CombatTool : A_ConfigurableTool<CombatTool, CombatToolConfiguration>, I_ThresholdListener
    {
        private List<ActionPointUpdateValue> actionState;

        private ActionPointUpdateValue currentUpdateValue;

        private List<I_ActionPointChangeListener> listeners;

        public override void Initialize()
        {
            base.Initialize();
            actionState = new List<ActionPointUpdateValue>();
            listeners = new List<I_ActionPointChangeListener>();
            currentUpdateValue = new()
            {
                commitments = new List<I_CharacterCommitment>()
            };
        }

        private void Start()
        {
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            rvTool.RegiserThresholdChangeListener(ResourceValues.Instance.ACTION_POINT, this);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (!toolManager)
            {
                return;
            }
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            if (!rvTool)
            {
                return;
            }
            rvTool.UnRegesterThresholdChangeListener(ResourceValues.Instance.ACTION_POINT, this);
        }

        public void FinalizeAndClear()
        {
            foreach (ActionPointUpdateValue updateValue in actionState)
            {
                foreach (I_CharacterCommitment commitment in updateValue.commitments)
                {
                    commitment.Finalize(toolManager);
                }
            }
            Clear();
        }

        public void Clear()
        {
            ClearCurrent();
            actionState.Clear();
            NotifyChange();
        }

        public void SetActionCost(int index, int actionPointCost, string name = "")
        {
            try
            {
                ActionPointUpdateValue updateValue = GetUpdateValue(index);
                updateValue.actionPointCost = actionPointCost;
                updateValue.name = name;
                NotifyChange();
            }
            catch (Exception e)
            {
                Logger.ErrorLog(e.Message);
            }
        }

        public void RemoveAction(int index)
        {
            if (index < 0 || index >= actionState.Count)
            {
                return;
            }
            ActionPointUpdateValue foundUpdateValue = actionState[index];
            foreach (I_CharacterCommitment commitment in foundUpdateValue.commitments)
            {
                commitment.Rollback(toolManager);
            }
            actionState.RemoveAt(index);
            for (int x = 0; x < actionState.Count; x++)
            {
                foreach (I_CharacterCommitment commitment in actionState[x].commitments)
                {
                    commitment.UpdateActionCount(x);
                }
            }
            NotifyChange();
        }

        public void AddCommitment(I_CharacterCommitment commitment)
        {
            currentUpdateValue.commitments.Add(commitment);
        }

        public void RemoveLastCommitment()
        {
            if (currentUpdateValue.commitments.Count > 0)
            {
                currentUpdateValue.commitments.RemoveAt(currentUpdateValue.commitments.Count - 1);
            }
        }

        public void AddListener(I_ActionPointChangeListener listener)
        {
            listeners.Add(listener);
            NotifyChange();
        }

        public void RemoveListener(I_ActionPointChangeListener listener)
        {
            listeners.Remove(listener);
        }

        public ActionPointUpdateValue GetUpdateValue(int index)
        {
            if (index < 0 || index > actionState.Count)
            {
                throw new Exception("Index Out Of Bounds");
            }
            if (index == actionState.Count)
            {
                return currentUpdateValue;
            }
            return actionState[index];
        }

        public void Commit()
        {
            foreach (I_CharacterCommitment commitment in currentUpdateValue.commitments)
            {
                commitment.OnCommit(toolManager);
            }
            actionState.Add(new ActionPointUpdateValue()
            {
                name = currentUpdateValue.name,
                actionPointCost = currentUpdateValue.actionPointCost,
                commitments = new List<I_CharacterCommitment>(currentUpdateValue.commitments)
            });
            currentUpdateValue.name = null;
            currentUpdateValue.actionPointCost = 0;
            currentUpdateValue.commitments.Clear();
            NotifyChange();
        }

        public void Rollback()
        {
            foreach (ActionPointUpdateValue updateValue in actionState)
            {
                foreach (I_CharacterCommitment commitment in updateValue.commitments)
                {
                    commitment.Rollback(toolManager);
                }
            }
            actionState.Clear();
            ClearCurrent();
            NotifyChange();
        }

        public void Rollback(int index)
        {
            if (index < 0 || index > actionState.Count)
            {
                return;
            }
            if (index == actionState.Count)
            {
                ClearCurrent();
                return;
            }
            foreach (I_CharacterCommitment commitment in actionState[index].commitments)
            {
                commitment.Rollback(toolManager);
            }
            actionState.RemoveAt(index);
            NotifyChange();
        }

        public void Select(int index)
        {
            for (int x = 0; x < actionState.Count; x++)
            {
                actionState[x].selected = false;
                if (x == index)
                {
                    actionState[x].selected = true;
                }
            }
            NotifyChange();
        }

        public int Count()
        {
            return actionState.Count;
        }

        public void DeselectAll()
        {
            for (int x = 0; x < actionState.Count; x++)
            {
                actionState[x].selected = false;
            }
            NotifyChange();
        }

        private void ClearCurrent()
        {
            currentUpdateValue.name = null;
            currentUpdateValue.commitments.Clear();
            currentUpdateValue.actionPointCost = 0;
        }

        private void NotifyChange()
        {
            ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            ThresholdEventValue value = rvTool.GetValue(ResourceValues.Instance.ACTION_POINT);
            int currentValue = rvTool.CalculateLimit(ResourceValues.Instance.ACTION_POINT, value.tempValues[(int)ThresholdValueTempCategories.Instance.PROMISED]);
            int preview = value.tempValues[(int)ThresholdValueTempCategories.Instance.PREVIEW];
            bool previewValid = (currentValue + preview) <= value.maxValue;
            ActionPointsUpdateValue change = new ActionPointsUpdateValue()
            {
                actionPointUpdates = new List<ActionPointUpdateValue>(actionState),
                maxResourceValue = value.maxValue,
                currentResourceValue = currentValue,
                preview = preview,
                previewValid = previewValid
            };
            foreach (I_ActionPointChangeListener listener in listeners)
            {
                listener.OnActionPointChange(change);
            }
        }

        public void OnThresholdEvent(ThresholdEventValue value)
        {
            NotifyChange();
        }
    }
}