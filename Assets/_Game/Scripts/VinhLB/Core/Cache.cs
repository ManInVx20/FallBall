using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public static class Cache
    {
        private static Dictionary<Collider, Component> cachedComponentDict = new Dictionary<Collider, Component>();
        private static Dictionary<Collider2D, Component> cachedComponent2DDict = new Dictionary<Collider2D, Component>();

        //public static bool TryGetComponent<T>(Collider collider, out T component)
        //{

        //}

        //public static bool TryGetComponent2D<T>(Collider2D collider2D, out T component)
        //{

        //}
    }
}
