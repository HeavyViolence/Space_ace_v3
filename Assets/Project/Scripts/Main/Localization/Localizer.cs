using Cysharp.Threading.Tasks;

using Newtonsoft.Json;

using SpaceAce.Auxiliary.EventArguments;
using SpaceAce.Main.Saving;

using System;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

using Zenject;

namespace SpaceAce.Main.Localization
{
    public sealed class Localizer : IInitializable, IDisposable, ISavable
    {
        public event EventHandler LanguageChanged, SavingRequested;
        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;

        private readonly SavingSystem _savingSystem;
        private readonly LocalizedFont _regularFont;

        private bool _initialized = false;

        public Language SelectedLanguage { get; private set; } = LocalizationTools.DefaultLanguage;
        public string SavedDataName => "Localization settings";

        public Localizer(LocalizerConfig config, SavingSystem savingSystem)
        {
            if (config == null)
            {
                throw new ArgumentNullException();
            }

            SelectedLanguage = config.InitialLanguage;
            _regularFont = config.RegularFont ?? throw new ArgumentNullException();
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
        }

        public async UniTask SetLanguageAsync(Language language)
        {
            if (_initialized == true && language == SelectedLanguage)
            {
                return;
            }

            if (_initialized == false)
            {
                var operation = LocalizationSettings.InitializationOperation;
                await UniTask.WaitUntil(() => operation.IsDone == true);
            }

            Language previouslySelectedLanguage = SelectedLanguage;
            string selectedLanguageCode = LocalizationTools.GetLanguageCode(language);

            foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
            {
                if (locale.Identifier.Code == selectedLanguageCode)
                {
                    LocalizationSettings.SelectedLocale = locale;
                    SelectedLanguage = language;
                }
            }

            if (previouslySelectedLanguage != language)
            {
                SavingRequested?.Invoke(this, EventArgs.Empty);
                LanguageChanged?.Invoke(this, EventArgs.Empty);
            }

            if (_initialized == false)
            {
                _initialized = true;
            }
        }

        public async UniTask<Font> GetLocalizedRegularFontAsync()
        {
            var operation = _regularFont.LoadAssetAsync();
            await UniTask.WaitUntil(() => operation.IsDone == true);

            return operation.Result;
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

            LocalizedString localizedString = new(tableName, entryName) { Arguments = args };

            var operation = localizedString.GetLocalizedStringAsync();
            await UniTask.WaitUntil(() => operation.IsDone == true);

            return operation.Result;
        }

        #region interfaces

        public void Initialize()
        {
            _savingSystem.Register(this);
            SetLanguageAsync(Language.EnglishUnitedStates).Forget();

            LocalizationSettings.SelectedLocaleChanged += (_) => LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _savingSystem.Deregister(this);

            LocalizationSettings.SelectedLocaleChanged -= (_) => LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public string GetState() =>
            JsonConvert.SerializeObject(SelectedLanguage, Formatting.Indented);

        public void SetState(string state)
        {
            try
            {
                Language language = JsonConvert.DeserializeObject<Language>(state);
                SetLanguageAsync(language).Forget();
            }
            catch (Exception ex)
            {
                SetLanguageAsync(LocalizationTools.DefaultLanguage).Forget();
                ErrorOccurred?.Invoke(this, new ErrorOccurredEventArgs(ex));
            }
        }

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as ISavable);

        public bool Equals(ISavable other) =>
            other is not null && SavedDataName == other.SavedDataName;

        public override int GetHashCode() => SavedDataName.GetHashCode();

        #endregion
    }
}