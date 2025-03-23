using MessagePack;

using SpaceAce.Auxiliary;

using System;
using System.Collections.Generic;

using UnityEngine;

using VContainer.Unity;

namespace SpaceAce.Main.Saving
{
    public abstract class SavingSystem : IInitializable, IDisposable
    {
        public event Action<Exception> CaptureStateFailed, RestoreStateFailed;
        public event Action<Exception> SavingFailed, LoadingFailed;

        private readonly KeyGenerator _keyGenerator;
        private readonly Encryptor _encryptor;

        private readonly HashSet<ISavable> _registeredEntities = new(new SavableEqualityComparer());
        private Dictionary<int, byte[]> _statesCache;
        private bool _bootstrapped = false;
        private bool _statesCacheUpdated = false;

        protected abstract string SavePath { get; }

        public SavingSystem(KeyGenerator keyGenerator, Encryptor encryptor)
        {
            _keyGenerator = keyGenerator ?? throw new ArgumentNullException(nameof(keyGenerator));
            _encryptor = encryptor ?? throw new ArgumentNullException(nameof(encryptor));
        }

        public void Register(ISavable entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EnsureSavingSystemBootstrap();

            if (_registeredEntities.Add(entity) == true)
            {
                entity.StateChanged += () => OnStateChanged(entity);

                try
                {
                    TryRestoreState(entity);
                }
                catch (Exception ex)
                {
                    AuxAsync.WaitForNextFrameThenDoAsync(() => RestoreStateFailed?.Invoke(ex)).Forget();
                }
            }
        }

        public void Deregister(ISavable entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (_registeredEntities.Remove(entity) == true)
            {
                entity.StateChanged -= () => OnStateChanged(entity);
            }
        }

        private void CaptureState(ISavable entity)
        {
            byte[] state = entity.GetState();
            byte[] key = _keyGenerator.GenerateKey(entity.StateName);
            byte[] iv = _keyGenerator.GenerateIV();

            byte[] encryptedState = _encryptor.Encrypt(state, key, iv);

            byte[] cachedState = new byte[encryptedState.Length + iv.Length];
            Buffer.BlockCopy(encryptedState, 0, cachedState, 0, encryptedState.Length);
            Buffer.BlockCopy(iv, 0, cachedState, encryptedState.Length, iv.Length);

            int cacheKey = entity.StateName.GetHashCode();
            _statesCache[cacheKey] = cachedState;
        }

        private bool TryRestoreState(ISavable entity)
        {
            if (_statesCache.Count == 0)
            {
                return false;
            }

            int cacheKey = entity.StateName.GetHashCode();

            if (_statesCache.TryGetValue(cacheKey, out byte[] cachedState) == true)
            {
                byte[] encryptedState = cachedState[..^_keyGenerator.IVSize];
                byte[] key = _keyGenerator.GenerateKey(entity.StateName);
                byte[] iv = cachedState[^_keyGenerator.IVSize..];

                byte[] decryptedState = _encryptor.Decrypt(encryptedState, key, iv);

                entity.SetState(decryptedState);
                return true;
            }

            return false;
        }

        private void EnsureSavingSystemBootstrap()
        {
            if (_bootstrapped == false)
            {
                MessagePackSerializer.DefaultOptions =
                    MessagePack.Resolvers.ContractlessStandardResolver.Options
                        .WithSecurity(MessagePackSecurity.UntrustedData)
                        .WithCompression(MessagePackCompression.Lz4BlockArray);

                try
                {
                    if (TryLoad(out var states) == true)
                    {
                        _statesCache = new(states);
                    }
                    else
                    {
                        _statesCache = new();
                    }
                }
                catch (Exception ex)
                {
                    _statesCache = new();
                    AuxAsync.WaitForNextFrameThenDoAsync(() => LoadingFailed?.Invoke(ex)).Forget();
                }

                _bootstrapped = true;
            }
        }

        protected abstract void Save(IEnumerable<KeyValuePair<int, byte[]>> states);
        protected abstract bool TryLoad(out IEnumerable<KeyValuePair<int, byte[]>> states);

        #region interfaces

        public void Initialize()
        {
            Application.wantsToQuit += OnApplicationWantsToQuit;
        }

        public void Dispose()
        {
            Application.wantsToQuit -= OnApplicationWantsToQuit;
        }

        #endregion

        #region event handlers

        private void OnStateChanged(ISavable entity)
        {
            try
            {
                CaptureState(entity);

                if (_statesCacheUpdated == false)
                {
                    _statesCacheUpdated = true;
                }
            }
            catch (Exception ex)
            {
                CaptureStateFailed?.Invoke(ex);
            }
        }

        private bool OnApplicationWantsToQuit()
        {
            if (_statesCacheUpdated == true)
            {
                try
                {
                    Save(_statesCache);
                    return true;
                }
                catch (Exception ex)
                {
                    SavingFailed?.Invoke(ex);
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}