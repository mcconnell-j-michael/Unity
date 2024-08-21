using Ashen.ToolSystem;

public class DerivedAttributeUI : A_AttributeUI<DerivedAttribute>
{
    public DerivedAttribute derivedAttribute;

    private AttributeTool attributeTool;

    public override float GetValue()
    {
        return attributeTool.GetAttribute(derivedAttribute);
    }

    public override string GetDefaultName()
    {
        return derivedAttribute.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        attributeTool.Cache(derivedAttribute, this);
        tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();
        SetText();
    }

    public override float GetBaseValue()
    {
        return attributeTool.GetBaseValue(derivedAttribute);
    }
}
