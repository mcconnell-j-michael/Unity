namespace Ashen.DeliverySystem
{
    public class ExtendedEffectArgumentsPack : A_DeliveryArgumentPack<ExtendedEffectArgumentsPack>
    {
        private bool isTemp;
        private float[] extendedEffectFloatArguments;

        public ExtendedEffectArgumentsPack()
        {
            extendedEffectFloatArguments = new float[ExtendedEffectArguments.Count];
        }

        public float GetFloatArgument(ExtendedEffectArgument argument)
        {
            return extendedEffectFloatArguments[(int)argument];
        }

        public int GetFloatArgumentFlat(ExtendedEffectArgument argument)
        {
            return (int)extendedEffectFloatArguments[(int)argument];
        }

        public void SetFloatArgument(ExtendedEffectArgument argument, float value)
        {
            extendedEffectFloatArguments[(int)argument] = value;
        }

        public bool IsTemp()
        {
            return isTemp;
        }

        public void SetTemp(bool value)
        {
            isTemp = value;
        }

        public void CopyFloatArguments(ExtendedEffectArgumentsPack other)
        {
            foreach (ExtendedEffectArgument arg in ExtendedEffectArguments.Instance)
            {
                SetFloatArgument(arg, other.extendedEffectFloatArguments[(int)arg]);
            }
        }

        public override I_DeliveryArgumentPack Initialize()
        {
            return new ExtendedEffectArgumentsPack();
        }

        public override void Clear()
        {
            foreach (ExtendedEffectArgument argument in ExtendedEffectArguments.Instance)
            {
                extendedEffectFloatArguments[(int)argument] = 0;
            }
        }

        public override void CopyInto(I_DeliveryArgumentPack pack)
        {
            ExtendedEffectArgumentsPack eap = pack as ExtendedEffectArgumentsPack;
            eap.CopyFloatArguments(this);

        }

    }

}
