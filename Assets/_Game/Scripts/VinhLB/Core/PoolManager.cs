using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [System.Serializable]
        private struct PoolVariant
        {
            public GameObject Prefab;
            public int Amount;
        }

        [SerializeField]
        private List<PoolVariant> _poolVariantList;
    }
}
