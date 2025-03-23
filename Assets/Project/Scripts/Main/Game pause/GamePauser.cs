using System;
using System.Collections.Generic;

namespace SpaceAce.Main.GamePause
{
    public sealed class GamePauser
    {
        public event Action GamePaused, GameResumed;

        private readonly HashSet<IPausable> _pausableEntities = new();

        public bool Paused { get; private set; } = false;

        public bool Register(IPausable entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            return _pausableEntities.Add(entity);
        }

        public bool Deregister(IPausable entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            return _pausableEntities.Remove(entity);
        }

        public bool DeregisterAll()
        {
            if (_pausableEntities.Count == 0)
            {
                return false;
            }

            _pausableEntities.Clear();
            return true;
        }

        public void Pause()
        {
            if (Paused == true)
            {
                return;
            }

            foreach (IPausable entity in _pausableEntities)
            {
                entity.Pause();
            }

            Paused = true;
            GamePaused?.Invoke();
        }

        public void Resume()
        {
            if (Paused == false)
            {
                return;
            }

            foreach (IPausable entity in _pausableEntities)
            {
                entity.Resume();
            }

            Paused = false;
            GameResumed?.Invoke();
        }
    }
}