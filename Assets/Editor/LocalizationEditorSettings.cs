using UnityEditor;
using UnityEngine;

namespace AllInOne
{
    [FilePath("Localization/Data/EditorSceneSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LocalizationEditorSettings : ScriptableSingleton<LocalizationEditorSettings>
    {
        [SerializeField] private LocalizationData _localizationData;

        public LocalizationData LocalizationData
        {
            get => _localizationData;
            set
            {
                if (value == _localizationData)
                    return;
                _localizationData = value;
                Save(true);
            }
        }
    }
}