using System;
using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    [CreateAssetMenu(fileName = "TradeTable", menuName = "Data/Items/Trade Table", order = 0)]
    public class TradeTable : ScriptableObject
    {
        [Serializable]
        public class TradeEntry
        {
            [ItemId] public string paymentId;
            [NonSerialized] public IItem payment;
            public int paymentAmount;
            [ItemId] public string productId;
            [NonSerialized] public IItem product;
            public int productAmount;
        }

        [SerializeField] private List<TradeEntry> buyList;
        [SerializeField] private List<TradeEntry> sellList;

        public List<TradeEntry> BuyList => buyList;
        public List<TradeEntry> SellList => sellList;

        private readonly Dictionary<IItem, TradeEntry> _buyTable = new();
        private readonly Dictionary<IItem, TradeEntry> _sellTable = new();

        private IInventoryService _inventoryService;

        public Dictionary<IItem, TradeEntry> BuyTable => _buyTable;
        public Dictionary<IItem, TradeEntry> SellTable => _sellTable;

        public void Init()
        {
            _inventoryService = ServiceLocator.Instance?.GetService<IInventoryService>();
            if (_inventoryService == null)
                return;
            InitList(buyList, _buyTable);
            InitList(sellList, _sellTable);
        }

        private void InitList(List<TradeEntry> list, Dictionary<IItem, TradeEntry> table)
        {
            table.Clear();

            for (int i = 0; i < list.Count; ++i)
            {
                TradeEntry entry = list[i];
                if (!_inventoryService.ItemLibrary.TryGetItem(entry.paymentId, out IItem payment))
                {
                    Debug.LogError($"Item {entry.paymentId} not found");
                }

                if (!_inventoryService.ItemLibrary.TryGetItem(entry.productId, out IItem product))
                {
                    Debug.LogError($"Item {entry.productId} not found");
                }

                entry.payment = payment;
                entry.product = product;

                table.Add(product, entry);
            }
        }

        public bool TryGetBuyTrade(IItem item, out TradeEntry trade)
        {
            return _buyTable.TryGetValue(item, out trade);
        }

        public bool TryGetSellTrade(IItem item, out TradeEntry trade)
        {
            return _sellTable.TryGetValue(item, out trade);
        }
    }
}