using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VinhLB;

public static class Utilities
{
    public static float RepeatValue(float value, float min, float max)
    {
        if (value < min)
        {
            value = max - (min - value) % (max - min);
        }
        else if (value > max)
        {
            value = (value - max) % (max - min) + min;
        }

        return value;
    }

    public static Vector3 FindCenterPosition(Vector3[] positionArray)
    {
        if (positionArray == null || positionArray.Length == 0)
        {
            return Vector3.zero;
        }

        float totalX = 0.0f;
        float totalY = 0.0f;
        float totalZ = 0.0f;
        for (int i = 0; i < positionArray.Length; i++)
        {
            totalX += positionArray[i].x;
            totalY += positionArray[i].y;
            totalZ += positionArray[i].z;
        }
        float centerX = totalX / positionArray.Length;
        float centerY = totalY / positionArray.Length;
        float centerZ = totalZ / positionArray.Length;

        return new Vector3(centerX, centerY, centerZ);
    }

    public static Vector2[] ToVector2Array(this Vector3[] vectorArray)
    {
        return System.Array.ConvertAll<Vector3, Vector2>(vectorArray, GetVector2FromVector3);
    }

    public static Vector2 GetVector2FromVector3(Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }
    
    public static Vector3[] ToVector3Array(this Vector2[] vectorArray)
    {
        return System.Array.ConvertAll<Vector2, Vector3>(vectorArray, GetVector3FromVector2);
    }

    public static Vector3 GetVector3FromVector2(Vector2 vector)
    {
        return new Vector3(vector.x, vector.y, 0.0f);
    }
}
