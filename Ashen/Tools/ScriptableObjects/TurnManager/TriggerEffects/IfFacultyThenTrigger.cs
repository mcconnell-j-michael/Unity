using Ashen.DeliverySystem;
using Sirenix.Serialization;
using System.Collections;
using UnityEngine;

namespace Ashen.ToolSystem
{
    public class IfFacultyThenTrigger : I_TriggerEffect
    {
        [OdinSerialize]
        private Faculty faculty;
        [OdinSerialize]
        private ExtendedEffectTrigger toTrigger;
        [OdinSerialize]
        private bool inverse;

        public void OnTriggerEffect(ToolManager toolManager, ExtendedEffectTrigger extendedEffectTrigger)
        {
            if (toTrigger == extendedEffectTrigger)
            {
                return;
            }
            FacultyTool fTool = toolManager.Get<FacultyTool>();
            if (!fTool)
            {
                return;
            }
            bool facultyRes = fTool.Can(faculty);
            bool conditionMet = (inverse && !facultyRes) || (!inverse && facultyRes);
            if (!conditionMet)
            {
                return;
            }
            TriggerTool tTool = toolManager.Get<TriggerTool>();
            if (!tTool)
            {
                return;
            }
            tTool.Trigger(toTrigger);
        }
    }
}