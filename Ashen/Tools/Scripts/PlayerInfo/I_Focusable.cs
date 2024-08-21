using UnityEngine;
using System.Collections;
using Ashen.DeliverySystem;

namespace Ashen.ToolSystem
{
    public interface I_Focusable
    {
        string BuildString(ToolManager deliveryTool);
        bool HandleFocus(ToolManager deliveryTool);
    }
}