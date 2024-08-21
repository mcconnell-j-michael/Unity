using UnityEngine;
using System.Collections;

public class RotationDirectionFunctions
{
    public static int GetDegrees(RotationDirection rotation)
    {
        switch (rotation)
        {
            case RotationDirection.NORTH:
                return 0;
            case RotationDirection.EAST:
                return 90;
            case RotationDirection.SOUTH:
                return 180;
            case RotationDirection.WEST:
                return 270;
        }
        return 0;
    }

    public static RotationDirection GetRotationDirection(float degrees)
    {
        while (degrees >= 360f)
        {
            degrees -= 360f;
        }
        while (degrees < 0)
        {
            degrees += 360f;
        }
        if (degrees >= 0f && degrees <= 45f)
        {
            return RotationDirection.NORTH;
        }
        if (degrees > 45f && degrees <= 135f)
        {
            return RotationDirection.EAST;
        }
        if (degrees > 135f && degrees <= 225f)
        {
            return RotationDirection.SOUTH;
        }
        if (degrees > 225f && degrees <= 360f)
        {
            return RotationDirection.WEST;
        }
        return RotationDirection.NORTH;
    }

    public static Vector2Int GetForward(RotationDirection direction)
    {
        switch (direction)
        {
            case RotationDirection.NORTH:
                return new Vector2Int(0, 1);
            case RotationDirection.EAST:
                return new Vector2Int(1, 0);
            case RotationDirection.SOUTH:
                return new Vector2Int(0, -1);
            case RotationDirection.WEST:
                return new Vector2Int(-1, 0);
        }
        return new Vector2Int(0, 1);
    }

    public static Vector2Int GetRight(RotationDirection direction)
    {
        switch (direction)
        {
            case RotationDirection.NORTH:
                return new Vector2Int(1, 0);
            case RotationDirection.EAST:
                return new Vector2Int(0, -1);
            case RotationDirection.SOUTH:
                return new Vector2Int(-1, 0);
            case RotationDirection.WEST:
                return new Vector2Int(0, 1);
        }
        return new Vector2Int(0, 1);
    }

    public static Vector2Int GetBackward(RotationDirection direction)
    {
        return -GetForward(direction);
    }

    public static Vector2Int GetLeft(RotationDirection direction)
    {
        return -GetRight(direction);
    }

    public static RotationDirection TurnRight(RotationDirection direction)
    {
        switch (direction)
        {
            case RotationDirection.NORTH:
                return RotationDirection.EAST;
            case RotationDirection.EAST:
                return RotationDirection.SOUTH;
            case RotationDirection.SOUTH:
                return RotationDirection.WEST;
            case RotationDirection.WEST:
                return RotationDirection.NORTH;
        }
        return RotationDirection.NORTH;
    }

    public static RotationDirection TurnLeft(RotationDirection direction)
    {
        switch (direction)
        {
            case RotationDirection.NORTH:
                return RotationDirection.WEST;
            case RotationDirection.EAST:
                return RotationDirection.NORTH;
            case RotationDirection.SOUTH:
                return RotationDirection.EAST;
            case RotationDirection.WEST:
                return RotationDirection.SOUTH;
        }
        return RotationDirection.NORTH;
    }
}

public enum RotationDirection
{
    NORTH, EAST, SOUTH, WEST
}