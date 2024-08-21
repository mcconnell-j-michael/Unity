using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Ashen.NodeTreeSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkillTreeNodeColorBinder : A_ColorThemeBinder
{
    private NodeTreeUIManager nodeTreeUIManager;

    [FoldoutGroup("Valid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string validNodeAccentA;
    [FoldoutGroup("Valid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string validNodeAccentB;
    [FoldoutGroup("Valid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string validNodeAccentC;

    [FoldoutGroup("Invalid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string invalidNodeAccentA;
    [FoldoutGroup("Invalid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string invalidNodeAccentB;
    [FoldoutGroup("Invalid Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string invalidNodeAccentC;

    [FoldoutGroup("Selected Node")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string selectedNodeAccentA;

    [FoldoutGroup("Requirements")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string lineColor;
    [FoldoutGroup("Requirements")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string requirementsColor;

    [FoldoutGroup("Selection")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string activeSelection;
    [FoldoutGroup("Selection")]
    [ValueDropdown(nameof(GetColorThemes))]
    public string inactiveSelection;

    private Color cachedValidAccentA;
    private Color cachedValidAccentB;
    private Color cachedValidAccentC;
    private Color cachedInvalidAccentA;
    private Color cachedInvalidAccentB;
    private Color cachedInvalidAccentC;
    private Color cachedSelectedAccentA;
    private Color cachedSelectedAccentB;
    private Color cachedSelectedAccentC;
    private Color cachedActiveSelection;
    private Color cachedInactiveSelection;

    protected override void InternalUpdateColor()
    {
        if (!nodeTreeUIManager)
        {
            nodeTreeUIManager = GetComponent<NodeTreeUIManager>();
        }
        Color validA = colorThemeManager.colorMap[validNodeAccentA];
        if (cachedValidAccentA != validA)
        {
            nodeTreeUIManager.validNode.one = validA;
            cachedValidAccentA = validA;
        }
        Color validB = colorThemeManager.colorMap[validNodeAccentB];
        if (cachedValidAccentB != validB)
        {
            nodeTreeUIManager.validNode.two = validB;
            cachedValidAccentB = validB;
        }
        Color validC = colorThemeManager.colorMap[validNodeAccentC];
        if (cachedValidAccentC != validC)
        {
            nodeTreeUIManager.validNode.three = validC;
            cachedValidAccentC = validC;
        }

        Color invalidA = colorThemeManager.colorMap[invalidNodeAccentA];
        if (cachedInvalidAccentA != invalidA)
        {
            nodeTreeUIManager.invalidNode.one = invalidA;
            cachedInvalidAccentA = invalidA;
        }
        Color invalidB = colorThemeManager.colorMap[invalidNodeAccentB];
        if (cachedInvalidAccentB != invalidB)
        {
            nodeTreeUIManager.invalidNode.two = invalidB;
            cachedInvalidAccentB = invalidB;
        }
        Color invalidC = colorThemeManager.colorMap[invalidNodeAccentC];
        if (cachedInvalidAccentC != invalidC)
        {
            nodeTreeUIManager.invalidNode.three = invalidC;
            cachedInvalidAccentC = invalidC;
        }

        Color selectedA = colorThemeManager.colorMap[selectedNodeAccentA];
        if (cachedSelectedAccentA != selectedA)
        {
            nodeTreeUIManager.selectedNodeColor = selectedA;
            cachedSelectedAccentA = selectedA;
        }

        Color activeSelection = colorThemeManager.colorMap[this.activeSelection];
        if (cachedActiveSelection != activeSelection)
        {
            nodeTreeUIManager.selectedIndicator = activeSelection;
            cachedActiveSelection = activeSelection;
        }
        Color inactiveSelection = colorThemeManager.colorMap[this.inactiveSelection];
        if (cachedInactiveSelection != inactiveSelection)
        {
            nodeTreeUIManager.inactiveIndicator = inactiveSelection;
            cachedInactiveSelection = inactiveSelection;
        }

        nodeTreeUIManager.UpdateNodes();

        Color lineColor = colorThemeManager.colorMap[this.lineColor];
        NodeSquareLineDrawerUI[] lines = gameObject.GetComponentsInChildren<NodeSquareLineDrawerUI>();
        foreach (NodeSquareLineDrawerUI line in lines)
        {
            if (line.color != lineColor)
            {
                line.color = lineColor;
#if UNITY_EDITOR
                EditorUtility.SetDirty(line);
#endif
            }
        }

        Color requirementsColor = colorThemeManager.colorMap[this.requirementsColor];
        NodeRequirementsPositionController[] requirements = gameObject.GetComponentsInChildren<NodeRequirementsPositionController>();
        foreach (NodeRequirementsPositionController requirement in requirements)
        {
            Graphic graphic = requirement.GetComponent<Graphic>();
            if (graphic != null)
            {
                if (graphic.color != requirementsColor)
                {
                    graphic.color = requirementsColor;
#if UNITY_EDITOR
                    EditorUtility.SetDirty(graphic);
#endif
                }
            }
        }
    }
}
