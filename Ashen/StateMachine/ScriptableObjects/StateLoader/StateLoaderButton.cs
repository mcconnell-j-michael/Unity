using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class StateLoaderButton : MonoBehaviour
{
    public LoadOption option;
    public bool autoload = true;
    [HideIf(nameof(autoload))]
    public StateLoader loader;

    public void Start()
    {
        if (autoload)
        {
            StateLoaderHolder holder = GetComponentInParent<StateLoaderHolder>();
            loader = holder.stateLoader;
        }
    }

    public void Click()
    {
        loader.ExecuteState(option);
    }
}
