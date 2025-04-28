using System;
using System.Collections.Generic;

namespace AllInOne
{
    public interface ILocalizationService : IService
    {
        event Action OnLanguageChanged;
        void SetLanguage(string language);
        string GetCurrentLanguage();
        List<string> GetSupportedLanguages();
        string GetTermValue(string term);
    }
}