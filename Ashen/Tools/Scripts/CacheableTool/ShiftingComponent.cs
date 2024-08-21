using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.EquationSystem;
using Ashen.ObjectPoolSystem;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class ShiftingComponent<Enum, Base, Current, Shift> :
        I_Shiftable<Enum, Shift>,
        I_CombinedEnumListener
        where Enum : class, I_EnumSO
    {
        protected A_Shiftable<Base, Current, Shift> shiftableValues;
        protected Dictionary<string, Enum> keyToEnumValue;

        private ToolManager toolManager;
        private I_DeliveryTool deliveryTool;
        private I_DeliveryTool DeliveryTool
        {
            get
            {
                if (deliveryTool == null)
                {
                    deliveryTool = toolManager.Get<DeliveryTool>();
                }
                return deliveryTool;
            }
        }
        private I_EnumChangeListener<Enum> changeListener;

        public ShiftingComponent(
            A_Shiftable<Base, Current, Shift> shiftableValues,
            ToolManager toolManager,
            I_EnumChangeListener<Enum> changeListener,
            IEnumerable<Enum> enumList
        )
        {
            this.shiftableValues = shiftableValues;
            keyToEnumValue = new Dictionary<string, Enum>();
            this.toolManager = toolManager;
            this.changeListener = changeListener;
            foreach (Enum enumValue in enumList)
            {
                keyToEnumValue.Add(enumValue.ToString(), enumValue);
            }
        }

        public void Initialize(string source, IEnumerable<Enum> enumList)
        {
            shiftableValues.Initialize(source, enumList, DeliveryTool, this);
        }

        public void AddShift(Enum enumValue, int priority, ShiftCategory shiftCategory, string source, Shift value)
        {
            shiftableValues.AddShift(DeliveryTool, this, enumValue, priority, shiftCategory, source, value);
            changeListener.OnChange(enumValue);
        }

        public void RemoveShift(Enum enumValue, ShiftCategory shiftCategory, string source)
        {
            shiftableValues.RemoveShift(DeliveryTool, this, enumValue, shiftCategory, source);
            changeListener.OnChange(enumValue);
        }

        public Current GetAttribute(Enum enumValue)
        {
            return Get(enumValue, null);
        }

        public Current Get(Enum enumValue, DeliveryArgumentPacks argument)
        {
            return shiftableValues.Get(enumValue.GetIndex(), toolManager, argument);
        }

        public Current GetAttribute(Enum enumValue, AttributeLimiter limiter)
        {
            DeliveryArgumentPacks deliveryArgumentPacks = AGenericPool<DeliveryArgumentPacks>.Get();
            EquationArgumentPack argument = deliveryArgumentPacks.GetPack<EquationArgumentPack>();
            argument.SetPassthroughAttributeLimiter(limiter);
            Current retValue = Get(enumValue, deliveryArgumentPacks);
            AGenericPool<DeliveryArgumentPacks>.Release(deliveryArgumentPacks);
            return retValue;
        }

        public Current GetBaseValue(Enum enumValue)
        {
            return shiftableValues.GetBase(enumValue.GetIndex(), toolManager);
        }

        public void Invalidate(I_DeliveryTool deliveryTool, InvalidationIdentifier identifier)
        {
            if (identifier.enumKey is Enum enumValue)
            {
                changeListener.OnChange(enumValue);
                shiftableValues.OnChange(enumValue.GetIndex());
            }
        }

        public void Recalculate(I_EnumSO enumSO, I_DeliveryTool deliveryTool)
        {
            if (enumSO is Enum enumValue)
            {
                changeListener.OnChange(enumValue);
                shiftableValues.OnChange(enumValue.GetIndex());
            }
        }
    }
}