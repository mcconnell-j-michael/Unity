using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;

public class StatusEffectSymbolsManager : SerializedMonoBehaviour
{
    public GameObject statusEffectSymbolPrefab;
    public int totalSymbols;
    private List<StatusEffectSymbolManager> availableStatusEffectSymbolManagers;
    private List<StatusEffectSymbolManager> currentStatusEffectSymbolManagers;

    [NonSerialized]
    public float initialValue;
    [NonSerialized]
    public float growthRate;

    public void Start()
    {
        availableStatusEffectSymbolManagers = new List<StatusEffectSymbolManager>();
        currentStatusEffectSymbolManagers = new List<StatusEffectSymbolManager>();
        for (int x = 0; x < totalSymbols; x++)
        {
            BuildStatusEffectSymbolManager();
        }
    }

    private void BuildStatusEffectSymbolManager()
    {
        GameObject background = Instantiate(statusEffectSymbolPrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        GameObject symbol = background.transform.GetChild(0).gameObject;
        StatusEffectSymbolManager manager = symbol.GetComponent<StatusEffectSymbolManager>();
        manager.statusEffectSymbolsManager = this;
        manager.image.enabled = false;
        manager.backgroundImage.enabled = false;
        availableStatusEffectSymbolManagers.Add(manager);
    }

    public StatusEffectSymbolManager CreateStatusEffectSymbol(StatusEffectSymbol symbol)
    {
        if (availableStatusEffectSymbolManagers.Count == 0)
        {
            BuildStatusEffectSymbolManager();
        }
        StatusEffectSymbolManager manager = availableStatusEffectSymbolManagers[0];
        availableStatusEffectSymbolManagers.RemoveAt(0);
        int currentTotal = currentStatusEffectSymbolManagers.Count;
        currentStatusEffectSymbolManagers.Add(manager);
        manager.symbol = symbol;
        manager.backgroundImage.enabled = true;
        manager.image.enabled = true;
        manager.image.color = symbol.color;
        manager.parentTransform.anchoredPosition = new Vector3(initialValue + (currentTotal * growthRate), manager.parentTransform.position.y, 0);
        return manager;
    }

    public void Disable(StatusEffectSymbolManager statusEffectSymbolManager)
    {
        int index = currentStatusEffectSymbolManagers.IndexOf(statusEffectSymbolManager);
        availableStatusEffectSymbolManagers.Add(statusEffectSymbolManager);
        currentStatusEffectSymbolManagers.Remove(statusEffectSymbolManager);
        for (int x = index; x < currentStatusEffectSymbolManagers.Count; x++)
        {
            StatusEffectSymbolManager manager = currentStatusEffectSymbolManagers[x];
            manager.parentTransform.anchoredPosition = new Vector3(manager.parentTransform.anchoredPosition.x - growthRate, manager.parentTransform.position.y);
        }
        statusEffectSymbolManager.image.enabled = false;
        statusEffectSymbolManager.backgroundImage.enabled = false;
    }
}
