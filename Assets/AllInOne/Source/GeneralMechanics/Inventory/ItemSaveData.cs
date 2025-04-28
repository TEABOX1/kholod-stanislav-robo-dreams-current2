using System;

namespace AllInOne
{
    [Serializable]
    public struct ItemSaveData
    {
        [ItemId] public string id;
        public int count;
    }
}