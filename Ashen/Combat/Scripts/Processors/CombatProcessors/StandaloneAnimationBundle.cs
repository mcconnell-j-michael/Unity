using System.Collections;

namespace Ashen.CombatSystem
{
    public class StandaloneAnimationBundle : A_CombatProcessor
    {
        public AnimationExecutable animationExecutable;

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            yield return animationExecutable.Execute(info.runner);
            isValid = false;
        }

        public override bool IsFinished(CombatProcessorInfo info)
        {
            if (!animationExecutable.IsFinished())
            {
                return false;
            }
            return true;
        }
    }
}