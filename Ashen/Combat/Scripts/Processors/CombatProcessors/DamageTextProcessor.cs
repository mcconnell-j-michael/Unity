using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class DamageTextProcessor : A_CombatProcessor
    {
        public string message;
        public bool big;
        public Transform location;
        public GameObject parent;
        public GameObject damageTextPrefab;
        private PoolableDamageText dtPool;

        public override IEnumerator Execute(CombatProcessorInfo info)
        {
            dtPool = PoolManager.Instance.GetPoolManager(damageTextPrefab, parent).GetObject() as PoolableDamageText;
            dtPool.mover.tween.Rewind();
            dtPool.fader.tween.Rewind();
            dtPool.transform.position = location.position;
            dtPool.transform.localScale = Vector3.Scale(dtPool.transform.localScale, location.lossyScale);
            dtPool.text.text = message;
            yield return null;
            dtPool.mover.tween.Play();
            dtPool.fader.tween.Play();
            isValid = false;
            yield break;
        }

        public override bool IsFinished(CombatProcessorInfo info)
        {
            bool isFinished = !dtPool.mover.tween.IsPlaying() && !dtPool.fader.tween.IsPlaying();
            if (isFinished)
            {
                dtPool.Disable();
            }
            return isFinished;
        }
    }
}