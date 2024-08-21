using Ashen.ControllerSystem;
using UnityEngine.InputSystem;

public class DungeonPlayerInputManager : A_PlayerInputManager<DungeonPlayerInputManager, I_DungeonPlayerInputListener>
{
    private void Update()
    {
        if (moveForward)
        {
            foreach (I_DungeonPlayerInputListener listener in listeners) { listener.OnMoveForward(); }
        }
        else if (moveBackward)
        {
            foreach (I_DungeonPlayerInputListener listener in listeners) { listener.OnMoveBackward(); }
        }
        else if (moveLeft)
        {
            foreach (I_DungeonPlayerInputListener listener in listeners) { listener.OnMoveLeft(); }
        }
        else if (moveRight)
        {
            foreach (I_DungeonPlayerInputListener listener in listeners) { listener.OnMoveRight(); }
        }
        else if (turnLeft)
        {
            foreach (I_DungeonPlayerInputListener listener in listeners) { listener.OnTurnLeft(); }
        }
        else if (turnRight)
        {
            foreach (I_DungeonPlayerInputListener listener in listeners) { listener.OnTurnRight(); }
        }
    }

    public bool moveForward { get; private set; }
    void OnMoveForward(InputValue value)
    {
        moveForward = value.isPressed;
    }

    public bool moveBackward { get; private set; }
    void OnMoveBackward(InputValue value)
    {
        moveBackward = value.isPressed;
    }

    public bool moveLeft { get; private set; }
    void OnMoveLeft(InputValue value)
    {
        moveLeft = value.isPressed;
    }

    public bool moveRight { get; private set; }
    void OnMoveRight(InputValue value)
    {
        moveRight = value.isPressed;
    }

    public bool turnLeft { get; private set; }
    void OnTurnLeft(InputValue value)
    {
        turnLeft = value.isPressed;
    }

    public bool turnRight { get; private set; }
    void OnTurnRight(InputValue value)
    {
        turnRight = value.isPressed;
    }

    public bool menu { get; private set; }
    void OnMenu(InputValue value)
    {
        if (value.isPressed)
        {
            foreach (I_DungeonPlayerInputListener listener in listeners) { listener.OnMenu(); }
        }
    }
}
