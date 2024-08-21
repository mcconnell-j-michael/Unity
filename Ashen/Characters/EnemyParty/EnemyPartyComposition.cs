using Ashen.CombatSystem;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPartyComposition : SerializedScriptableObject
{
    public Dictionary<PartyPosition, GameObject> partyComposition;
    public List<CombatChecker> combatCheckers;
}
