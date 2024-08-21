using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ashen.ToolSystem
{
    /**
     * This is what is used to track the tools that are currently on a character
     **/
    public class ToolEnumList
    {
        public A_Tool[] list;

        public ToolEnumList(int count)
        {
            list = new A_Tool[count];
        }

        public ToolEnumList(ToolEnumList oldList, int count)
        {
            list = new A_Tool[count];
            for (int x = 0; x < oldList.list.Length && x < count; x++)
            {
                list[x] = oldList.list[x];
            }
        }

        public A_Tool Get(ToolEnum toolEnum)
        {
            if (toolEnum == null)
            {
                return null;
            }
            return list[toolEnum.IntValue];
        }

        public void Add(ToolEnum toolEnum, A_Tool tool)
        {
            if (toolEnum != null)
            {
                list[toolEnum.IntValue] = tool;
            }
        }

        public void Remove(ToolEnum toolEnum)
        {
            if (toolEnum != null)
            {
                list[toolEnum.IntValue] = null;
            }
        }
    }
}
