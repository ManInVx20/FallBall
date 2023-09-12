using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public interface IObjectPool<T>
    {
        T Pull();
        void Push(T t);
    }
}
