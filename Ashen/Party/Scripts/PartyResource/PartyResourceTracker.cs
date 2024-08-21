using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Ashen.PartySystem
{
    public class PartyResourceTracker : A_EnumeratedTool<PartyResourceTracker>, I_Saveable
    {
        [ShowInInspector]
        private int[] partyResources;
        [ShowInInspector]
        private int[] tempPartyResource;
        private List<I_PartyResourceChangeListener>[] changeListeners;

        public override void Initialize()
        {
            partyResources = new int[PartyResources.Count];
            tempPartyResource = new int[PartyResources.Count];
            changeListeners = new List<I_PartyResourceChangeListener>[PartyResources.Count];
            for (int x = 0; x < changeListeners.Length; x++)
            {
                changeListeners[x] = new List<I_PartyResourceChangeListener>();
                partyResources[x] = 1;
            }
        }

        public void ResetReservedTotals()
        {
            for (int x = 0; x < tempPartyResource.Length; x++)
            {
                PartyResource pr = PartyResources.Instance[x];
                int oldReservedTotal = GetReservedTotal(pr);
                if (tempPartyResource[x] != 0)
                {
                    tempPartyResource[x] = 0;
                    HandleChange(pr, GetResourceTotal(pr), oldReservedTotal);
                }
            }
        }

        public void CommitReservedTotals()
        {
            for (int x = 0; x < partyResources.Length; x++)
            {
                PartyResource pr = PartyResources.Instance[x];
                int oldValue = GetResourceTotal(pr);
                int oldReservedTotal = GetReservedTotal(pr);
                if (oldValue != oldReservedTotal)
                {
                    int tempValue = tempPartyResource[x];
                    tempPartyResource[x] = 0;
                    AddResource(pr, tempValue);
                }
            }
        }

        public int GetResourceTotal(PartyResource resource)
        {
            return partyResources[(int)resource];
        }

        public int GetReservedTotal(PartyResource resource)
        {
            return tempPartyResource[(int)resource] + GetResourceTotal(resource);
        }

        public void AddResource(PartyResource resource, int total)
        {
            int previous = GetResourceTotal(resource);
            int previousReserved = GetReservedTotal(resource);
            partyResources[(int)resource] += total;
            HandleChange(resource, previous, previousReserved);
        }

        public void AddReservedResource(PartyResource resource, int total)
        {
            int previous = GetReservedTotal(resource);
            tempPartyResource[(int)resource] += total;
            HandleChange(resource, GetResourceTotal(resource), previous);
        }

        public void RemoveResource(PartyResource resource, int total)
        {
            int previous = GetResourceTotal(resource);
            int previousReserved = GetReservedTotal(resource);
            partyResources[(int)resource] -= total;
            if (partyResources[(int)resource] < 0)
            {
                partyResources[(int)resource] = 0;
            }
            HandleChange(resource, previous, previousReserved);
        }

        public void RemoveReservedResource(PartyResource resource, int total)
        {
            int previous = GetReservedTotal(resource);
            tempPartyResource[(int)resource] -= total;
            HandleChange(resource, GetResourceTotal(resource), previous);
        }

        public void SetResourceTotal(PartyResource resource, int total)
        {
            int previous = GetResourceTotal(resource);
            int previousReserved = GetReservedTotal(resource);
            partyResources[(int)resource] = total;
            HandleChange(resource, previous, previousReserved);
        }

        public void SetReservedTotal(PartyResource resource, int total)
        {
            int previous = GetReservedTotal(resource);
            tempPartyResource[(int)resource] = total;
            HandleChange(resource, GetResourceTotal(resource), previous);
        }

        public void RegisterListener(PartyResource resource, I_PartyResourceChangeListener listener)
        {
            changeListeners[(int)resource].Add(listener);
            HandleChange(resource, GetResourceTotal(resource), GetReservedTotal(resource));
        }

        public void UnRegisterListener(PartyResource resource, I_PartyResourceChangeListener listener)
        {
            changeListeners[(int)resource].Remove(listener);
        }

        private void HandleChange(PartyResource resource, int previous, int previousReserved)
        {
            List<I_PartyResourceChangeListener> listeners = changeListeners[(int)resource];
            foreach (I_PartyResourceChangeListener listener in listeners)
            {
                listener.OnChange(resource, previous, GetResourceTotal(resource), previousReserved, GetReservedTotal(resource));
            }
        }

        public object CaptureState()
        {
            PartyResourceSaveData saveData = new()
            {
                partyResources = new int[partyResources.Length]
            };
            for (int x = 0; x < partyResources.Length; x++)
            {
                saveData.partyResources[x] = partyResources[x];
            }
            return saveData;
        }

        public void RestoreState(object state)
        {
            PartyResourceSaveData saveData = (PartyResourceSaveData)state;
            for (int x = 0; x < partyResources.Length; x++)
            {
                SetResourceTotal(PartyResources.Instance[x], saveData.partyResources[x]);
            }
        }

        public void PrepareRestoreState()
        {
            ResetReservedTotals();
        }

        [Serializable]
        private struct PartyResourceSaveData
        {
            public int[] partyResources;
        }
    }
}