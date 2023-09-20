using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class Effect : MonoBehaviour, IPoolable<Effect>
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        private Action<Effect> _onReturnAction;

        private void OnParticleSystemStopped()
        {
            ReturnToPool();
        }

        public void Play()
        {
            _particleSystem.Play();
        }

        public void PoolSetup(Action<Effect> onReturnAction)
        {
            _onReturnAction = onReturnAction;
        }

        public void ReturnToPool()
        {
            _onReturnAction?.Invoke(this);
        }
    }
}
