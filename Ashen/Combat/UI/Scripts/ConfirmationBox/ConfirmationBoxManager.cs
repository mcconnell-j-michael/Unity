using TMPro;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class ConfirmationBoxManager : SingletonMonoBehaviour<ConfirmationBoxManager>
    {
        [SerializeField]
        private GameObject background;
        [SerializeField]
        private TextMeshProUGUI dialogueText;
        [SerializeField]
        private ConfirmationBoxButton noButton;
        [SerializeField]
        private ConfirmationBoxButton yesButton;

        private ConfirmationBoxButton currentButton;


        public void Initialize(string text, ConfirmationButtonValue confirmationButtonValue = ConfirmationButtonValue.NO)
        {
            background.SetActive(true);
            Select(confirmationButtonValue);
            dialogueText.text = text;
        }

        public void SelectNext()
        {
            if (currentButton == yesButton)
            {
                Select(ConfirmationButtonValue.NO);
            }
            else
            {
                Select(ConfirmationButtonValue.YES);
            }
        }

        public void Select(ConfirmationButtonValue confirmationButtonValue)
        {
            if (confirmationButtonValue == ConfirmationButtonValue.YES)
            {
                noButton.Deselect();
                yesButton.Select();
                currentButton = yesButton;
            }
            else
            {
                yesButton.Deselect();
                noButton.Select();
                currentButton = noButton;
            }
        }

        public ConfirmationButtonValue Submit()
        {
            if (currentButton == yesButton)
            {
                return ConfirmationButtonValue.YES;
            }
            return ConfirmationButtonValue.NO;
        }

        public void DeInitialize()
        {
            dialogueText.text = "";
            background.SetActive(false);
        }
    }



    public enum ConfirmationButtonValue
    {
        YES, NO
    }
}