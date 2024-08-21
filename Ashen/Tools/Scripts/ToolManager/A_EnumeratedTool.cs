using UnityEngine;
using System.Collections;

namespace Ashen.ToolSystem
{
    /**
     * All Monobehaviours where there can only be one of per entity can extend from this class and they
     * will be able to be retrieved with instant access.
     **/
    public abstract class A_EnumeratedTool<T> : A_Tool where T : A_Tool
    {
        public static ToolEnum toolEnum;

        public override void SetTool()
        {
            toolEnum = ToolEnum;
        }

        public override ToolEnum GetToolEnum()
        {
            return toolEnum;
        }
    }
}