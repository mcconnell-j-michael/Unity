using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.NodeTreeSystem
{
    [Serializable]
    public class RequirementsContainer : MonoBehaviour
    {

        public List<NodeRequirementsPositionController> controllers;
        public NodeRequirementsPositionController focus;
        public NodeTreeUIManager source;
        public DisplayType displayType;

        public void Reset()
        {
            if (displayType == DisplayType.Combine)
            {
                focus = null;
                foreach (NodeRequirementsPositionController controller in controllers)
                {
                    if (!controller.disable)
                    {
                        focus = controller;
                        focus.hide = false;
                    }
                }
                foreach (NodeRequirementsPositionController controller in controllers)
                {
                    if (controller != focus)
                    {
                        controller.hide = true;
                    }
                }
            }
            else
            {
                foreach (NodeRequirementsPositionController controller in controllers)
                {
                    if (controller.disable)
                    {
                        controller.hide = true;
                    }
                    else
                    {
                        controller.hide = false;
                    }
                }
            }
        }
    }

    public enum DisplayType
    {
        Combine, Separate
    }
}