using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventSystemHelper : SingletonMonoBehaviour<EventSystemHelper>
{
    public void UpdateSelected(GameObject go)
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(Select(go));
    }

    private IEnumerator Select(GameObject go)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(go);
    }
}
