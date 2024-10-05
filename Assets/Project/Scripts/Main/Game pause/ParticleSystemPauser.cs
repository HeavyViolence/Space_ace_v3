using System;
using System.Collections.Generic;

using UnityEngine;

using Zenject;

namespace SpaceAce.Main.GamePause
{
    public sealed class ParticleSystemPauser : MonoBehaviour, IPausable
    {
        private GamePauser _gamePauser;
        private IEnumerable<ParticleSystem> _particleSystems;

        [Inject]
        private void Construct(GamePauser gamePauser)
        {
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
        }

        private void Awake()
        {
            _particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        }

        private void OnEnable()
        {
            _gamePauser.Register(this);
        }

        private void OnDisable()
        {
            _gamePauser.Deregister(this);
        }

        public void Pause()
        {
            if (_particleSystems is not null)
                foreach (ParticleSystem system in _particleSystems)
                    system.Pause();
        }

        public void Resume()
        {
            if ( _particleSystems is not null)
                foreach (ParticleSystem system in _particleSystems)
                    system.Play();
        }
    }
}