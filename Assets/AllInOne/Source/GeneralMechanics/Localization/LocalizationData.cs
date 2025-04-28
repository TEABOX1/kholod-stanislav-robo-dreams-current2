using System;
using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    [CreateAssetMenu(fileName = "LocalizationData", menuName = "Data/Localization/Main Asset", order = 0)]
    public class LocalizationData : ScriptableObject
    {
        [Serializable]
        public class LanguageEntry
        {
            // Better name - termValues
            public string[] terms;
        }
        
        [SerializeField, HideInInspector] LanguageEntry[] _languageEntries;
        
        [SerializeField, HideInInspector] private List<string> _terms;
        [SerializeField, HideInInspector] private List<string> _languages;
        
        public List<string> Terms => _terms;
        public List<string> Languages => _languages;
        public LanguageEntry[] LanguageEntries => _languageEntries;
    }
}