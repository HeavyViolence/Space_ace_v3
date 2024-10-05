using Cysharp.Threading.Tasks;

using SpaceAce.Auxiliary;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace SpaceAce.Main.Saving
{
    public sealed class ToPlayerPrefsSavingSystem : SavingSystem
    {
        public ToPlayerPrefsSavingSystem(IKeyGenerator keyGenerator, Encryptor encryptor) :
            base(keyGenerator, encryptor) { }

        protected override string GetSavedDataPath(string savedDataName) => savedDataName;

        protected async override UniTask SaveAsync(ISavable entity)
        {
            string state = entity.GetSatate();

            byte[] data = UTF8.GetBytes(state);
            byte[] key = KeyGenerator.GenerateKey(entity.SavedDataName);
            byte[] iv = KeyGenerator.GenerateIV();

            byte[] encryptedData = Encryptor.Encrypt(data, key, iv);

            List<byte> savableData = new(data.Length + iv.Length);
            savableData.AddRange(encryptedData);
            savableData.AddRange(iv);

            string savableState = Convert.ToBase64String(savableData.ToArray());
            string path = GetSavedDataPath(entity.SavedDataName);

            PlayerPrefs.SetString(path, savableState);
            await UniTask.Yield();

            MyMath.ResetMany(data, key, iv);
            OnStateSaved(entity.SavedDataName);
        }

        protected async override UniTask<bool> TryLoadAsync(ISavable entity)
        {
            string path = GetSavedDataPath(entity.SavedDataName);

            if (PlayerPrefs.HasKey(path) == true)
            {
                string loadedState = PlayerPrefs.GetString(path);
                await UniTask.Yield();

                byte[] loadedData = Convert.FromBase64String(loadedState);

                byte[] encryptedData = loadedData[..^KeyGenerator.IVSize];
                byte[] key = KeyGenerator.GenerateKey(entity.SavedDataName);
                byte[] iv = loadedData[^KeyGenerator.IVSize..];

                byte[] decryptedData = Encryptor.Decrypt(encryptedData, key, iv);
                string state = UTF8.GetString(decryptedData);
                entity.SetState(state);

                MyMath.ResetMany(decryptedData, key, iv);
                OnStateLoaded(entity.SavedDataName);

                return true;
            }

            entity.SetDefaultState();
            return false;
        }
    }
}