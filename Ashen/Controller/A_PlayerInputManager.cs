using System.Collections.Generic;
using UnityEngine.InputSystem;

public abstract class A_PlayerInputManager<T, E> : SingletonMonoBehaviour<T> where T : A_PlayerInputManager<T, E>
{
    public string actionMapName;

    private PlayerInput playerInput;

    protected List<E> listeners;

    public void RegisterListner(E listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListener(E listener)
    {
        listeners.Remove(listener);
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        listeners = new List<E>();
        Initialize();
    }

    protected virtual void Initialize() { }

    public void Enable()
    {
        playerInput.SwitchCurrentActionMap(actionMapName);
    }
}
