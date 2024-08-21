using System.Collections.Generic;
using UnityEngine;

public class A_SavableObject : MonoBehaviour
{
    public Dictionary<string, object> CaptureState()
    {
        Dictionary<string, object> state = new();

        foreach (I_Saveable savable in GetComponents<I_Saveable>())
        {
            state[savable.GetType().ToString()] = savable.CaptureState();
        }

        return state;
    }

    public void RestoreState(object stateObj)
    {
        Dictionary<string, object> state = (Dictionary<string, object>)stateObj;
        I_Saveable[] savables = GetComponents<I_Saveable>();

        foreach (I_Saveable savable in savables)
        {
            savable.PrepareRestoreState();
        }

        foreach (I_Saveable saveable in savables)
        {
            string typeName = saveable.GetType().ToString();

            if (state.TryGetValue(typeName, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}