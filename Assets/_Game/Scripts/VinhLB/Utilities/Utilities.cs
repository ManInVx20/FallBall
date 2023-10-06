using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
