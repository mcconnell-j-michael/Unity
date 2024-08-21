using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class InitialDungeonCrawlerState : SingletonScriptableObject<InitialDungeonCrawlerState>, I_GameState
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

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        MapBuilder mapBuilder = Object.FindObjectOfType<MapBuilder>();
        PlayerIdentifier playerIdentifier = Object.FindObjectOfType<PlayerIdentifier>();

        DungeonCrawlingState dungeonCrawlingState = new(smoothTransition, transitionSpeed, transitionRotationSpeed, moveDelay,
            rotateDelay, startingEncounterPercentage, baseEncounterRate, combatState, victoryState,
            failureState, pauseScreenState);
        DungeonMemory dungeonMemory = DungeonMemory.Instance;
        mapBuilder.mapDescription = dungeonMemory.description;
        mapBuilder.BuildLevel();
        dungeonCrawlingState.Initialize(mapBuilder.mapDescription, playerIdentifier.transform, dungeonMemory.currentPosition, dungeonMemory.currentRotation, dungeonMemory.encounterPercentage);
        yield return dungeonCrawlingState.RunState(request, response);
    }
}
