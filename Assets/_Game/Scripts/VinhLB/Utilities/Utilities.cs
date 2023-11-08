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

    public static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
    {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 temp = camera.WorldToViewportPoint(position);

        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        //The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
        //returned value will still be correct.

        temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
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
