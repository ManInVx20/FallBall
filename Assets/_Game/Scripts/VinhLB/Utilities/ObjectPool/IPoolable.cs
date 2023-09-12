using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public interface IPoolable<T>
    {
        void PoolSetup(System.Action<T> onReturnAction);
        void ReturnToPool();
    }
}
