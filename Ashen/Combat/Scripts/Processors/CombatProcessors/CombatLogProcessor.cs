using System.Collections;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class CombatLogProcessor : A_CombatProcessor
    {
        public string message;

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            CombatLog.Instance.AddMessage(message);
            info.runner.StartCoroutine(DisplayWait());
            isValid = false;
            yield break;
        }

        private IEnumerator DisplayWait()
        {
            yield return new WaitForSeconds(1f);
            isFinished = true;
        }
    }
}