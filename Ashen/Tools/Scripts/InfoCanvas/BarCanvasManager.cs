using Ashen.DeliverySystem;
using Ashen.ToolSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarCanvasManager : SerializedMonoBehaviour
{
    public GameObject barPrefab;

    private Dictionary<ResourceValue, BarManager> stackMapping;
    private List<ResourceValue> stack;

    [NonSerialized]
    public BarInfo[] barInfos;
    [NonSerialized]
    public List<ResourceValue> defaultResourceValues;
    [NonSerialized]
    public float initialValue;
    [NonSerialized]
    public float growthRate;
    [NonSerialized]
    public ToolManager toolManager;

    public void Start()
    {
        stackMapping = new Dictionary<ResourceValue, BarManager>();
        foreach (ResourceValue resourceValue in ResourceValues.Instance)
        {
            GameObject background = Instantiate(barPrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
            GameObject bar = background.transform.GetChild(0).gameObject;
            BarManager barManager = bar.AddComponent<BarManager>();
            stackMapping.Add(resourceValue, barManager);
            barManager.background = background.GetComponent<Image>();
            barManager.background.enabled = false;
            barManager.parentTransform = background.GetComponent<RectTransform>();
            barManager.resourceValue = resourceValue;
            barManager.toolManager = toolManager;
            barManager.barCanvasManager = this;
            Image image = bar.GetComponent<Image>();
            barManager.bar = image;
            image.enabled = false;
            image.color = barInfos[(int)resourceValue].color;
        }
        stack = new List<ResourceValue>();
        foreach (ResourceValue resourceValue in defaultResourceValues)
        {
            AddBar(resourceValue);
        }
    }

    public void AddBar(ResourceValue resourceValue)
    {
        if (stack.Contains(resourceValue))
        {
            return;
        }
        BarManager barManager = stackMapping[resourceValue];
        stack.Add(resourceValue);
        RectTransform rectTransform = barManager.parentTransform;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.anchoredPosition = new Vector3(0, initialValue + (growthRate * (stack.Count - 1)), 0);
        barManager.bar.enabled = true;
        barManager.background.enabled = true;
    }

    public void RemoveBar(ResourceValue resourceValue)
    {
        if (stack.Contains(resourceValue))
        {
            BarManager barManager = stackMapping[resourceValue];
            barManager.bar.enabled = false;
            barManager.background.enabled = false;
            int index = stack.IndexOf(resourceValue);
            for (int x = index + 1; x < stack.Count; x++)
            {
                ResourceValue stackResourceValue = stack[x];
                BarManager stackBackground = stackMapping[stackResourceValue];
                RectTransform rectTransform = stackBackground.parentTransform;
                rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - growthRate, rectTransform.anchoredPosition3D.z);
            }
            stack.Remove(resourceValue);
        }
    }
}
