using UnityEngine;

[CreateAssetMenu(fileName = nameof(PrefabPool), menuName = "Custom/Pool/" + nameof(PrefabPool))]
public class PrefabPool : A_Pool<PoolableBehaviour>
{
    public GameObject prefab;
    public GameObject parent;

    protected override PoolableBehaviour InternalBuildObject()
    {
        if (!parent)
        {
            GenerateParent();
        }
        GameObject go = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        go.transform.SetParent(parent.transform, false);
        PoolableBehaviour poolable = go.GetComponent<PoolableBehaviour>();
        if (!poolable)
        {
            poolable = go.AddComponent<PoolableBehaviour>();
        }
        poolable.OnCreate();
        poolable.RegisterPrefabPool(this);
        if (poolable is PoolableWrapper wrapper)
        {
            return wrapper.subBehaviour;
        }
        return poolable;
    }

    private void GenerateParent()
    {
        string prefabParent = nameof(PoolManager);
        GameObject prefabParentGo = GameObject.Find(prefabParent);
        if (!prefabParentGo)
        {
            prefabParentGo = new GameObject(prefabParent);
        }
        string prefabPoolName = prefab.name;
        Transform prefabPoolTransform = prefabParentGo.transform.Find(prefabPoolName);
        if (!prefabPoolTransform)
        {
            GameObject prefabPoolGo = new GameObject(prefabPoolName);
            prefabPoolTransform = prefabPoolGo.transform;
            prefabPoolTransform.parent = prefabParentGo.transform;
        }
        parent = prefabPoolTransform.gameObject;
    }

    protected override void InternalRelease(PoolableBehaviour element)
    {
        element.gameObject.transform.SetParent(parent.transform, false);
    }

    protected override void OnDestroyObject(PoolableBehaviour element)
    {
        Destroy(element);
    }
}
