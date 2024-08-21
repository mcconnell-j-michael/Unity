using Ashen.ToolSystem;
using UnityEngine;

namespace Ashen.AbilitySystem
{
    public class Target : A_EnumSO<Target, Targets>
    {
        [SerializeField]
        public I_TargetHolder targetHolder;
        [SerializeField]
        public I_TargetProcessor targetProcessor;

        public I_TargetHolder BuildTargetHolder(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, AbilityAction action)
        {
            I_TargetHolder holder = targetHolder.Clone();
            holder.Initialize(source, sourceParty, targetParty, action);
            return holder;
        }
    }
}