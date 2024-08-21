using Ashen.CombatSystem;

namespace Ashen.PartySystem
{
    public class EnemyPartyHolder : SingletonMonoBehaviour<EnemyPartyHolder>
    {
        public EnemyPartyManager enemyPartyManager { get; private set; }
        private EnemyPartyUIManager enemyPartyUIManager;

        public void RegisterPartyUIManager(EnemyPartyUIManager uiManager)
        {
            if (enemyPartyManager != null)
            {
                enemyPartyManager.RegisterPartyUIManager(uiManager);
            }
            else
            {
                enemyPartyUIManager = uiManager;
            }
        }

        public void RegisterEnemyPartyManager(EnemyPartyManager manager)
        {
            enemyPartyManager = manager;
            if (enemyPartyUIManager != null)
            {
                manager.RegisterPartyUIManager(enemyPartyUIManager);
                enemyPartyUIManager = null;
            }
        }
    }
}