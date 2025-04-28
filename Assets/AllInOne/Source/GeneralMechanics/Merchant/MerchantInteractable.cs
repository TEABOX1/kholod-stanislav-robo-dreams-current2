using System;
using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    public class MerchantInteractable : InteractableBase
    {
        public override InteractableType Type => InteractableType.Activate;

        [SerializeField] private TradeTable _tradeTable;
        [SerializeField] private ItemSaveData[] _startingItems;
        [SerializeField] private MerchantUI _merchantUI;
        
        private Inventory _inventory;
        
        private bool _active = false;

        private InputController _inputController;
        private IInventoryService _inventoryService;

        public Inventory Inventory => _inventory;
        public TradeTable TradeTable => _tradeTable;
        
        private void Start()
        {
            _inventory = new();
            
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();
            
            for (int i = 0; i < _startingItems.Length; ++i)
            {
                ItemSaveData itemData = _startingItems[i];
                _inventory.Add(itemData.id, itemData.count);
            }
            
            _tradeTable.Init();
        }

        public override void Interact()
        {
            _active = !_active;
            if (_active)
            {
                _merchantUI.Show();
                _inputController.OnEscape += EscapeHandler;

                _inputController.EnableGameplayInput(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Debug.Log("Gameplay Input Disabled, Cursor Enabled");
            }
            else
            {
                EscapeHandler();
            }
            tooltip.gameObject.SetActive(!_active);
        }

        public override void Highlight(bool active)
        {
            if (active)
            {
                base.Highlight(active);
            }
            else
            {
                _active = false;
                _merchantUI.Hide();
                tooltip.gameObject.SetActive(false);
            }
        }

        private void EscapeHandler()
        {
            _active = false;
            _inputController.OnEscape -= EscapeHandler;
            _merchantUI.Hide();
            Highlight(true);

            _inputController.EnableGameplayInput(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Debug.Log("Gameplay Input Enabled, Cursor Locked");
        }
    }
}