using UnityEngine;

namespace Ashen.ToolSystem
{
    public class PortraitTool : A_ConfigurableTool<PortraitTool, PortraitToolConfiguration>
    {
        private Sprite pauseScreenPortrait;

        public override void Initialize()
        {
            this.pauseScreenPortrait = Config.PauseScreenPortrait;
        }

        public Sprite getPauseScreenPortrait()
        {
            return this.pauseScreenPortrait;
        }
    }
}