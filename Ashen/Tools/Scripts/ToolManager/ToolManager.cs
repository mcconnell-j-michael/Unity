using Ashen.DeliverySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ashen.ToolSystem
{
    /**
     * The ToolManager allows you to retrieve any other Tool with instant access
     **/
    public class ToolManager : MonoBehaviour
    {
        private static ToolEnum[] tools;
        private static int count = 0;

        public static void AddTool(ToolEnum toolEnum)
        {
            if (tools == null)
            {
                tools = new ToolEnum[20];
            }
            if (count >= tools.Length)
            {
                ToolEnum[] newTools = new ToolEnum[tools.Length * 2];
                CopyTools(tools, newTools);
                tools = newTools;
            }
            tools[count] = toolEnum;
            count++;
        }

        public static void CopyTools(ToolEnum[] original, ToolEnum[] newArr)
        {
            for (int x = 0; x < original.Length; x++)
            {
                newArr[x] = original[x];
            }
        }

        public static int Count()
        {
            return count;
        }

        private ToolEnumList InnerToolsTemp;
        private ToolEnumList innerTools
        {
            get
            {
                if (InnerToolsTemp == null)
                {
                    InnerToolsTemp = new ToolEnumList(tools.Length);
                }
                else if (InnerToolsTemp.list.Length != tools.Length)
                {
                    InnerToolsTemp = new ToolEnumList(InnerToolsTemp, tools.Length);
                }
                return InnerToolsTemp;
            }
        }
        private bool invalid = false;

        private void Awake()
        {
            Init();
            if (ToolLookUp.Instance.GetToolManager(gameObject))
            {
                invalid = true;
                Destroy(this);
            }
            else
            {
                ToolLookUp.Instance.Register(gameObject, this);
            }
        }

        public void Init()
        {
            if (tools == null)
            {
                tools = new ToolEnum[20];
            }
        }

        private void OnDestroy()
        {
            if (!invalid)
            {
                ToolLookUp.Instance.UnRegister(gameObject);
            }
        }

        public T Get<T>() where T : A_EnumeratedTool<T>
        {
            Init();
            return innerTools.Get(A_EnumeratedTool<T>.toolEnum) as T;
        }

        public A_Tool Get(ToolEnum toolEnum)
        {
            Init();
            return innerTools.Get(toolEnum);
        }

        public void Add(ToolEnum toolEnum, A_Tool tool)
        {
            Init();
            innerTools.Add(toolEnum, tool);
        }

        public void Remove(ToolEnum toolEnum)
        {
            Init();
            if (innerTools != null)
            {
                innerTools.Remove(toolEnum);
            }
        }
    }
}

