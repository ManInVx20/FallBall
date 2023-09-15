using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        public Color GetColorByColorType(ColorType colorType)
        {
            Color color;
            switch (colorType)
            {
                case ColorType.Red:
                    color = Color.red;
                    break;
                case ColorType.Green:
                    color = Color.green;
                    break;
                case ColorType.Blue:
                    color = Color.blue;
                    break;
                case ColorType.Yellow:
                    color = Color.yellow;
                    break;
                case ColorType.Cyan:
                    color = Color.cyan;
                    break;
                case ColorType.Magenta:
                    color = Color.magenta;
                    break;
                default:
                    color = Color.white;
                    break;
            }

            return color;
        }
    }

    [System.Serializable]
    public enum ColorType
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3,
        Cyan = 4,
        Magenta = 5,
    }

    [System.Serializable]
    public enum BallType
    {
        Normal = 0,
        Rainbow = 1,
    }
}
