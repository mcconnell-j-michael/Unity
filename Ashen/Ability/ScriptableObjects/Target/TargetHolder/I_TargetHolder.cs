using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Ashen.AbilitySystem
{
    public interface I_TargetHolder
    {
        I_CombatProcessor ResolveTarget();
        bool HasNextTarget();
        void GetRandomTargetable();
        void SetTargetable(ToolManager target);
        void GetTargetableByThreat();
        List<PartyPosition> GetValidPositions();
        I_TargetHolder Clone();
        I_Targetable GetFirstAvailableTargetable();
        void InitializeTarget(I_Targetable targetable);
        void InitializeTarget(ToolManager target);
        I_Targetable RequestMove(MoveDirection moveDirection);
        void Cleanup();
        void Initialize(ToolManager source, A_PartyManager sourceParty, A_PartyManager targetParty, AbilityAction action);
        List<TargetResult> FinalizeTargets();
    }
}