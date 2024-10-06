using System;

using UnityEngine;

using Zenject;

namespace SpaceAce.Main.GamePause
{
    public sealed class AnimatorPauser : MonoBehaviour, IPausable
    {
        private GamePauser _gamePauser;
        private Animator _animator;

        public Guid ID { get; private set; }

        [Inject]
        private void Construct(GamePauser gamePauser)
        {
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
        }

        private void Awake()
        {
            ID = Guid.NewGuid();
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            _gamePauser.Register(this);
            _animator.speed = 1f;
        }

        private void OnDisable()
        {
            _gamePauser.Deregister(this);
        }

        #region interfaces

        public void Pause()
        {
            if (_animator != null)
                _animator.speed = 0f;
        }

        public void Resume()
        {
            if ( _animator != null)
                _animator.speed = 1f;
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