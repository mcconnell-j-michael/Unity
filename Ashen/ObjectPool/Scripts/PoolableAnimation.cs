using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableAnimation : PoolableBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public string animationName;
    public GameObject mover;

    private List<I_AnimationDisabledListener> animationDisabledEvents;

    public override void Initialize()
    {
        gameObject.SetActive(true);
        enabled = true;
        animator.Play(animationName, -1, 0f);
        StartCoroutine(CheckAnimationStatus());
    }

    private IEnumerator CheckAnimationStatus()
    {
        bool isPlaying = true;
        while (isPlaying)
        {
            yield return null;
            isPlaying = false;
            for (int x = 0; x < animator.layerCount; x++)
            {
                if (animator.GetCurrentAnimatorStateInfo(x).IsName(animationName))
                {
                    isPlaying = true;
                }
            }
        }
        this.Disable();
    }

    protected override void InternalDisable()
    {
        gameObject.SetActive(false);
        enabled = false;
        if (animationDisabledEvents != null)
        {
            foreach (I_AnimationDisabledListener listener in animationDisabledEvents)
            {
                listener.AnimationEndEvent();
            }
            animationDisabledEvents.Clear();
        }
    }

    public override bool Enabled()
    {
        return enabled;
    }

    public void RegisterAnimationDisabledEvent(I_AnimationDisabledListener listener)
    {
        if (animationDisabledEvents == null)
        {
            animationDisabledEvents = new List<I_AnimationDisabledListener>();
        }
        animationDisabledEvents.Add(listener);
    }

    public void MovePosition(Vector3 position)
    {
        if (!mover)
        {
            mover = gameObject;
        }
        mover.transform.position = position;
    }
}
