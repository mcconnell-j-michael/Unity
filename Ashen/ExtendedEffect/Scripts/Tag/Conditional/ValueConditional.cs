using Sirenix.OdinInspector;

namespace Ashen.DeliverySystem
{
    public class ValueConditional : I_TagConditional
    {
        [HideLabel, HorizontalGroup]
        public I_DeliveryValue left;
        [HideLabel, HorizontalGroup]
        [ValueDropdown("@Enum.GetValues(typeof(" + nameof(EQUALITY_TYPE) + "))")]
        public EQUALITY_TYPE equalityType;
        [HideLabel, HorizontalGroup]
        public I_DeliveryValue right;


        public bool Check(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            float leftValue = left.Build(owner, target, deliveryArguments);
            float rightValue = right.Build(owner, target, deliveryArguments);
            switch (equalityType)
            {
                case EQUALITY_TYPE.GT:
                    return leftValue > rightValue;
                case EQUALITY_TYPE.LT:
                    return leftValue < rightValue;
                case EQUALITY_TYPE.EQ:
                    return ((int)(leftValue * 1000)) == ((int)(rightValue * 1000));
            }

            return false;
        }

        public string visualize()
        {
            return "(" + left.Visualize() + " " + equalityType.ToString() + " " + right.Visualize() + ")";
        }
    }

    public enum EQUALITY_TYPE
    {
        GT, LT, EQ
    }
}
