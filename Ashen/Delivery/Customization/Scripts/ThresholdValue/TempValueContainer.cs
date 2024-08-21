namespace Ashen.DeliverySystem
{
    public class TempValueContainer
    {
        private int tempValue;
        public int TempValue { get { return tempValue; } set { tempValue = value; } }
        public TempValueContainer(int tempValue)
        {
            this.tempValue = tempValue;
        }
    }
}