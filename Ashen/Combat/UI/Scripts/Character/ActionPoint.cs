using UnityEngine;
using UnityEngine.UI;

namespace Ashen.CombatSystem
{
    public class ActionPoint : MonoBehaviour
    {
        [SerializeField]
        private Color32 enabledColor;
        [SerializeField]
        private Color32 disabledColor;
        [SerializeField]
        private Color32 promisedColor;
        [SerializeField]
        private Color32 selectedColor;
        [SerializeField]
        private Image actionPointImage;
        [SerializeField]
        private GameObject aura;

        public void EnableActionPoint()
        {
            actionPointImage.color = enabledColor;
        }

        public void PromiseActionPoint()
        {
            actionPointImage.color = promisedColor;
        }

        public void DisableActionPoint()
        {
            actionPointImage.color = disabledColor;
        }

        public void PreviewActionPoint()
        {
            actionPointImage.color = selectedColor;
        }

        public void SelectActionPoint()
        {
            aura.SetActive(true);
        }

        public void DeselectActionPoint()
        {
            aura.SetActive(false);
        }
    }
}