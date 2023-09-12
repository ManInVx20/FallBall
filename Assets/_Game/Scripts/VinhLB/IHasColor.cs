using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public interface IHasColor
    {
        public ColorType GetColorType();
        public void SetColorType(ColorType colorType);
        public bool IsColorTypeMatching(ColorType colorType);
    }
}
