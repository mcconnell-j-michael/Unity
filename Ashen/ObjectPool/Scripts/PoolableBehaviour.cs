using UnityEngine;

public class PoolableBehaviour : MonoBehaviour, I_Poolable
{
    protected PrefabPool parentPool;

    public void Disable()
    {
        if (!Enabled())
        {
            return;
        }
        InternalDisable();
        if (parentPool != null)
        {
            parentPool.Release(this);
        }
    }

    protected virtual void InternalDisable()
    {
        enabled = false;
        gameObject.SetActive(false);
    }

    public virtual bool Enabled()
    {
        return enabled;
    }

    public virtual void Initialize()
    {
        enabled = true;
        gameObject.SetActive(true);
    }

    public virtual void OnCreate()
    {
        InternalDisable();
    }

    public virtual void RegisterPrefabPool(PrefabPool pool)
    {
        parentPool = pool;
    }
}
