using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    [InlineProperty]
    public class TagConditionalOperator : I_TagConditional
    {
        [HideLabel, HorizontalGroup]
        public I_TagConditional left;
        [HideLabel, HorizontalGroup]
        [ValueDropdown("@Enum.GetValues(typeof(" + nameof(OPERAND_TYPE) + "))")]
        public OPERAND_TYPE operandType;
        [HideLabel, HorizontalGroup]
        public I_TagConditional right;

        public bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            switch (operandType)
            {
                case OPERAND_TYPE.AND:
                    return left.Check(owner, target, deliveryArguments) && right.Check(owner, target, deliveryArguments);
                case OPERAND_TYPE.OR:
                    return left.Check(owner, target, deliveryArguments) || right.Check(owner, target, deliveryArguments);
            }
            return false;
        }

        public string visualize()
        {
            return "(" + left.visualize() + " " + operandType.ToString() + " " + right.visualize() + ")";
        }
    }

    public enum OPERAND_TYPE
    {
        AND, OR
    }
}