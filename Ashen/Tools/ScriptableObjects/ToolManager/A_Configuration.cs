using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.ToolSystem
{
    public class A_Configuration<T, E> : SerializedScriptableObject, I_Configuration where T : A_ConfigurableTool<T, E> where E : A_Configuration<T, E>
    {
        [SerializeField]
        private bool isDefault;

        public void Reconfigure(ToolManager tm, Dictionary<string, object> arguments, bool addIfMissing = true)
        {
            T foundTool = tm.gameObject.GetComponent<T>();
            if (!foundTool)
            {
                if (!addIfMissing)
                {
                    return;
                }
                foundTool = tm.gameObject.AddComponent<T>();
            }
            foundTool.Initialize(this as E, arguments);
        }

        private E defaultValue;

        public E GetDefault()
        {
            if (defaultValue == null)
            {
                defaultValue = DefaultValues.Instance.config.GetConfiguration<E>();
            }
            return defaultValue;
        }

        public bool IsDefault()
        {
            return isDefault;
        }
    }
}