using Ashen.ControllerSystem;
using Ashen.StateMachineSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class DungeonCrawlingState : A_InputState, I_DungeonPlayerInputListener
{
    public bool smoothTransition = true;
    [ShowIf(nameof(smoothTransition))]
    public float transitionSpeed = 10f;
    [ShowIf(nameof(smoothTransition))]
    public float transitionRotationSpeed = 500f;

    public float moveDelay = .15f;
    public float rotateDelay = .15f;

    public float startingEncounterPercentage;
    public float baseEncounterRate;

    public I_GameState combatState;
    public I_GameState victoryState;
    public I_GameState failureState;

    public I_GameState pauseScreenState;

    [NonSerialized]
    public MapDescription map;

    private DungeonPlayerInputManager playerInput;
    private float encounterPercentage;
    private Transform player;
    private Vector2Int currentPosition;
    private RotationDirection currentRotation;

    private MovePartyState movePartyState;
    private RotatePartyState rotatePartyState;

    public DungeonCrawlingState(bool smoothTransition, float transitionSpeed, float transitionRotationSpeed, float moveDelay, float rotateDelay, float startingEncounterPercentage,
        float baseEncounterRate, I_GameState combatState, I_GameState victoryState, I_GameState failureState, I_GameState pauseScreenState)
    {
        this.smoothTransition = smoothTransition;
        this.transitionSpeed = transitionSpeed;
        this.transitionRotationSpeed = transitionRotationSpeed;
        this.moveDelay = moveDelay;
        this.rotateDelay = rotateDelay;
        this.startingEncounterPercentage = startingEncounterPercentage;
        this.baseEncounterRate = baseEncounterRate;
        this.combatState = combatState;
        this.victoryState = victoryState;
        this.failureState = failureState;
        this.pauseScreenState = pauseScreenState;
    }

    public void Initialize(MapDescription map, Transform player)
    {
        Initialize(map, player, map.defaultPosition, map.defaultRotation, startingEncounterPercentage);
    }

    public void Initialize(MapDescription map, Transform player, Vector2Int currentPosition, RotationDirection currentRotation, float encounterPercentage)
    {
        this.map = map;
        this.player = player;
        this.currentPosition = currentPosition;
        this.currentRotation = currentRotation;
        this.player.SetPositionAndRotation(new Vector3(currentPosition.x, this.player.position.y, currentPosition.y),
            Quaternion.Euler(new Vector3(this.player.rotation.eulerAngles.x, RotationDirectionFunctions.GetDegrees(currentRotation), this.player.rotation.eulerAngles.z)));
        this.encounterPercentage = encounterPercentage;
        playerInput = DungeonPlayerInputManager.Instance;
        movePartyState = new MovePartyState();
        rotatePartyState = new RotatePartyState();
    }

    protected override void PostProcessState()
    {
        SaveMemory();
    }

    public bool RollForEncounter()
    {
        if (encounterPercentage < 0f)
        {
            return false;
        }
        if (UnityEngine.Random.Range(0, 100f) <= encounterPercentage)
        {
            return true;
        }
        return false;
    }

    public void SaveMemory()
    {
        DungeonMemory dungeonMemory = DungeonMemory.Instance;
        dungeonMemory.currentPosition = currentPosition;
        dungeonMemory.currentRotation = currentRotation;
        dungeonMemory.description = map;
        dungeonMemory.encounterPercentage = encounterPercentage;
    }

    public void OnMoveForward()
    {
        OnMove(currentPosition + RotationDirectionFunctions.GetForward(currentRotation));
    }

    public void OnMoveBackward()
    {
        OnMove(currentPosition + RotationDirectionFunctions.GetBackward(currentRotation));
    }

    public void OnMoveLeft()
    {
        OnMove(currentPosition + RotationDirectionFunctions.GetLeft(currentRotation));
    }

    public void OnMoveRight()
    {
        OnMove(currentPosition + RotationDirectionFunctions.GetRight(currentRotation));
    }

    private void OnMove(Vector2Int targetPosition)
    {
        if (IsDelayed() || targetPosition == currentPosition || !map.RequestMove(currentPosition, targetPosition))
        {
            return;
        }
        movePartyState.Initialize(player, smoothTransition, transitionSpeed, targetPosition);
        currentPosition = targetPosition;
        SaveMemory();
        encounterPercentage += (map.GetEncounterMultiplier(currentPosition) * baseEncounterRate);
        if (RollForEncounter())
        {
            encounterPercentage = startingEncounterPercentage;
            response.nextState = combatState;
        }
        else
        {
            SetSelectDelay(moveDelay);
        }
        WaitForState(movePartyState);
    }

    public void OnTurnLeft()
    {
        OnRotate(RotationDirectionFunctions.TurnLeft(currentRotation));
    }

    public void OnTurnRight()
    {
        OnRotate(RotationDirectionFunctions.TurnRight(currentRotation));
    }

    private void OnRotate(RotationDirection targetRotation)
    {
        if (IsDelayed() || targetRotation == currentRotation)
        {
            return;
        }
        rotatePartyState.Initialize(player, smoothTransition, transitionRotationSpeed, targetRotation);
        currentRotation = targetRotation;
        SetSelectDelay(rotateDelay);
        WaitForState(rotatePartyState);
    }

    public void OnMenu()
    {
        WaitForState(pauseScreenState);
    }

    protected override void RegisterInputManager()
    {
        DungeonPlayerInputManager inputManager = DungeonPlayerInputManager.Instance;
        inputManager.Enable();
        inputManager.RegisterListner(this);
    }

    protected override void UnRegisterInputManager()
    {
        DungeonPlayerInputManager inputManager = DungeonPlayerInputManager.Instance;
        inputManager.UnRegisterListener(this);
    }
}
