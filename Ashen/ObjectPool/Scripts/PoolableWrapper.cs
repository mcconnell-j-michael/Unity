public class PoolableWrapper : PoolableBehaviour
{
    public PoolableBehaviour subBehaviour;

    protected override void InternalDisable()
    {
        subBehaviour.Disable();
    }

    public override bool Enabled()
    {
        return subBehaviour.Enabled();
    }

    public override void Initialize()
    {
        subBehaviour.Initialize();
    }

    public override void OnCreate()
    {
        subBehaviour.OnCreate();
    }

    public override void RegisterPrefabPool(PrefabPool pool)
    {
        subBehaviour.RegisterPrefabPool(pool);
    }
}
