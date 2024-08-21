using Ashen.ToolSystem;

namespace Ashen.CombatSystem
{
    public class MissingFaculty : I_CombatCondition
    {
        public Faculty faculty;
        public bool enemy;

        public bool ConditionMet(A_PartyManager playerParty, A_PartyManager enemyParty)
        {
            A_PartyManager partyManager = (enemy ? enemyParty : playerParty);
            foreach (PartyPosition position in partyManager.enabledPositions)
            {
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