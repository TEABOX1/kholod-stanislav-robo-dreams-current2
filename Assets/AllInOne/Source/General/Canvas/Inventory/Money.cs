using UnityEngine;
using TMPro;

namespace AllInOne
{
    public class Money : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _moneyAmount;
        [SerializeField] ItemInteractable _itemInteractable;

        private IInventoryService _inventoryService;

        private void Start()
        {
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();
           // _itemInteractable.onDestroy += UpdateUI;

            for (int i = 0; i < _inventoryService.Inventory.Count; ++i)
            {
                if (_inventoryService.Inventory[i].Item.Id == "Money")
                    SetItem(_inventoryService.Inventory[i]);
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _inventoryService.Inventory.Count; ++i)
            {
                if (_inventoryService.Inventory[i].Item.Id == "Money")
                    SetItem(_inventoryService.Inventory[i]);
            }
        }

        private void UpdateUI(IInteractable interactable)
        {
            //for (int i = 0; i < _inventoryService.Inventory.Count; ++i)
            //{
            //    if (_inventoryService.Inventory[i].Item.Id == "Money")
            //    {
            //        Debug.Log("money: " + _inventoryService.Inventory[i].Count);
            //        SetItem(_inventoryService.Inventory[i]);
            //    }
            //}
        }

        public void SetItem(ItemEntry item)
        {
            _moneyAmount.text = item.Count.ToString();
        }
    }
}
