namespace Ashen.ControllerSystem
{
    public interface I_DungeonPlayerInputListener
    {
        void OnMoveForward();
        void OnMoveBackward();
        void OnMoveLeft();
        void OnMoveRight();
        void OnTurnLeft();
        void OnTurnRight();
        void OnMenu();
    }
}