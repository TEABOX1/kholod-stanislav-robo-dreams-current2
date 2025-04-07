using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AllInOne
{
    public class TradeListEntry : MonoBehaviour, IPointerClickHandler
    {
        public const float CLICK_DELAY = 0.2f;

        public event Action<TradeTable.TradeEntry> onPressed;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemCount;
        [SerializeField] private TextMeshProUGUI _itemTradeCount;
        [SerializeField] private TextMeshProUGUI _currency;
        [SerializeField] private TextMeshProUGUI _currencyAmount;
        [SerializeField] private CanvasGroup _priceGroup;

        private float _firstClickTime;

        private TradeTable.TradeEntry _tradeEntry;
        private ItemEntry _itemEntry;

        private IInventoryService _inventoryService;

        private IInventoryService InventoryService
        {
            get
            {
                if (_inventoryService == null)
                {
                    _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();
                }
                return _inventoryService;
            }
        }

        public void EnablePrice(bool enabled)
        {
            _priceGroup.alpha = enabled ? 1 : 0;
        }

        public void SetEnabled(bool enabled)
        {
            _canvasGroup.interactable = enabled;
            _canvasGroup.alpha = enabled ? 1f : 0.25f;
        }

        public void SetItem(ItemEntry itemEntry)
        {
            _itemEntry = itemEntry;
            if (!InventoryService.ItemLibrary.TryGetItem(itemEntry.Item.Id, out IItem product))
            {
                Debug.LogError($"Could not find product: {itemEntry.Item.Id}");
                return;
            }
            _itemName.text = product.Name;
            _itemCount.text = _itemEntry.Count.ToString();
        }

        public void SetTrade(TradeTable.TradeEntry trade, ItemEntry itemEntry)
        {
            _tradeEntry = trade;
            SetItem(itemEntry);
            _itemTradeCount.text = _tradeEntry.productAmount.ToString();
            if (!InventoryService.ItemLibrary.TryGetItem(_tradeEntry.paymentId, out IItem payment))
            {
                Debug.LogError($"Could not find payment: {_tradeEntry.paymentId}");
                return;
            }
            _currency.text = payment.Name;
            _currencyAmount.text = _tradeEntry.paymentAmount.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            float time = Time.time;
            if (_firstClickTime + CLICK_DELAY < time)
            {
                _firstClickTime = time;
            }
            else
            {
                onPressed?.Invoke(_tradeEntry);
            }
        }
    }
}