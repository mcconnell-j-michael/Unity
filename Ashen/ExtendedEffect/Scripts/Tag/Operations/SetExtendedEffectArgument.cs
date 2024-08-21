namespace Ashen.DeliverySystem
{
    public class SetExtendedEffectArgument : I_TagOperation
    {
        public ExtendedEffectArgument argument;
        public I_DeliveryValue value;

        public void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments)
        {
            ExtendedEffectArgumentsPack pack = deliveryArguments.GetPack<ExtendedEffectArgumentsPack>();
            pack.SetFloatArgument(argument, value.Build(owner, target, deliveryArguments));
        }

        public string visualize(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            visualization += "Set " + argument + " to " + value.Visualize();
            return visualization;
        }
    }
}
