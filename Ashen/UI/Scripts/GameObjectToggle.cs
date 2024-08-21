using UnityEngine;
using System.Collections;

public class GameObjectToggle : TabButton
{
    public GameObject toggle;

    public override void OnSelected()
    {
        base.OnSelected();
        toggle.SetActive(true);
    }

    public override void OnDeselected()
    {
        base.OnDeselected();
        toggle.SetActive(false);
    }
}
