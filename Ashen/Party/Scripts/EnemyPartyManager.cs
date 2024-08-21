using Ashen.CombatSystem;
using Ashen.ToolSystem;
using System;
using UnityEngine;

namespace Ashen.PartySystem
{
    public class EnemyPartyManager : A_PartyManager
    {
        [NonSerialized]
        public EnemyPartyMemberManager[] partyMemberManagers;

        protected override void Start()
        {
            partyMemberManagers = new EnemyPartyMemberManager[PartyPositions.Count];
            PartyPositionManager[] positionManagers = GetComponentsInChildren<PartyPositionManager>();
            foreach (PartyPositionManager manager in positionManagers)
            {
                PartyPosition pos = PartyPositions.Instance.GetPartyPosition(manager.partyRow, manager.partyColumn);
                partyMemberManagers[(int)pos] = manager.GetComponentInChildren<EnemyPartyMemberManager>();
            }
            EnemyPartyHolder.Instance.RegisterEnemyPartyManager(this);
        }

        public override void SetToolManager(PartyPosition position, ToolManager toolManager)
        {
            base.SetToolManager(position, toolManager);
            if (!toolManager)
            {
                return;
            }
            SpriteRenderer renderer = toolManager.GetComponent<SpriteRenderer>();
            renderer.sortingOrder = position.partyRow.enemySortingOrder;
        }

        protected override void Swap(PartyPosition pos1, PartyPosition pos2)
        {
            ToolManager tm1 = GetToolManager(pos1);
            ToolManager tm2 = GetToolManager(pos2);
            EnemyPartyMemberManager pm1 = partyMemberManagers[(int)pos1];
            EnemyPartyMemberManager pm2 = partyMemberManagers[(int)pos2];
            if (tm1 != null)
            {
                GameObject go1 = tm1.gameObject;
                go1.transform.SetParent(pm2.transform, false);
            }
            if (tm2 != null)
            {
                GameObject go2 = tm2.gameObject;
                go2.transform.SetParent(pm1.transform, false);
            }
            base.Swap(pos1, pos2);
        }
    }
}