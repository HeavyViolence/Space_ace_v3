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

        protected override void Save(ISavable entity)
        {
            string state = entity.GetState();

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
            MyMath.ResetMany(data, key, iv);
        }

        protected override bool TryLoad(ISavable entity)
        {
            string path = GetSavedDataPath(entity.SavedDataName);

            if (PlayerPrefs.HasKey(path) == true)
            {
                string loadedState = PlayerPrefs.GetString(path);
                byte[] loadedData = Convert.FromBase64String(loadedState);

                byte[] encryptedData = loadedData[..^KeyGenerator.IVSize];
                byte[] key = KeyGenerator.GenerateKey(entity.SavedDataName);
                byte[] iv = loadedData[^KeyGenerator.IVSize..];

                byte[] decryptedData = Encryptor.Decrypt(encryptedData, key, iv);
                string state = UTF8.GetString(decryptedData);
                entity.SetState(state);

                MyMath.ResetMany(decryptedData, key, iv);
                return true;
            }

            entity.SetDefaultState();
            return false;
        }
    }
}