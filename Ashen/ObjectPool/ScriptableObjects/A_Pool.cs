using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class A_Pool<T> : ScriptableObject where T : class, I_Poolable
{
    [NonSerialized]
    private bool initialized = false;
    private ObjectPool<T> objectPool;

    public void Initialize()
    {
        if (initialized)
        {
            return;
        }
        initialized = true;
        objectPool = new ObjectPool<T>(
            BuildObject,
            (T element) => { element.Initialize(); },
            (T element) => { element.Disable(); },
            OnDestroyObject,
            false,
            100,
            10000);
    }

    public T GetObject()
    {
        Initialize();
        return objectPool.Get();
    }

    public void Release(T element)
    {
        Initialize();
        InternalRelease(element);
        objectPool.Release(element);
    }

    protected T BuildObject()
    {
        T obj = InternalBuildObject();
        return obj;
    }

    protected virtual void OnDestroyObject(T element) { }
    protected virtual void InternalRelease(T element) { }

    protected abstract T InternalBuildObject();
}