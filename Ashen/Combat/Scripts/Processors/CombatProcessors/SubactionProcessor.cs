using Ashen.DeliverySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using System.Collections;

namespace Ashen.CombatSystem
{
    public class SubactionProcessor : A_CombatProcessor
    {
        public ActionExecutable actionExecutable;
        public AnimationExecutable animationExecutable;

        private ToolManager lastTarget = null;

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            ExecuteInputState.Instance.currentSubactionProcessor = this;
            TriggerTool triggerTool;
            if (lastTarget != actionExecutable.target)
            {
                lastTarget = actionExecutable.target;
                triggerTool = actionExecutable.target.Get<TriggerTool>();
                triggerTool.Trigger(ExtendedEffectTriggers.Instance.Targeted);
                yield break;
            }
            if (animationExecutable != null)
            {
                yield return animationExecutable.Execute(info.runner);
            }
            yield return actionExecutable.Execute(info.runner);
            triggerTool = actionExecutable.target.Get<TriggerTool>();
            triggerTool.Trigger(ExtendedEffectTriggers.Instance.Effected);
            isValid = false;
        }

        public override bool IsFinished(CombatProcessorInfo info)
        {
            if (!actionExecutable.IsFinished())
            {
                return false;
            }
            if (animationExecutable != null && !animationExecutable.IsFinished())
            {
                return false;
            }
            return true;
        }
    }
}