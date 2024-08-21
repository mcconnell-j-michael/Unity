using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class TagCondition : I_TagOperation
    {
        [HideLabel, FoldoutGroup("Condition"), InlineProperty]
        public I_TagConditional tagConditional;

        [HideLabel, FoldoutGroup("Operation"), InlineProperty]
        public I_TagOperation operation;

        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            if (tagConditional.Check(owner, target, deliveryArguments))
            {
                operation.Operate(owner, target, tagState, deliveryArguments);
            }
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "if(" + tagConditional.visualize() + ")\n";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "{\n" + operation.visualize(depth + 1) + "\n";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "}";
            return visualization;
        }
    }
}