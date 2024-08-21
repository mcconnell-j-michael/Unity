using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButton selectedTab;

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
        button.OnDeselected();
        if (tabButtons.Count == 1 || button.isDefault)
        {
            OnTabSelected(button);
        }
        else
        {
            ResetTabs();
        }
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.SetSprite(tabHover);
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null && selectedTab == button)
        {
            return;
        }
        if (selectedTab != null)
        {
            selectedTab.OnDeselected();
        }
        selectedTab = button;
        ResetTabs();
        button.SetSprite(tabActive);
        selectedTab.OnSelected();
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.SetSprite(tabIdle);
        }
    }
}
