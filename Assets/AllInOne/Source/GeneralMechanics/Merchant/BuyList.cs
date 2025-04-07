using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AllInOne
{
    public class BuyList : MonoBehaviour
    {
        public event Action onInventriesUpdate;

        [SerializeField] private TradeListEntry _tradeListEntryPrefab;
        [SerializeField] private Transform _content;

        private readonly List<TradeListEntry> _buyListEntries = new();

        private Dictionary<IItem, TradeTable.TradeEntry> _tradeTable;
        private Inventory _merchantInventory;
        private Inventory _buyerInventory;

        private IInventoryService _inventoryService;

        private void Start()
        {
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();
        }

        public void Open(Dictionary<IItem, TradeTable.TradeEntry> tradeTable,
            Inventory merchantInventory, Inventory buyerInventory)
        {
            _tradeTable = tradeTable;
            _merchantInventory = merchantInventory;
            _buyerInventory = buyerInventory;

            UpdateList();
        }

        public void UpdateList()
        {
            for (int i = 0; i < _buyListEntries.Count; ++i)
            {
                Destroy(_buyListEntries[i].gameObject);
            }
            _buyListEntries.Clear();

            for (int i = 0; i < _merchantInventory.Count; ++i)
            {
                if (_merchantInventory[i].Item.Id == "Money")
                    continue;
                ItemEntry item = _merchantInventory[i];
                TradeListEntry tradeListEntry = Instantiate(_tradeListEntryPrefab, _content);
                tradeListEntry.gameObject.SetActive(true);
                if (_tradeTable.TryGetValue(item.Item, out TradeTable.TradeEntry tradeEntry))
                {
                    tradeListEntry.SetTrade(tradeEntry, item);
                    tradeListEntry.EnablePrice(true);

                    if (!_inventoryService.ItemLibrary.TryGetItem(tradeEntry.paymentId, out IItem payment))
                    {
                        Debug.LogError($"Payment item {tradeEntry.paymentId} not found");
                        tradeListEntry.SetEnabled(false);
                    }
                    else
                    {
                        bool hasProduct = false, hasPayment = false;

                        if (_merchantInventory.TryGetItemEntry(item.Item, out List<ItemEntry> productEntryList))
                        {
                            int total = 0;
                            for (int j = 0; j < productEntryList.Count; ++j)
                            {
                                total += productEntryList[j].Count;
                            }

                            hasProduct = total >= tradeEntry.productAmount;
                        }

                        if (_buyerInventory.TryGetItemEntry(payment, out List<ItemEntry> paymentEntryList))
                        {
                            int total = 0;
                            for (int j = 0; j < paymentEntryList.Count; ++j)
                            {
                                total += paymentEntryList[j].Count;
                            }

                            hasPayment = total >= tradeEntry.paymentAmount;
                        }

                        tradeListEntry.SetEnabled(hasProduct && hasPayment);
                    }

                    tradeListEntry.onPressed += EntryPressed;
                }
                else
                {
                    tradeListEntry.SetItem(item);
                    tradeListEntry.EnablePrice(false);
                    tradeListEntry.SetEnabled(false);
                }
                _buyListEntries.Add(tradeListEntry);
            }
        }

        private void EntryPressed(TradeTable.TradeEntry entry)
        {
            Debug.Log($"Processing entry: {entry.productAmount} of {entry.productId} for {entry.paymentAmount} of {entry.paymentId}");
            if (entry.productId == "Money")
                return;
            if (_buyerInventory.Remove(entry.paymentId, entry.paymentAmount))
            {
                _merchantInventory.Add(entry.paymentId, entry.paymentAmount);
                _ = _merchantInventory.Remove(entry.productId, entry.productAmount);
                _buyerInventory.Add(entry.productId, entry.productAmount);
                onInventriesUpdate?.Invoke();
                Debug.Log($"Payment processed");
            }
        }

        private void CompleteTrade(TradeTable.TradeEntry trade)
        {
            //ItemEntry product = _merchantInventory.Find(item => FindProductPredicate(trade, item));
            //ItemEntry payment = _buyerInventory.Find(item => BuyPredicate(trade, item));
        }

        public bool FindProductPredicate(TradeTable.TradeEntry trade, ItemEntry item)
        {
            return trade.productId == item.Item.Id;
        }

        public bool HasItemPredicate(ItemEntry entry, IItem item)
        {
            return entry.Item == item;
        }
    }
}