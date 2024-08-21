using Ashen.PauseSystem;

namespace Assets.Ashen.Pause.UI.Scripts
{
    public class PauseMenuManager : SingletonMonoBehaviour<PauseMenuManager>
    {
        private PauseOptionsManager pauseOptionsManager;
        private PauseMenuPortraitManager pauseMenuPortraitManager;

        private void Start()
        {
            pauseOptionsManager = GetComponentInChildren<PauseOptionsManager>(true);
            pauseMenuPortraitManager = GetComponentInChildren<PauseMenuPortraitManager>(true);
        }

        public PauseOptionsManager PauseOptionManager
        {
            get
            {
                return pauseOptionsManager;
            }
        }

        public PauseMenuPortraitManager PauseMenuPortraitManager
        {
            get
            {
                return pauseMenuPortraitManager;
            }
        }
    }
}