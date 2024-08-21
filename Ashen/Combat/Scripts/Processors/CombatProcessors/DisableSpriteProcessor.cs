using System.Collections;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class DisableSpriteProcessor : A_CombatProcessor
    {
        public SpriteRenderer spriteRenderer;

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            spriteRenderer.enabled = false;
            isFinished = true;
            yield return null;
        }

        public override bool IsValid(CombatProcessorInfo info)
        {
            return !isFinished;
        }
    }
}