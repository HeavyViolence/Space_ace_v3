using Cysharp.Threading.Tasks;

using MessagePack;

using SpaceAce.Main.Saving;

using System;

using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Main.Localization
{
    public sealed class Localizer : IInitializable, IDisposable, ISavable
    {
        public event Action LanguageChanged, StateChanged;

        private readonly SavingSystem _savingSystem;

        private bool _initialized = false;

        public Language SelectedLanguage { get; private set; } = Language.None;
        public string StateName => "Localization settings";

        [Inject]
        public Localizer(SavingSystem savingSystem)
        {
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
        }

        public void SetLanguage(Language language)
        {
            if (language == SelectedLanguage || language == Language.None)
            {
                return;
            }

            SetLanguageInternally(language);
        }

        private void SetLanguageInternally(Language language)
        {
            string selectionCode = LocalizationTools.GetLanguageCode(language);

            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                if (locale.Identifier.Code == selectionCode)
                {
                    LocalizationSettings.SelectedLocale = locale;
                    SelectedLanguage = language;

                    StateChanged?.Invoke();
                    LanguageChanged?.Invoke();
                }
            }
        }

        public async UniTask<string> GetLocalizedStringAsync(string tableName, string entryName, params object[] args)
        {
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(entryName) || string.IsNullOrWhiteSpace(entryName))
            {
                throw new ArgumentNullException();
            }

            await EnsureLocalizationStartupAsync();

            LocalizedString localizedString = new(tableName, entryName) { Arguments = args };

            var operation = localizedString.GetLocalizedStringAsync();
            await UniTask.WaitUntil(() => operation.IsDone == true);

            return operation.Result;
        }

        private async UniTask EnsureLocalizationStartupAsync()
        {
            if (_initialized == false)
            {
                var initialization = LocalizationSettings.InitializationOperation;
                await UniTask.WaitUntil(() => initialization.IsDone == true);

                Language startupLanguage = LocalizationTools.ValidateLanguage(SelectedLanguage);
                SetLanguageInternally(startupLanguage);

                _initialized = true;
            }
        }

        #region interfaces

        public void Initialize()
        {
            _savingSystem.Register(this);
            LocalizationSettings.SelectedLocaleChanged += (_) => LanguageChanged?.Invoke();
        }

        public void Dispose()
        {
            _savingSystem.Deregister(this);
            LocalizationSettings.SelectedLocaleChanged -= (_) => LanguageChanged?.Invoke();
        }

        public byte[] GetState() => MessagePackSerializer.Serialize(SelectedLanguage);

        public void SetState(byte[] state)
        {
            try
            {
                SelectedLanguage = MessagePackSerializer.Deserialize<Language>(state);
            }
            catch (Exception)
            {
                SelectedLanguage = LocalizationTools.GetNativeLanguage();
            }
        }

        #endregion
    }
}