namespace Ashen.StartMenuSystem
{
    public class StartMenuManager : SingletonMonoBehaviour<StartMenuManager>
    {
        private StartMenuOptionsManager startMenuOptionsManager;

        private void Start()
        {
            startMenuOptionsManager = GetComponentInChildren<StartMenuOptionsManager>(true);
        }

        public StartMenuOptionsManager StartMenuOptionsManager
        {
            get
            {
                return startMenuOptionsManager;
            }
        }
    }
}