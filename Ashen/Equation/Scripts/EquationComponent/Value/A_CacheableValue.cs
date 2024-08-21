using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.ToolSystem;
using System;
using System.Runtime.Serialization;

namespace Ashen.EquationSystem
{
    [Serializable]
    public abstract class A_CacheableValue<Tool, Enum, ReturnValue> : A_Value
        where Tool : A_EnumeratedTool<Tool>, I_CacheableTool<Enum, ReturnValue>
        where Enum : I_EnumSO, I_EquationAttribute<Tool, Enum, ReturnValue>
    {
        public Enum enumSO;
        public bool useTarget;

        public A_CacheableValue() { }

        public override float Calculate(Equation equation, I_DeliveryTool source, I_DeliveryTool target, float total, DeliveryArgumentPacks extraArguments)
        {
            if (enumSO == null)
            {
                return 0;
            }
            if (useTarget)
            {
                return enumSO.Get(target, enumSO, extraArguments);
            }
            return enumSO.Get(target, enumSO, extraArguments);
        }

        public override string Representation()
        {
            if (enumSO != null)
            {
                return enumSO.ToString();
            }
            return "null";
        }

        public override bool IsCachable()
        {
            return !useTarget && enumSO != null;
        }

        public override bool RequiresCaching()
        {
            return true;
        }

        public override bool RequiresRebuild()
        {
            return useTarget;
        }

        public override I_EquationComponent Rebuild(I_DeliveryTool source, I_DeliveryTool target, DeliveryArgumentPacks extraArguments)
        {
            if (!useTarget)
            {
                return this;
            }
            else
            {
                return new BasicValue
                {
                    value = enumSO.Get(target, enumSO, extraArguments)
                };
            }
        }

        public override bool Cache(I_DeliveryTool source, Equation equation)
        {
            if (IsCachable())
            {
                if (source == null)
                {
                    return false;
                }
                ToolManager toolManager = (source as DeliveryTool).toolManager;
                Tool tool = GetCachingTool(toolManager);
                if (tool == null)
                {
                    return false;
                }
                tool.Cache(enumSO, equation);
            }
            return false;
        }

        protected A_CacheableValue(SerializationInfo info, StreamingContext context)
        {
            enumSO = GetEnumFromIndex(info.GetInt32(nameof(enumSO)));
            useTarget = info.GetBoolean(nameof(useTarget));
        }

        protected void BaseGetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(enumSO), enumSO.GetIndex());
            info.AddValue(nameof(useTarget), useTarget);
        }

        protected abstract Tool GetCachingTool(ToolManager toolManager);
        protected abstract Enum GetEnumFromIndex(int index);
    }
}