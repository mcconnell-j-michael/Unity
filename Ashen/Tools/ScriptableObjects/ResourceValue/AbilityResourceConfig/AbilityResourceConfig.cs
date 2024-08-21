using UnityEngine;

public class AbilityResourceConfig : ScriptableObject
{
    [SerializeField]
    private Color resourceColor1;
    [SerializeField]
    private Color resourceColor2;
    [SerializeField]
    private string resourceDisplayName;

    public Color ResourceColor1 { get { return resourceColor1; } }
    public Color ResourceColor2 { get { return resourceColor2; } }
    public string ResourceDisplayName { get { return resourceDisplayName; } }
}
