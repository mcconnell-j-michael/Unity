using UnityEngine;
using Sirenix.OdinInspector;

namespace Ashen.GameManagerWindow
{
    [CreateAssetMenu(fileName = nameof(GameManagerState), menuName = "Custom/GameManager/" + nameof(GameManagerState))]
    public class GameManagerState : SerializedScriptableObject
    {
        public string menuName;
        public A_GameManagerOption gameManagerOption;

        public override string ToString()
        {
            if (menuName != null && "" != menuName)
            {
                return menuName;
            }
            if (gameManagerOption.GetName() != null)
            {
                return gameManagerOption.GetName();
            }
            return name;
        }
    }
}