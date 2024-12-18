using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceAce.Main.Saving
{
    public abstract class SavingSystem
    {
        public event EventHandler<StateSavedEventArgs> StateSaved;
        public event EventHandler<StateLoadedEventArgs> StateLoaded;

        public event EventHandler<SavingFailedEventArgs> SavingFailed;
        public event EventHandler<LoadingFailedEventArgs> LoadingFailed;

        protected static readonly UTF8Encoding UTF8 = new(true, true);

        private readonly HashSet<ISavable> _registeredEntities = new();

        protected readonly IKeyGenerator KeyGenerator;
        protected readonly Encryptor Encryptor;

        public SavingSystem(IKeyGenerator keyGenerator, Encryptor encryptor)
        {
            KeyGenerator = keyGenerator ?? throw new ArgumentNullException();
            Encryptor = encryptor ?? throw new ArgumentNullException();
        }

        public void Register(ISavable entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            if (_registeredEntities.Add(entity) == true)
            {
                entity.SavingRequested += (_, _) => Save(entity);
                entity.ErrorOccurred += (s, e) => LoadingFailed?.Invoke(s, new(entity.SavedDataName, e.Error));

                try
                {
                    if (TryLoad(entity) == true)
                    {
                        StateLoaded?.Invoke(this, new(entity.SavedDataName));
                    }
                }
                catch (Exception ex)
                {
                    LoadingFailed?.Invoke(this, new(entity.SavedDataName, ex));
                }
            }
        }

        public void Deregister(ISavable entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException();
            }

            if (_registeredEntities.Remove(entity) == true)
            {
                entity.SavingRequested -= (_, _) => Save(entity);
                entity.ErrorOccurred -= (s, e) => LoadingFailed?.Invoke(s, new(entity.SavedDataName, e.Error));

                try
                {
                    Save(entity);
                    StateSaved?.Invoke(this, new(entity.SavedDataName));
                }
                catch (Exception ex)
                {
                    SavingFailed?.Invoke(this, new(entity.SavedDataName, ex));
                }
            }
        }

        protected abstract string GetSavedDataPath(string savedDataName);

        protected abstract void Save(ISavable entity);
        protected abstract bool TryLoad(ISavable entity);
    }
}