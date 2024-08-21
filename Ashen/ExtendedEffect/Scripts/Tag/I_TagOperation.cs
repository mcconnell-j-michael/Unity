namespace Ashen.DeliverySystem
{
    public interface I_TagOperation
    {
        void Operate(I_DeliveryTool owner, I_DeliveryTool target, TagState tagState, DeliveryArgumentPacks deliveryArguments);
        string visualize(int depth);

        public string AddDepth(int depth)
        {
            string visualization = "";
            for (int x = 0; x < depth; x++)
            {
                visualization += "\t";
            }
            return visualization;
        }
    }
}