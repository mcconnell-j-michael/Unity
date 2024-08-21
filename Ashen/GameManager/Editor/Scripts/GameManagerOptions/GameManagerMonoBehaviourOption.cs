using System.Collections.Generic;

namespace Ashen.GameManagerWindow
{
    public class GameManagerMonoBehaviourOption : A_GameManagerUnityObjectOption
    {
        private DrawMonoBehaviour drawMonoBehaviour;

        public override A_GameManagerOption Clone(GameManagerState state)
        {
            GameManagerMonoBehaviourOption option = new GameManagerMonoBehaviourOption();
            option.state = state;
            base.Copy(option);
            option.drawMonoBehaviour = new DrawMonoBehaviour(option);
            return option;
        }

        public override void AddTarget(List<object> targets)
        {
            targets.Add(drawMonoBehaviour);
        }

        public override GameManagerOptionType GetGameManagerOptionType()
        {
            return GameManagerOptionType.MonoBehaviour;
        }
    }
}