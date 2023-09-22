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
}
