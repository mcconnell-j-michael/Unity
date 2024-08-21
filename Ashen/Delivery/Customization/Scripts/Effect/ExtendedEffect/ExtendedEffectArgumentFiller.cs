using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class ExtendedEffectArgumentFiller
    {
        private int[] arguments;
        private bool[] filledArguments;

        public ExtendedEffectArgumentFiller()
        {
            this.arguments = new int[ExtendedEffectArguments.Count];
            this.filledArguments = new bool[ExtendedEffectArguments.Count];
        }

        public void FillArguments(DeliveryArgumentPacks deliveryArguments)
        {
            ExtendedEffectArgumentsPack extendedEffectArgumentsPack = deliveryArguments.GetPack<ExtendedEffectArgumentsPack>();
            foreach (ExtendedEffectArgument argument in ExtendedEffectArguments.Instance)
            {
                if (filledArguments[(int)argument])
                {
                    extendedEffectArgumentsPack.SetFloatArgument(argument, arguments[(int)argument]);
                }
            }
        }

        public void SetValue(ExtendedEffectArgument argument, int value)
        {
            filledArguments[(int)argument] = true;
            arguments[(int)argument] = value;
        }

        public ExtendedEffectArgumentFiller(SerializationInfo info, StreamingContext context)
        {
            arguments = (int[])info.GetValue(nameof(arguments), typeof(int[]));
            filledArguments = (bool[])info.GetValue(nameof(filledArguments), typeof(bool[]));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(arguments), arguments);
            info.AddValue(nameof(filledArguments), filledArguments);
        }
    }
}
