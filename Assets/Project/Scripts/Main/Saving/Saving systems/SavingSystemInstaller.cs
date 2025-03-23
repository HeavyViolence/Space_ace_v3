using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.Main.Saving
{
    public sealed class SavingSystemInstaller : ServiceInstaller
    {
        [SerializeField]
        private SavingSystemType _savingSystemType;

        [SerializeField]
        private EncryptionType _encryptionType;

        public override void Install(IContainerBuilder builder)
        {
            KeyGenerator keyGenerator = SelectKeyGenerator(_encryptionType);
            KeyValidator keyValidator = SelectKeyValidator(_encryptionType);
            Encryptor encryptor = SelectEncryptor(_encryptionType, keyValidator);
            SavingSystem savingSystem = SelectSavingSystem(_savingSystemType, keyGenerator, encryptor);

            builder.RegisterInstance(savingSystem).AsImplementedInterfaces().AsSelf();
        }

        private KeyGenerator SelectKeyGenerator(EncryptionType type)
        {
            return type switch
            {
                EncryptionType.None => new BlankKeyGenerator(),
                EncryptionType.XOR => new HashKeyGenerator(),
                EncryptionType.ArithmeticTransform => new HashKeyGenerator(),
                EncryptionType.AES => new AESKeyGenerator(),
                _ => new BlankKeyGenerator()
            };
        }

        private KeyValidator SelectKeyValidator(EncryptionType type)
        {
            return type switch
            {
                EncryptionType.None => new BlankKeyValidator(),
                EncryptionType.XOR => new HashKeyValidator(),
                EncryptionType.ArithmeticTransform => new HashKeyValidator(),
                EncryptionType.AES => new AESKeyValidator(),
                _ => new BlankKeyValidator()
            };
        }

        private Encryptor SelectEncryptor(EncryptionType type, KeyValidator validator)
        {
            return type switch
            {
                EncryptionType.None => new BlankEncryptor(validator),
                EncryptionType.XOR => new XOREncryptor(validator),
                EncryptionType.ArithmeticTransform => new ArithmeticTransformEncryptor(validator),
                EncryptionType.AES => new AESEncryptor(validator),
                _ => new BlankEncryptor(validator)
            };
        }

        private SavingSystem SelectSavingSystem(SavingSystemType type, KeyGenerator keyGenerator, Encryptor encryptor)
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