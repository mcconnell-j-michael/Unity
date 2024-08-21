using DG.Tweening;
using UnityEngine;

namespace Ashen.UISystem
{
    public abstract class A_HidableOptionUI : A_OptionUI
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
            if (IsHidden() && IsSelected())
            {
                Reselect();
            }
            title.enabled = true;
        }

        protected override void Reselect()
        {
            layoutElement.Play();
        }

        protected override void Select()
        {
            if (IsHidden())
            {
                ShowSelected();
            }
            if (Valid)
            {
                DOTween.Restart(iconTweenId, false);
                DOTween.Play(iconTweenId);
            }
            layoutElement.Play();
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
            title.enabled = true;
        }

        private bool IsHidden()
        {
            return !title.enabled;
        }

        public override void OnSubmit()
        {
            HideSelected();
        }
    }
}
