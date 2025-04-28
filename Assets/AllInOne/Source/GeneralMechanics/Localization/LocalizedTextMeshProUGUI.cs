using System;
using TMPro;
using UnityEngine;

namespace AllInOne
{
    public class LocalizedTextMeshProUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField, LocalizationTerm] private string _term;

        private ILocalizationService _localizationService;

        private void Start()
        {
            _localizationService = ServiceLocator.Instance.GetService<ILocalizationService>();
            _textMeshProUGUI.text = _localizationService.GetTermValue(_term);

            _localizationService.OnLanguageChanged += LanguageHandler;
        }

        private void LanguageHandler()
        {
            _textMeshProUGUI.text = _localizationService.GetTermValue(_term);
        }

        private void OnDestroy()
        {
            _localizationService.OnLanguageChanged -= LanguageHandler;
        }
    }
}