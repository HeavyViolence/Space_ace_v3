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

        public Guid ID { get; private set; }

        [Inject]
        private void Construct(GamePauser gamePauser)
        {
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
        }

        private void Awake()
        {
            ID = Guid.NewGuid();
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

        #region interfaces

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

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as IPausable) == true;

        public bool Equals(IPausable other) =>
            other is not null && ID == other.ID;

        public override int GetHashCode() =>
            ID.GetHashCode();

        #endregion
    }
}