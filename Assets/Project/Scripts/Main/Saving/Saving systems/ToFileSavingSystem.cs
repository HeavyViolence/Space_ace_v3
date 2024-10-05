using Cysharp.Threading.Tasks;

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

            string path = GetSavedDataPath(entity.SavedDataName);
            await File.WriteAllBytesAsync(path, savableData.ToArray());

            MyMath.ResetMany(data, key, iv);
            OnStateSaved(entity.SavedDataName);
        }

        protected async override UniTask<bool> TryLoadAsync(ISavable entity)
        {
            string path = GetSavedDataPath(entity.SavedDataName);

            if (File.Exists(path) == true)
            {
                byte[] loadedData = await File.ReadAllBytesAsync(path);

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