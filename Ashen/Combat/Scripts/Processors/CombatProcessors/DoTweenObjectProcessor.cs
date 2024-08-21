using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class DoTweenObjectProcessor : A_CombatProcessor
    {
        public Tween tween;
        public float waitTime;

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            tween.Rewind();
            tween.Play();
            yield return new WaitForSeconds(waitTime);
            isValid = false;
        }

        public override bool IsFinished(CombatProcessorInfo info)
        {
            return !tween.IsPlaying();
        }
    }
}