using System;
using TMPro;
using UnityEngine;

namespace AllInOne
{
    public class MerchantUI : MonoBehaviour
    {
        [SerializeField] private MerchantInteractable _merchantInteractable;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private BuyList _buyList;
        [SerializeField] private BuyList _sellList;
        [SerializeField] TextMeshProUGUI _moneyAmount;

        private IInventoryService _inventoryService;

        private void Start()
        {
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();

            _buyList.onInventriesUpdate += UpdateLists;
            _sellList.onInventriesUpdate += UpdateLists;

            Hide();
        }

        private void UpdateLists()
        {
            _buyList.UpdateList();
            _sellList.UpdateList();
            UpdateMoney();
        }

        public void Show()
        {
            _canvas.enabled = true;
            _buyList.Open(_merchantInteractable.TradeTable.BuyTable, _merchantInteractable.Inventory, _inventoryService.Inventory);
            _sellList.Open(_merchantInteractable.TradeTable.SellTable, _inventoryService.Inventory, _merchantInteractable.Inventory);

            InputController inputController = ServiceLocator.Instance.GetService<InputController>();
            if (inputController != null)
                inputController.enabled = !_canvas.enabled;

            UpdateMoney();
        }

        private void UpdateMoney()
        {
            for (int i = 0; i < _merchantInteractable.Inventory.Count; ++i)
            {
                if (_merchantInteractable.Inventory[i].Item.Id != "Money")
                    continue;

                _moneyAmount.text = _merchantInteractable.Inventory[i].Count.ToString();
                return;
            }
            _moneyAmount.text = 0.ToString();
        }

        public void Hide()
        {
            InputController inputController = ServiceLocator.Instance.GetService<InputController>();
            if (inputController != null)
                inputController.enabled = !_canvas.enabled;
            _canvas.enabled = false;
        }
    }
}