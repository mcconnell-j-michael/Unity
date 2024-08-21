using Ashen.ToolSystem;
using System.Collections.Generic;

namespace Ashen.NodeTreeSystem
{
    public interface I_NodeTreeManager
    {
        List<List<NodeUIContainer>> GetNodeTreeDefinition();
        NodeIncreaseRequestResponse RequestSkillNodeIncrease(Node node);
        bool HasSkillNode(Node node);
        int GetCurrentLevel(Node node);
        int GetTotalPoints();
        int GetTierLevel(Node node);
        bool RequirementsMet(Node node);
        bool CanIncreaseSkillNode(Node node);
        bool IsNodeMax(Node node);
        string GetName();
    }
}