using Ashen.ToolSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.AbilitySystem
{
    [Serializable]
    public class FacultyRequirement : I_AbilityRequirement
    {
        [SerializeField]
        private bool inverse;
        [EnumSODropdown]
        public List<Faculty> faculties;

        public bool IsValid(ToolManager toolManager, DeliveryArgumentPacks deliveryArguments)
        {
            FacultyTool fTool = toolManager.Get<FacultyTool>();
            if (!fTool)
            {
                return true;
            }
            foreach (Faculty faculty in faculties)
            {
                if (
                    (inverse && fTool.Can(faculty)) ||
                    (!inverse && !fTool.Can(faculty))
                )
                {
                    return false;
                }
            }
            return true;
        }
    }
}