using System.Collections.Generic;

namespace Ashen.ToolSystem
{
    public interface I_Configuration
    {
        void Reconfigure(ToolManager tm, Dictionary<string, object> arguments, bool addIfMissing = true);
        bool IsDefault();
    }
}