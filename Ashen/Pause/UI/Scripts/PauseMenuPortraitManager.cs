using UnityEngine;
using UnityEngine.UI;

namespace Ashen.PauseSystem
{
    public class PauseMenuPortraitManager : MonoBehaviour
    {
        [SerializeField]
        private Image spriteImage;

        public void UpdateSpriteImage(Sprite sprite)
        {
            spriteImage.sprite = sprite;
        }
    }
}