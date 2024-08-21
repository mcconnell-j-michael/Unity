using JoshH.UI;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class ConfirmationBoxButton : MonoBehaviour
    {
        [SerializeField]
        private UIGradient borderHighlight;

        public void Select()
        {
            borderHighlight.enabled = true;
        }

        public void Deselect()
        {
            borderHighlight.enabled = false;
        }
    }
}