using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.ToolSystem;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Ashen.ExtendedEffectSystem
{
    public abstract class A_ShiftableComponent<ShiftableTool, Enum, Shift> : A_SimpleComponent
        where Enum : I_EnumSO
        where ShiftableTool : A_EnumeratedTool<ShiftableTool>, I_Shiftable<Enum, Shift>
    {
        private Enum enumValue = default;
        private ShiftCategory shiftCategory = default;
        private Shift shift;
        private int priority;

        public A_ShiftableComponent() { }

        public A_ShiftableComponent(Enum enumValue, ShiftCategory shiftCategory, Shift shift, int priority)
        {
            this.enumValue = enumValue;
            this.shiftCategory = shiftCategory;
            this.shift = shift;
            this.priority = priority;
        }

        public override void Apply(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool dTarget = dse.target as DeliveryTool;
            if (dTarget)
            {
                ShiftableTool shiftableTool = dTarget.toolManager.Get<ShiftableTool>();
                if (shiftableTool)
                {
                    shiftableTool.AddShift(enumValue, 1, shiftCategory, container.key, shift);
                }
            }
        }

        public override void Remove(ExtendedEffect dse, ExtendedEffectContainer container)
        {
            DeliveryTool dTarget = dse.target as DeliveryTool;
            if (dTarget)
            {
                ShiftableTool shiftableTool = dTarget.toolManager.Get<ShiftableTool>();
                if (shiftableTool)
                {
                    shiftableTool.RemoveShift(enumValue, shiftCategory, container.key);
                }
            }
        }

        public A_ShiftableComponent(SerializationInfo info, StreamingContext context)
        {
            enumValue = GetEnumFromIndex(info.GetInt32(nameof(enumValue)));
            shiftCategory = ShiftCategories.Instance[info.GetInt32(nameof(shiftCategory))];
            shift = GetShift(info, nameof(shift));
            priority = info.GetInt32(nameof(priority));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(enumValue), enumValue.GetIndex());
            info.AddValue(nameof(shiftCategory), (int)shiftCategory);
            SaveShift(info, nameof(shift), shift);
            info.AddValue(nameof(priority), priority);
        }

        protected abstract Enum GetEnumFromIndex(int index);
        protected abstract Shift GetShift(SerializationInfo info, string serializationName);
        protected abstract void SaveShift(SerializationInfo info, string serializationName, Shift shift);
    }
}