using Ashen.AbilitySystem;
using Ashen.DeliverySystem;
using Ashen.EnumSystem;
using Ashen.ToolSystem;
using UnityEngine;

public abstract class A_CharacterSelector : MonoBehaviour, I_Targetable, I_TriggerListener, I_DamageListener, I_EnumCacheable
{
    public ToolManager toolManager;

    protected ExtendedEffectTrigger primaryActionStart;
    protected ExtendedEffectTrigger primaryActionEnd;
    protected ExtendedEffectTrigger secondaryActionStart;
    protected ExtendedEffectTrigger secondaryActionEnd;

    protected void Awake()
    {
        primaryActionStart = ExtendedEffectTriggers.GetEnum("PrimaryActionStart");
        primaryActionEnd = ExtendedEffectTriggers.GetEnum("PrimaryActionEnd");
        secondaryActionStart = ExtendedEffectTriggers.GetEnum("SecondaryActionStart");
        secondaryActionEnd = ExtendedEffectTriggers.GetEnum("SecondaryActionEnd");
    }

    public void RegisterToolManager(ToolManager toolManager)
    {
        if (this.toolManager == toolManager)
        {
            return;
        }
        UnregisterToolManager();
        this.toolManager = toolManager;
        if (toolManager)
        {
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            triggerTool.RegisterTriggerListener(primaryActionStart, this);
            triggerTool.RegisterTriggerListener(primaryActionEnd, this);
            triggerTool.RegisterTriggerListener(secondaryActionStart, this);
            triggerTool.RegisterTriggerListener(secondaryActionEnd, this);
            DamageTool damageTool = toolManager.Get<DamageTool>();
            //damageTool.RegisterListener(DamageTypes.Instance.NORMAL, this);
            damageTool.RegisterListener(this);
            //ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            //rvTool.RegiserThresholdChangeListener(ResourceValues.Instance.health, this);
            FacultyTool fTool = toolManager.Get<FacultyTool>();
            fTool.Cache(Faculties.Instance.CONSCIOUS, this);
            RegisterToolManagerInternal(toolManager);
        }
    }

    public void UnregisterToolManager()
    {
        if (toolManager)
        {
            TriggerTool triggerTool = toolManager.Get<TriggerTool>();
            triggerTool.UnregisterTriggerListener(primaryActionStart, this);
            triggerTool.UnregisterTriggerListener(primaryActionEnd, this);
            triggerTool.UnregisterTriggerListener(secondaryActionStart, this);
            triggerTool.UnregisterTriggerListener(secondaryActionEnd, this);
            DamageTool damageTool = toolManager.Get<DamageTool>();
            //damageTool.UnRegisterListener(DamageTypes.Instance.NORMAL, this);
            damageTool.UnRegisterListener(this);
            //ResourceValueTool rvTool = toolManager.Get<ResourceValueTool>();
            //rvTool.UnRegesterThresholdChangeListener(ResourceValues.Instance.health, this);
            FacultyTool fTool = toolManager.Get<FacultyTool>();
            fTool.UnCache(Faculties.Instance.CONSCIOUS, this);
            UnregisterToolManagerInternal();
        }
    }

    public abstract void Deselected();

    public abstract void Selected();

    public abstract void SelectedSecondary();

    protected abstract void RegisterToolManagerInternal(ToolManager toolManager);

    protected abstract void UnregisterToolManagerInternal();

    public virtual void TurnSelectionStart() { }
    public virtual void TurnSelectionEnd() { }

    private void OnDestroy()
    {
        UnregisterToolManager();
    }

    public virtual GameObject GetDisabler()
    {
        return gameObject;
    }

    public bool HasRegisteredToolManager()
    {
        return toolManager != null;
    }

    public ToolManager GetTarget()
    {
        return toolManager;
    }

    public void OnTrigger(ExtendedEffectTrigger trigger)
    {
        if (trigger == primaryActionStart)
        {
            OnPrimaryActionStart();
        }
        else if (trigger == secondaryActionStart)
        {
            OnSecondaryActionStart();
        }
        else if (trigger == primaryActionEnd)
        {
            OnPrimaryActionEnd();
        }
        else if (trigger == secondaryActionEnd)
        {
            OnSecondaryActionEnd();
        }
        else
        {
            OnTriggerInternal(trigger);
        }
    }

    public void Recalculate(I_EnumSO enumValue, I_DeliveryTool deliveryTool)
    {
        if (enumValue is Faculty faculty)
        {
            Recalculate(faculty, deliveryTool);
        }
    }

    protected virtual void OnPrimaryActionStart() { }
    protected virtual void OnPrimaryActionEnd() { }
    protected virtual void OnSecondaryActionStart() { }
    protected virtual void OnSecondaryActionEnd() { }
    protected virtual void OnTriggerInternal(ExtendedEffectTrigger trigger) { }
    public abstract void OnDamageEvent(DamageEvent damageEvent);
    public abstract void OnFacultyChange(Faculty faculty, bool value);
    public abstract void OnThresholdEvent(ThresholdEventValue value);
    public abstract void Recalculate(Faculty faculty, I_DeliveryTool deliveryTool);

}
