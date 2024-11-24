namespace SpaceAce.Main.Localization
{
    public static class LocalizationTools
    {
        public static Language DefaultLanguage =>
            Language.EnglishUnitedStates;

        public static string DefaultLanguageCode =>
            GetLanguageCode(DefaultLanguage);

        public static string GetLanguageCode(Language language)
        {
            return language switch
            {
                Language.EnglishUnitedStates => "en-US",
                Language.Russian => "ru-RU",
                _ => "en-US"
            };
        }
    }
}