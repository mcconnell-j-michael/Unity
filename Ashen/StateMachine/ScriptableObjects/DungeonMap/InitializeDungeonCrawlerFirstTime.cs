using System.Collections;
using UnityEngine;

public class InitializeDungeonCrawlerFirstTime : ScriptableObject, I_GameState
{
    public MapDescription description;
    public Vector2Int currentPosition;
    public RotationDirection currentRotation;
    public float encounterPercentage;

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        DungeonMemory memory = DungeonMemory.Instance;
        memory.Memorize(description, currentPosition, currentRotation, encounterPercentage);
        yield break;
    }
}
