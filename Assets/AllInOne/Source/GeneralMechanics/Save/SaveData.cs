using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace AllInOne
{
    [Serializable]
    public struct SaveData
    {
        // Data related to sound
        public SoundSaveData soundData;
        // Data related to localization
        public LocalizationSaveData localizationData;
        public KDASavedata kDASavedata;
        public ItemSaveData[] items;
    }
}