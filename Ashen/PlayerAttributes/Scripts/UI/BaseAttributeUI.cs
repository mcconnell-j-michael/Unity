using Ashen.ToolSystem;

public class BaseAttributeUI : A_AttributeUI<BaseAttribute>
{
    public BaseAttribute baseAttribute;
    public DerivedAttribute derivedAttribute;

    private BaseAttributeTool baseAttributeTool;
    private AttributeTool attributeTool;

    public override float GetValue()
    {
        return attributeTool.GetAttribute(derivedAttribute);
    }

    public override string GetDefaultName()
    {
        return baseAttribute.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        tooltipTrigger = gameObject.GetComponent<TooltipTrigger>();
        baseAttributeTool.Cache(baseAttribute, this);
        SetText();
    }

    public override float GetBaseValue()
    {
        return attributeTool.GetBaseValue(derivedAttribute);
    }
}
