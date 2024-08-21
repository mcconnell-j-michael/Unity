using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Ashen.GameManagerWindow
{
    [CreateAssetMenu(fileName = nameof(GameManagerStateCollection), menuName = "Custom/GameManager/" + nameof(GameManagerStateCollection))]
    public class GameManagerStateCollection : SerializedScriptableObject
    {
        public List<GameManagerState> gameManagerStates;

        public int GetIndex(GameManagerState gameManagerState)
        {
            return gameManagerStates.IndexOf(gameManagerState);
        }
    }
}