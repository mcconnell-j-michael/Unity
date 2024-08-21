using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatusEffectSymbolManagerUI : MonoBehaviour
{
    public GameObject statusEffectSymbolPrefab;
    public RectTransform container;

    public List<StatusEffectSymbolUI> activeSymbols;

    private float totalWidth;
    private float symbolWidth;
    private float height;

    private void Start()
    {
        totalWidth = container.rect.width;
        height = container.rect.height;
        symbolWidth = statusEffectSymbolPrefab.GetComponent<RectTransform>().rect.width;

        activeSymbols = new List<StatusEffectSymbolUI>();
    }

    public void RegisterStatusEffectSymbols(List<StatusSymbolRegistery> registries)
    {
        for (int x = 0; x < registries.Count; x++)
        {
            StatusSymbolRegistery registry = registries[x];
            registries[x] = InternalRegisterStatusEffectSymbol(registry);
        }
        ResolveSymbols();
    }

    public StatusSymbolRegistery RegisterStatusEffectSymbol(StatusSymbolRegistery registry)
    {
        StatusSymbolRegistery newRegistry = InternalRegisterStatusEffectSymbol(registry);

        ResolveSymbols();

        return newRegistry;
    }

    public StatusSymbolRegistery InternalRegisterStatusEffectSymbol(StatusSymbolRegistery registry)
    {
        PrefabPool statusEffectPool = PoolManager.Instance.GetPoolManager(statusEffectSymbolPrefab);
        PoolableBehaviour behaviour = statusEffectPool.GetObject();
        behaviour.transform.SetParent(container.transform, false);

        StatusEffectSymbolUI symbol = behaviour.GetComponent<StatusEffectSymbolUI>();
        registry.symbolUI = symbol;

        symbol.greyScaleImage.sprite = registry.sprite;
        symbol.filler.sprite = registry.sprite;
        symbol.SetFill(registry.percentage);

        activeSymbols.Add(symbol);

        return registry;
    }

    public void UnregisterStatusEffectSymbol(StatusEffectSymbolUI ui)
    {
        ui.poolableBehaviour.Disable();
        ResolveSymbols();
    }

    public void ResolveSymbols()
    {
        float halfSprite = symbolWidth / 2f;
        float halfContainer = totalWidth / 2f;

        for (int x = 0; x < activeSymbols.Count; x++)
        {
            StatusEffectSymbolUI ui = activeSymbols[x];
            if (!ui.poolableBehaviour.Enabled())
            {
                activeSymbols.RemoveAt(x);
                x--;
                continue;
            }
            ui.transform.localPosition = new Vector3((-symbolWidth * x) + halfContainer - halfSprite, 0);
        }
    }
}
