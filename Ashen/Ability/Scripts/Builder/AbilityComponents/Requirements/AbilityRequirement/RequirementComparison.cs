using Sirenix.OdinInspector;

namespace Ashen.AbilitySystem
{
    public enum RequirementComparison
    {
        [LabelText(">")] GT,
        [LabelText("<")] LT,
        [LabelText("=")] EQ,
        [LabelText("!=")] NOT_EQ,
        [LabelText("<=")] LT_EQ,
        [LabelText(">=")] GT_EQ
    }
}
