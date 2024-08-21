using UnityEngine;

namespace Ashen.ToolSystem
{
    public class PortraitToolConfiguration : A_Configuration<PortraitTool, PortraitToolConfiguration>
    {
        [SerializeField]
        private Sprite pauseScreenPortrait;

        public Sprite PauseScreenPortrait
        {
            get
            {
                return pauseScreenPortrait;
            }
        }
    }
}