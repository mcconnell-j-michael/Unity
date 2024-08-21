using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class PreventDeselection : MonoBehaviour
{
    EventSystem evt;

    private void Start()
    {
        evt = EventSystem.current;
    }

    [ShowInInspector]
    GameObject sel;

    private void Update()
    {
        if (evt.currentSelectedGameObject != null && evt.currentSelectedGameObject != sel)
            sel = evt.currentSelectedGameObject;
        else if (sel != null && evt.currentSelectedGameObject == null)
            evt.SetSelectedGameObject(sel);
    }
}
