using System.Collections;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    public class FacultyConditionalFilterBuilder : A_ConditionalFilterBuilder
    {
        [SerializeField]
        private Faculty faculty;
        [SerializeField]
        private bool requireDisabled;

        public override I_ConditionalFilter BuildCondition(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks arguments)
        {
            return new FacultyConditionalFilter(faculty, requireDisabled);
        }
    }
}