using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction : int
{
    East,
    NorthEast,
    North,
    NorthWest,
    West,
    SouthWest,
    South,
    SouthEast,
}

static class DirectionMethods
{
    public static bool IsCardinal(this Direction d) => (int)d % 2 == 0;
    public static Vector2 ToVector2(this Direction d)
    {
        switch (d)
        {
            case Direction.East:
                return Vector2.right;
            case Direction.NorthEast:
                return Vector2.one.normalized;
            case Direction.North:
                return Vector2.up;
            case Direction.NorthWest:
                return new Vector2(-1, 1).normalized;
            case Direction.West:
                return Vector2.left;
            case Direction.SouthWest:
                return new Vector2(-1, -1).normalized;
            case Direction.South:
                return Vector2.down;
            case Direction.SouthEast:
                return new Vector2(1, -1).normalized;
            default:
                return Vector2.zero;
        }
    }
    /// <summary>
    /// Returns the angle equivalent of this direction in degrees
    /// </summary>
    public static float ToAngleDeg(this Direction d)
    {
        var vec = d.ToVector2();
        var angle = Vector2.SignedAngle(Vector2.right, vec);
        if (angle < 0)
            angle += 360;
        return angle;
    }
    public static Direction FromVec2(Vector2 vec)
    {
        var movementAngle = Vector2.SignedAngle(Vector2.right, vec);
        if (movementAngle < 0)
            movementAngle += 360;
        return Direction.East.Offset(Mathf.RoundToInt(movementAngle / 45));
    }

    public static Direction FromAngleDeg(float angle)
    {
        if (angle < 0)
            angle += 360;
        return Direction.East.Offset(Mathf.RoundToInt(angle / 45));
    }
}
