using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class ResourceManager : MonoSingleton<ResourceManager>
    {
        [System.Serializable]
        private struct SpriteByType
        {
            public BallType BallType;
            public Sprite Sprite;
        }

        [System.Serializable]
        private struct ColorByType
        {
            public ColorType ColorType;
            public Color Color;
        }

        [SerializeField]
        private SpriteByType[] _spriteByTypeArray;
        [SerializeField]
        private ColorByType[] _colorByTypeArray;

        public Sprite GetBallSpriteByType(BallType ballType)
        {
            for (int i = 0; i < _spriteByTypeArray.Length; i++)
            {
                if (_spriteByTypeArray[i].BallType == ballType)
                {
                    return _spriteByTypeArray[i].Sprite;
                }
            }

            return null;
        }

        public Color GetColorByColorType(ColorType colorType)
        {
            for (int i = 0; i < _colorByTypeArray.Length; i++)
            {
                if (_colorByTypeArray[i].ColorType == colorType)
                {
                    return _colorByTypeArray[i].Color;
                }
            }

            return Color.white;
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
        White = 6,
    }

    [System.Serializable]
    public enum BallType
    {
        Normal = 0,
        Rainbow = 1,
        Spike = 2,
    }
}
