using Ashen.EnumSystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using UnityEngine;

namespace Ashen.DeliverySystem
{
    [Serializable]
    public class DerivedAttributeValue : A_RecalculatedDeliveryValue
    {
        [SerializeField, HideLabel]
        private DerivedAttribute derivedAttribute;

        public override float Build(I_DeliveryTool owner, I_DeliveryTool target, DeliveryArgumentPacks deliveryArguments)
        {
            DeliveryTool dTool = owner as DeliveryTool;
            ToolManager tm = dTool.toolManager;
            AttributeTool aTool = tm.Get<AttributeTool>();
            return (int)aTool.GetAttribute(derivedAttribute);
        }

        public override string Visualize()
        {
            return derivedAttribute.name;
        }

        protected override void OnRegisterInternal(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO)
        {
            DeliveryTool dTool = deliveryTool as DeliveryTool;
            ToolManager tm = dTool.toolManager;
            AttributeTool aTool = tm.Get<AttributeTool>();
            aTool.Cache(enumSO, listener);
        }

        protected override void OnDeregisterInternal(I_DeliveryTool deliveryTool, I_CombinedEnumListener listener, I_EnumSO enumSO)
        {
            DeliveryTool dTool = deliveryTool as DeliveryTool;
            ToolManager tm = dTool.toolManager;
            AttributeTool aTool = tm.Get<AttributeTool>();
            aTool.UnCache(enumSO, listener);
        }

        public DerivedAttributeValue(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            derivedAttribute = DerivedAttributes.Instance[info.GetInt32(nameof(derivedAttribute))];
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(derivedAttribute), (int)derivedAttribute);
        }
    }
}
