using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ashen.ToolSystem
{
    /**
     * All EnumeratedTools automatically will have a ToolEnum generated for them 
     **/
    public class ToolEnum
    {
        public int IntValue { get; set; }
        public Type Type { get; set; }

        public ToolEnum(Type type)
        {
            IntValue = ToolManager.Count();
            Type = type;
            ToolManager.AddTool(this);
        }
    }
}
