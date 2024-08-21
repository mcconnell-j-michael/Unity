using Ashen.ToolSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ashen.CombatSystem
{
    public class PartyUIManager : A_PartyUIManager
    {
        [NonSerialized]
        public GameObject damageTextCanvas;

        public GameObject damageTextPrefab;
        public GameObject partyMemberUIPrefab;

        private bool showAll;
        public bool ShowAll
        {
            get { return showAll; }
            set
            {
                showAll = value;
                Recalculate();
            }
        }

        protected override void Start()
        {
            PlayerPartyHolder.Instance.partyManager.RegisterPartyUIManager(this);
            positionToManager = new Dictionary<PartyPosition, A_CharacterSelector>();
            PartyPositionManager[] positionManagers = GetComponentsInChildren<PartyPositionManager>();
            foreach (PartyPositionManager positionManager in positionManagers)
            {
                GameObject partyPosition = Instantiate(partyMemberUIPrefab, positionManager.transform);
                A_CharacterSelector selector = partyPosition.GetComponent<A_CharacterSelector>();
                PartyPosition pos = PartyPositions.Instance.GetPartyPosition(positionManager.partyRow, positionManager.partyColumn);
                positionToManager.Add(pos, selector);
            }
            base.Start();
            foreach (PartyPosition position in PartyPositions.Instance)
            {
                if (positionToManager.TryGetValue(position, out A_CharacterSelector manager))
                {
                    (manager as PartyMemberManager).partyUIManager = this;
                }
            }
            damageTextCanvas = DamageTextCanvas.Instance.gameObject;
            Recalculate();
        }

        public override void SetPartyMember(PartyPosition position, ToolManager toolManager)
        {
            base.SetPartyMember(position, toolManager);
            Recalculate();
        }

        public void Recalculate()
        {
            PlayerPartyManager playerManager = PlayerPartyHolder.Instance.partyManager;
            foreach (PartyPosition pos in PartyPositions.Instance)
            {
                A_CharacterSelector manager = managers[(int)pos];
                if (manager)
                {
                    if (playerManager.enabledPositions.Contains(pos))
                    {
                        HandleSlot(manager);
                    }
                    else
                    {
                        manager.GetDisabler().SetActive(false);
                    }
                }
            }
        }

        public void HandleSlot(A_CharacterSelector slot)
        {
            if (!slot.HasRegisteredToolManager() && !showAll)
            {
                slot.GetDisabler().SetActive(false);
            }
            else
            {
                slot.GetDisabler().SetActive(true);
            }
        }

        protected override A_PartyManager GetPartyManager()
        {
            return PlayerPartyHolder.Instance.partyManager;
        }
    }
}