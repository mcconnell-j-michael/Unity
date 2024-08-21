using Ashen.ToolSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Ashen.PauseSystem
{
    public class CombatPortraitManager : SingletonMonoBehaviour<CombatPortraitManager>
    {
        [SerializeField]
        private Image spriteImage;
        /*        [SerializeField]
                private DOTweenAnimation intro;
                [SerializeField]
                private DOTweenAnimation outro;*/

        /*   private Tween tweenIntro;
           private Tween tweenOutro;*/

        [SerializeField]
        private RectTransform transformToMove;
        [SerializeField]
        private float xPosActive;
        [SerializeField]
        private float xPosInactive;
        [SerializeField]
        private float transitionDuration;

        private bool active = false;
        private ToolManager toolManager;

        private void Start()
        {
            transformToMove.DOAnchorPosX(xPosInactive, 0.01f);
            active = false;
        }

        public float RegisterToolManager(ToolManager toolManager)
        {
            if (toolManager == this.toolManager)
            {
                return 0f;
            }
            float delay = UnRegisterToolManager();
            if (toolManager == null)
            {
                return delay;
            }
            this.toolManager = toolManager;
            PortraitTool pt = this.toolManager.Get<PortraitTool>();
            spriteImage.sprite = pt.getPauseScreenPortrait();
            return delay;
        }

        public float UnRegisterToolManager()
        {
            if (toolManager == null)
            {
                return 0f;
            }
            float delay = HideSprite(true);
            this.toolManager = null;
            return delay;
        }

        public float DisplaySprite(bool delay = true)
        {
            float timeDelay = 0f;
            if (!active)
            {
                timeDelay = Transition(xPosActive, delay);
            }
            active = true;
            return timeDelay;
        }

        public float HideSprite(bool delay = true)
        {
            float timeDelay = 0f;
            if (active)
            {
                timeDelay = Transition(xPosInactive, delay);
            }
            active = false;
            return timeDelay;
        }

        public bool IsRegistered(ToolManager toolManager)
        {
            return toolManager == this.toolManager;
        }

        private float Transition(float xPos, bool delay)
        {
            transformToMove.DOAnchorPosX(xPos, transitionDuration);
            if (delay)
            {
                return transitionDuration;
            }
            return 0f;
        }
    }
}