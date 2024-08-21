using Ashen.AbilitySystem;
using Ashen.PartySystem;
using Sirenix.OdinInspector;

namespace Ashen.CombatSystem
{
    public class EnemyTargetableRegister : SerializedMonoBehaviour
    {
        public I_Targetable targetable;
        public EnemyPartyManager enemyPartyManager;
        public PartyPosition position;

        //private void Start()
        //{
        //enemyPartyManager.SetTargetable(position, targetable);
        //}
    }
}