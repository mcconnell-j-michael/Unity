using Ashen.PartySystem;
using System.Collections.Generic;

namespace Ashen.CombatSystem
{
    public class EnemyPartyUIManager : A_PartyUIManager
    {
        protected override void Start()
        {
            EnemyPartyHolder.Instance.RegisterPartyUIManager(this);
            PartyPositionManager[] positionManagers = GetComponentsInChildren<PartyPositionManager>();
            positionToManager = new Dictionary<PartyPosition, A_CharacterSelector>();

            foreach (PartyPositionManager positionManager in positionManagers)
            {
                PartyPosition pos = PartyPositions.Instance.GetPartyPosition(positionManager.partyRow, positionManager.partyColumn);
                A_CharacterSelector selector = positionManager.GetComponent<A_CharacterSelector>();
                positionToManager.Add(pos, selector);
            }
            base.Start();
        }

        protected override A_PartyManager GetPartyManager()
        {
            return EnemyPartyHolder.Instance.enemyPartyManager;
        }
    }
}