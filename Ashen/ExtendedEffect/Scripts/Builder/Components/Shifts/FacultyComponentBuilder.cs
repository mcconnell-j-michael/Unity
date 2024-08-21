using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class FacultyComponentBuilder : I_ComponentBuilder
    {
        [SerializeField]
        private Faculty faculty;
        [SerializeField]
        private int priority;
        [SerializeField]
        private bool enable;

        public I_ExtendedEffectComponent Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            return new FacultyComponent(faculty, null, enable, priority);
        }

        public FacultyComponentBuilder(SerializationInfo info, StreamingContext context)
        {
            faculty = Faculties.Instance[info.GetInt32(nameof(faculty))];
            priority = info.GetInt32(nameof(priority));
            enable = info.GetBoolean(nameof(enable));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(faculty), (int)faculty);
            info.AddValue(nameof(priority), priority);
            info.AddValue(nameof(enable), enable);
        }
    }
}