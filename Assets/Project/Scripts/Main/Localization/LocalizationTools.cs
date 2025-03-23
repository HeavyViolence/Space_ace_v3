using UnityEngine;

namespace SpaceAce.Main.Localization
{
    public static class LocalizationTools
    {
        public static Language DefaultLanguage => Language.English;

        public static string GetLanguageCode(Language language)
        {
            return language switch
            {
                Language.English => "en-US",
                Language.Russian => "ru-RU",
                Language.Ukrainian => "uk-UA",
                Language.Turkish => "tr-TR",
                _ => string.Empty,
            };
        }

        public static Language GetNativeLanguage()
        {
            return Application.systemLanguage switch
            {
                SystemLanguage.English => Language.English,
                SystemLanguage.Russian => Language.Russian,
                SystemLanguage.Turkish => Language.Turkish,
                SystemLanguage.Ukrainian => Language.Ukrainian,
                _ => DefaultLanguage,
            };
        }

        public static Language ValidateLanguage(Language language)
        {
            return language switch
            {
                Language.None => GetNativeLanguage(),
                _ => language,
            };
        }
    }
}