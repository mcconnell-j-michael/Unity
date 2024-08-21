using Ashen.PartySystem;
using TMPro;
using UnityEngine;

namespace Ashen.Combat
{
    public class PartyResourceListener : MonoBehaviour, I_PartyResourceChangeListener
    {
        public PartyResource resource;
        public TextMeshProUGUI resourceCount;

        private PartyResourceTracker tracker;

        void Start()
        {
            tracker = PlayerPartyHolder.Instance.partyManager.partyToolManager.Get<PartyResourceTracker>();
            tracker.RegisterListener(resource, this);
        }

        private void OnDestroy()
        {
            if (tracker)
            {
                tracker.UnRegisterListener(resource, this);
            }
        }

        public void OnChange(PartyResource resource, int oldValue, int newValue, int oldReservedValue, int newReservedValue)
        {
            resourceCount.text = newReservedValue.ToString();
        }
    }
}