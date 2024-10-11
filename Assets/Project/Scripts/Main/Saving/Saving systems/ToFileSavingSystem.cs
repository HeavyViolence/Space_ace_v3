using SpaceAce.Auxiliary;

using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace SpaceAce.Main.Saving
{
    public sealed class ToFileSavingSystem : SavingSystem
    {
        private const string SaveFileExtension = ".save";

        public ToFileSavingSystem(IKeyGenerator keyGenerator, Encryptor encryptor) :
            base(keyGenerator, encryptor) { }

        protected override string GetSavedDataPath(string savedDataName) =>
            Path.Combine(Application.persistentDataPath, savedDataName + SaveFileExtension);

        protected override void Save(ISavable entity)
        {
            string state = entity.GetSatate();

            byte[] data = UTF8.GetBytes(state);
            byte[] key = KeyGenerator.GenerateKey(entity.SavedDataName);
            byte[] iv = KeyGenerator.GenerateIV();

            byte[] encryptedData = Encryptor.Encrypt(data, key, iv);

            List<byte> savableData = new(data.Length + iv.Length);
            savableData.AddRange(encryptedData);
            savableData.AddRange(iv);

            string path = GetSavedDataPath(entity.SavedDataName);
            File.WriteAllBytes(path, savableData.ToArray());

            MyMath.ResetMany(data, key, iv);
        }

        protected override bool TryLoad(ISavable entity)
        {
            string path = GetSavedDataPath(entity.SavedDataName);

            if (File.Exists(path) == true)
            {
                byte[] loadedData = File.ReadAllBytes(path);

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