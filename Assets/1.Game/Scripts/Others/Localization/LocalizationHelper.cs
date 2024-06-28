using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TrickyBrain
{
    public static class LocalizationHelper
    {
        public static string Localize(string key, Locale locale = null)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(key, locale);
        }

        public static string Localize(string key, object[] arguments, Locale locale = null)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(key, locale, arguments: arguments);
        }

        public static void AddOnLocaleChanged(Action<Locale> onChanged)
        {
            LocalizationSettings.SelectedLocaleChanged += onChanged;
        }

        public static void RemoveOnLocaleChanged(Action<Locale> onChanged)
        {
            LocalizationSettings.SelectedLocaleChanged -= onChanged;
        }

        public static void ChangeLocale(int index)
        {
            if(index < 0 || index >= LocalizationSettings.AvailableLocales.Locales.Count)
            {
                return;
            }
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        }

        public static void ChangeLocale(string code)
        {
            foreach(var locale in LocalizationSettings.AvailableLocales.Locales)
            {
                if(locale.Identifier.Code == code)
                {
                    LocalizationSettings.SelectedLocale = locale;
                    break;
                }
            }
        }
    }
}