using MessagePack;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SpaceAce.Main.Saving
{
    public sealed class ToPlayerPrefsSavingSystem : SavingSystem
    {
        protected override string SavePath => "Space ace";

        public ToPlayerPrefsSavingSystem(KeyGenerator keyGenerator, Encryptor encryptor) :
            base(keyGenerator, encryptor) { }

        protected override void Save(IEnumerable<KeyValuePair<int, byte[]>> states)
        {
            byte[] data = MessagePackSerializer.Serialize(states);
            string dataAsBase64 = Convert.ToBase64String(data);

            PlayerPrefs.SetString(SavePath, dataAsBase64);
        }

        protected override bool TryLoad(out IEnumerable<KeyValuePair<int, byte[]>> states)
        {
            if (PlayerPrefs.HasKey(SavePath) == true)
            {
                string dataAsBase64 = PlayerPrefs.GetString(SavePath);
                byte[] data = Convert.FromBase64String(dataAsBase64);

                states = MessagePackSerializer.Deserialize<IEnumerable<KeyValuePair<int, byte[]>>>(data);
                return true;
            }

            states = Enumerable.Empty<KeyValuePair<int, byte[]>>();
            return false;
        }
    }
}