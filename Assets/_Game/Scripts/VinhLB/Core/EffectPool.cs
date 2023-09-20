using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class EffectPool : MonoSingleton<EffectPool>
    {
        [SerializeField]
        private GameObject _hitVFXPrefab;

        private ObjectPool<Effect> _hitVFXPool;

        private void Awake()
        {
            _hitVFXPool = new ObjectPool<Effect>(_hitVFXPrefab, OnPullCallback, OnPushCallback);
        }

        public Effect SpawnHitVFX(Vector3 position, Quaternion rotation)
        {
            Effect hitVFX = _hitVFXPool.Pull();
            hitVFX.transform.SetPositionAndRotation(position, rotation);

            return hitVFX;
        }

        public void RetrieveAll()
        {
            _hitVFXPool.RetrieveAll();
        }

        private void OnPullCallback(Effect effect)
        {
            effect.Play();
        }

        private void OnPushCallback(Effect effect)
        {
            
        }
    }
}
