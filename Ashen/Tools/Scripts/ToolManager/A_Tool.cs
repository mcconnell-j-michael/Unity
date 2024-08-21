using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Ashen.ToolSystem
{
    /**
     * See A_EnumeratedTool
     **/
    public abstract class A_Tool : SerializedMonoBehaviour
    {
        public ToolEnum ToolEnum { get; private set; }
        public ToolManager toolManager;
        public bool invalid;

        public static ToolEnum GetToolEnumFromTool(A_Tool tool)
        {
            return tool.ToolEnum;
        }

        public void Awake()
        {
            ToolEnum = GetToolEnum();
            if (ToolEnum == null)
            {
                ToolEnum = new ToolEnum(GetType());
            }
            toolManager = gameObject.GetComponent<ToolManager>();
            if (toolManager == null)
            {
                toolManager = gameObject.AddComponent<ToolManager>();
                toolManager.Init();
            }
            A_Tool abstractTool = toolManager.Get(ToolEnum);
            if (abstractTool)
            {
                invalid = true;
                Destroy(this);
            }
            else
            {
                toolManager.Add(ToolEnum, this);
            }
            Initialize();
            SetTool();
        }

        public virtual void OnDestroy()
        {
            if (!invalid)
            {
                toolManager.Remove(ToolEnum);
            }
        }

        public abstract void SetTool();
        public abstract ToolEnum GetToolEnum();
        public virtual void Initialize()
        {

        }
    }
}
