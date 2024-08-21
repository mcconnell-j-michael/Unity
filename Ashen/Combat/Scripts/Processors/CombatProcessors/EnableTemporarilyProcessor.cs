using System.Collections;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class EnableTemporarilyProcessor : A_CombatProcessor
    {
        public MonoBehaviour toEnable;
        public float totalTime;

        private float currentTIme;

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            toEnable.enabled = true;
            currentTIme = 0;
            isValid = false;
            info.runner.StartCoroutine(Timer());
            yield break;
        }

        public IEnumerator Timer()
        {
            while (currentTIme < totalTime)
            {
                currentTIme += Time.deltaTime;
                yield return null;
            }
            toEnable.enabled = false;
            isFinished = true;
        }
    }
}