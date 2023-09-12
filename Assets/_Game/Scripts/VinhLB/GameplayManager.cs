using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class GameplayManager : MonoSingleton<GameplayManager>
    {
        public Color GetColorByColorType(ColorType colorType)
        {
            Color color = Color.white;
            switch (colorType)
            {
                case ColorType.None:
                    color = Color.white;
                    break;
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
            }

            return color;
        }
    }

    [System.Serializable]
    public enum ColorType
    {
        None = 0,
        Red = 1,
        Green = 2,
        Blue = 3,
        Yellow = 4,
        Cyan = 5,
        Magenta = 6,
        Rainbow = 7,
    }
}
