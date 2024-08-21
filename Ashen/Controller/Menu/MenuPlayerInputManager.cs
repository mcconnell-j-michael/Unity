using Ashen.ControllerSystem;
using UnityEngine.InputSystem;

public class MenuPlayerInputManager : A_PlayerInputManager<MenuPlayerInputManager, I_MenuPlayerInputListener>
{
    private void Update()
    {
        if (SelectUp)
        {
            foreach (I_MenuPlayerInputListener listener in listeners) { listener.OnSelectUp(); }
        }
        else if (SelectDown)
        {
            foreach (I_MenuPlayerInputListener listener in listeners) { listener.OnSelectDown(); }
        }
        else if (SelectLeft)
        {
            foreach (I_MenuPlayerInputListener listener in listeners) { listener.OnSelectLeft(); }
        }
        else if (SelectRight)
        {
            foreach (I_MenuPlayerInputListener listener in listeners) { listener.OnSelectRight(); }
        }
    }

    private void LateUpdate()
    {
        Submit = false;
        Cancel = false;
        Information = false;
        NextButton = false;
        PreviousButton = false;
    }

    public bool Cancel { get; private set; }
    void OnCancel(InputValue value)
    {
        if (value.isPressed)
        {
            foreach (I_MenuPlayerInputListener listener in listeners) { listener.OnCancel(); }
        }
        Cancel = value.isPressed;
    }

    public bool SelectDown { get; private set; }
    void OnSelectDown(InputValue value)
    {
        SelectDown = value.isPressed;
    }

    public bool SelectLeft { get; private set; }
    void OnSelectLeft(InputValue value)
    {
        SelectLeft = value.isPressed;
    }

    public bool SelectRight { get; private set; }
    void OnSelectRight(InputValue value)
    {
        SelectRight = value.isPressed;
    }

    public bool SelectUp { get; private set; }
    void OnSelectUp(InputValue value)
    {
        SelectUp = value.isPressed;
    }

    public bool Submit { get; private set; }
    void OnSubmit(InputValue value)
    {
        if (value.isPressed)
        {
            foreach (I_MenuPlayerInputListener listener in listeners) { listener.OnSubmit(); }
        }
        Submit = value.isPressed;
    }

    public bool Information { get; private set; }
    void OnInformation(InputValue value)
    {
        if (value.isPressed)
        {
            foreach (I_MenuPlayerInputListener listener in listeners) { listener.OnInformation(); }
        }
        Information = value.isPressed;
    }

    public bool NextButton { get; private set; }
    void OnNextButton(InputValue value)
    {
        if (value.isPressed)
        {
            foreach (I_MenuPlayerInputListener listener in listeners) { listener.OnNextButton(); }
        }
    }

    public bool PreviousButton { get; private set; }
    void OnPreviousButton(InputValue value)
    {
        if (value.isPressed)
        {
            foreach (I_MenuPlayerInputListener listener in listeners) { listener.OnPreviousButton(); }
        }
    }
}
