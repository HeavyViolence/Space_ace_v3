using Cysharp.Threading.Tasks;

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

        public async UniTask<bool> RegisterAsync(ISavable entity)
        {
            if (entity is null)
                throw new ArgumentNullException();

            if (_registeredEntities.Add(entity) == true)
            {
                try
                {
                    entity.SavingRequested += async (_, _) => await SaveAsync(entity);
                    return await TryLoadAsync(entity);
                }
                catch (Exception ex)
                {
                    _registeredEntities.Remove(entity);
                    entity.SavingRequested -= async (_, _) => await SaveAsync(entity);

                    entity.SetDefaultState();
                    OnLoadingFailed(entity.SavedDataName, ex);

                    return false;
                }
            }

            return false;
        }

        public async UniTask<bool> DeregisterAsync(ISavable entity)
        {
            if (entity is null)
                throw new ArgumentNullException();

            if (_registeredEntities.Remove(entity) == true)
            {
                try
                {
                    entity.SavingRequested -= async (_, _) => await SaveAsync(entity);
                    await SaveAsync(entity);

                    return true;
                }
                catch (Exception ex)
                {
                    _registeredEntities.Add(entity);
                    entity.SavingRequested += async (_, _) => await SaveAsync(entity);

                    OnSavingFailed(entity.SavedDataName, ex);

                    return false;
                }
            }

            return false;
        }

        public async UniTask<bool> DeregisterAllAsync()
        {
            if (_registeredEntities.Count == 0)
                return false;

            foreach (ISavable savable in _registeredEntities)
                await DeregisterAsync(savable);

            return true;
        }

        protected void OnStateSaved(string savedDataName) =>
            StateSaved?.Invoke(this, new(savedDataName));

        protected void OnStateLoaded(string savedDataName) =>
            StateLoaded?.Invoke(this, new(savedDataName));

        protected void OnSavingFailed(string savedDataName, Exception ex) =>
            SavingFailed?.Invoke(this, new(savedDataName, ex));

        protected void OnLoadingFailed(string savedDataName, Exception ex) =>
            LoadingFailed?.Invoke(this, new(savedDataName, ex));

        protected abstract string GetSavedDataPath(string savedDataName);

        protected abstract UniTask SaveAsync(ISavable entity);
        protected abstract UniTask<bool> TryLoadAsync(ISavable entity);
    }
}