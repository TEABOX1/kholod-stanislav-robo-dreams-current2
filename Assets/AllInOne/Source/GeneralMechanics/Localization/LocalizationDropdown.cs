using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AllInOne
{
    public class LocalizationDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;

        private ILocalizationService _localizationService;
        private List<string> _localizationKeys;
        
        private void Start()
        {
            _localizationService = ServiceLocator.Instance.GetService<ILocalizationService>();
            _localizationKeys = _localizationService.GetSupportedLanguages();
            _dropdown.ClearOptions();
            _dropdown.AddOptions(_localizationKeys);
            _dropdown.SetValueWithoutNotify(_localizationKeys.IndexOf(_localizationService.GetCurrentLanguage()));
            _dropdown.onValueChanged.AddListener(DropdownHandler);
        }

        private void DropdownHandler(int option)
        {
            _localizationService.SetLanguage(_localizationKeys[option]);
        }
    }
}