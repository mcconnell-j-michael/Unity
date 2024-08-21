using Ashen.EnumSystem;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public class A_ShiftableAttributeToolConfiguration<Tool, Self, Key, Value> : A_Configuration<Tool, Self>
        where Tool : A_ShiftableCacheTool<Tool, Self, Key, Value, Value, Value>
        where Self : A_ShiftableAttributeToolConfiguration<Tool, Self, Key, Value>
        where Key : class, I_EnumSO
    {
        [OdinSerialize]
        private Dictionary<Key, Value> defaultValues;

        public Dictionary<Key, Value> GetDefaultValues()
        {
            if (defaultValues == null)
            {
                if (this == GetDefault())
                {
                    return null;
                }
                return GetDefault().GetDefaultValues();
            }
            else
            {
                Dictionary<Key, Value> derivedDefaultBase = new Dictionary<Key, Value>();
                if (this != GetDefault())
                {
                    foreach (KeyValuePair<Key, Value> pair in GetDefault().defaultValues)
                    {
                        derivedDefaultBase.Add(pair.Key, pair.Value);
                    }
                }
                foreach (KeyValuePair<Key, Value> pair in defaultValues)
                {
                    if (derivedDefaultBase.ContainsKey(pair.Key))
                    {
                        derivedDefaultBase[pair.Key] = pair.Value;
                    }
                    else
                    {
                        derivedDefaultBase.Add(pair.Key, pair.Value);
                    }
                }
                return derivedDefaultBase;
            }
        }
    }
}