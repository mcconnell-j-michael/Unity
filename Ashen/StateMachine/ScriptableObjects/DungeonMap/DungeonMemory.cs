using Ashen.Map;
using System;
using UnityEngine;

public class DungeonMemory : SingletonMonoBehaviour<DungeonMemory>, I_Saveable
{
    public MapDescription description;
    public Vector2Int currentPosition;
    public RotationDirection currentRotation;
    public float encounterPercentage;

    public void Memorize(MapDescription description, Vector2Int currentPosition, RotationDirection currentRotation, float encounterPercentage)
    {
        this.description = description;
        this.currentPosition = currentPosition;
        this.currentRotation = currentRotation;
        this.encounterPercentage = encounterPercentage;
    }

    public object CaptureState()
    {
        return new DungeonMemorySaveData()
        {
            mapDescription = description.name,
            currentPositionX = currentPosition.x,
            currentPositionY = currentPosition.y,
            rotationDirection = currentRotation,
            encounterPercentage = encounterPercentage
        };
    }

    public void RestoreState(object state)
    {
        DungeonMemorySaveData saveData = (DungeonMemorySaveData)state;
        this.description = MapDescriptionLibrary.Instance.GetScriptableObject(saveData.mapDescription);
        this.currentPosition = new Vector2Int(saveData.currentPositionX, saveData.currentPositionY);
        this.currentRotation = saveData.rotationDirection;
        this.encounterPercentage = saveData.encounterPercentage;
    }

    public void PrepareRestoreState()
    {
    }

    [Serializable]
    private struct DungeonMemorySaveData
    {
        public string mapDescription;
        public int currentPositionX;
        public int currentPositionY;
        public RotationDirection rotationDirection;
        public float encounterPercentage;
    }
}
