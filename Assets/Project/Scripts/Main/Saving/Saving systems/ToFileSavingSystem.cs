using MessagePack;

using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

namespace SpaceAce.Main.Saving
{
    public sealed class ToFileSavingSystem : SavingSystem
    {
        private const string SaveFileName = "Space ace";
        private const string SaveFileExtension = ".save";

        protected override string SavePath =>
            Path.Combine(Application.persistentDataPath, SaveFileName + SaveFileExtension);

        public ToFileSavingSystem(KeyGenerator keyGenerator, Encryptor encryptor) :
            base(keyGenerator, encryptor) { }

        protected override void Save(IEnumerable<KeyValuePair<int, byte[]>> states)
        {
            byte[] data = MessagePackSerializer.Serialize(states);
            File.WriteAllBytes(SavePath, data);
        }

        protected override bool TryLoad(out IEnumerable<KeyValuePair<int, byte[]>> states)
        {
            if (File.Exists(SavePath) == true)
            {
                byte[] data = File.ReadAllBytes(SavePath);

                states = MessagePackSerializer.Deserialize<IEnumerable<KeyValuePair<int, byte[]>>>(data);
                return true;
            }

            states = Enumerable.Empty<KeyValuePair<int, byte[]>>();
            return false;
        }
    }
}