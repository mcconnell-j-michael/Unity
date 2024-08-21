using Ashen.UISystem;
using DG.Tweening;
using UnityEngine;

public class CombatOptionUI : A_OptionUI
{
    public GameObject emptySpace;
    public TweenLayoutElement layoutElement;
    private string iconTweenId;

    protected override void Initialize()
    {
        iconTweenId = gameObject.name + "Icon";
    }

    protected override void SelectedInternal()
    {
        title.enabled = true;
    }

    protected override void Reselect()
    {
        DOTween.Restart(iconTweenId, false);
        DOTween.Play(iconTweenId);
    }

    protected override void Select()
    {
        layoutElement.Play();
        ActionOptionsManager.Instance.currentlySelected = this;
    }

    protected override void DeselectedInternal()
    {
        title.enabled = true;
    }

    protected override void Invalidate()
    {
        DOTween.Complete(iconTweenId);
        DOTween.PlayBackwards(iconTweenId);
    }

    protected override void Deslect()
    {
        layoutElement.Rewind();
    }

    public void HideSelected()
    {
        layoutElement.RewindPartial();
        title.enabled = false;
    }

    public void ShowSelected()
    {
        layoutElement.Play();
        title.enabled = true;
    }

    protected override I_OptionsManager GetManager()
    {
        return ActionOptionsManager.Instance;
    }
}
