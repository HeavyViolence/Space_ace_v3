using UnityEngine;

using Zenject;

namespace SpaceAce.Main.Saving
{
    public sealed class SavingSystemInstaller : MonoInstaller
    {
        [SerializeField]
        private SavingSystemType _savingSystemType;

        [SerializeField]
        private KeyGenerationType _keyGenerationType;

        [SerializeField]
        private EncryptionType _encryptionType;

        public override void InstallBindings()
        {
            IKeyGenerator keyGenerator = SelectKeyGenerator(_keyGenerationType);
            IKeyValidator keyValidator = SelectKeyValidator(_encryptionType);
            Encryptor encryptor = SelectEncryptor(_encryptionType, keyValidator);
            SavingSystem savingSystem = SelectSavingSystem(_savingSystemType, keyGenerator, encryptor);

            Container.BindInterfacesAndSelfTo<SavingSystem>()
                     .FromInstance(savingSystem)
                     .AsSingle()
                     .NonLazy();
        }

        private IKeyGenerator SelectKeyGenerator(KeyGenerationType type)
        {
            return type switch
            {
                KeyGenerationType.Blank => new BlankKeyGenerator(),
                KeyGenerationType.Hash => new HashKeyGenerator(),
                KeyGenerationType.AES => new AESKeyGenerator(),
                _ => new BlankKeyGenerator()
            };
        }

        private IKeyValidator SelectKeyValidator(EncryptionType type)
        {
            return type switch
            {
                EncryptionType.None => new BlankKeyValidator(),
                EncryptionType.XOR => new HashKeyValidator(),
                EncryptionType.PrimeTransform => new HashKeyValidator(),
                EncryptionType.AES => new AESKeyValidator(),
                _ => new BlankKeyValidator()
            };
        }

        private Encryptor SelectEncryptor(EncryptionType type, IKeyValidator validator)
        {
            return type switch
            {
                EncryptionType.None => new BlankEncryptor(validator),
                EncryptionType.XOR => new XOREncryptor(validator),
                EncryptionType.PrimeTransform => new PrimeTransformEncryptor(validator),
                EncryptionType.AES => new AESEncryptor(validator),
                _ => new BlankEncryptor(validator)
            };
        }

        private SavingSystem SelectSavingSystem(SavingSystemType type, IKeyGenerator keyGenerator, Encryptor encryptor)
        {
            return type switch
            {
                SavingSystemType.ToFile => new ToFileSavingSystem(keyGenerator, encryptor),
                SavingSystemType.ToPlayerPrefs => new ToPlayerPrefsSavingSystem(keyGenerator, encryptor),
                _ => new ToFileSavingSystem(keyGenerator, encryptor)
            };
        }
    }
}