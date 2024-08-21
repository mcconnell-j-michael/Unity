using Ashen.ToolSystem;

namespace Ashen.CombatSystem
{
    public class MissingFacultyRow : I_CombatCondition
    {
        public Faculty faculty;
        public PartyRow row;
        public bool enemy;

        public bool ConditionMet(A_PartyManager playerParty, A_PartyManager enemyParty)
        {
            A_PartyManager partyManager = (enemy ? enemyParty : playerParty);
            foreach (PartyPosition position in partyManager.enabledPositions)
            {
                if (position.partyRow != row)
                {
                    continue;
                }
                ToolManager manager = partyManager.GetToolManager(position);
                if (manager)
                {
                    FacultyTool fTool = manager.Get<FacultyTool>();
                    if (fTool.Can(faculty))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}