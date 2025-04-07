using System;
using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    public class InventoryService : MonoServiceBase, IInventoryService
    {
        [Serializable]
        public struct ItemData
        {
            [ItemId] public string id;
            public int count;
        }
        
        [SerializeField] private ItemLibrary _itemLibrary;

        [SerializeField] private ItemData[] _startingItems;
        [SerializeField] private InventoryView _inventoryView;
        
        private Inventory _inventory;

        private bool _inventoryOpened;

        private bool InventoryOpened
        {
            get => _inventoryOpened;
            set
            {
                _inventoryOpened = value;

                if (_inventoryOpened)
                {
                    _inventoryView.Show();
                }
                else
                {
                    _inventoryView.Hide();
                }
                
                InputController inputController = ServiceLocator.Instance.GetService<InputController>();
                if (inputController != null)
                    inputController.enabled = !_inventoryOpened;
            }
        }
        
        public override Type Type { get; } = typeof(IInventoryService);

        public Inventory Inventory => _inventory;
        public ItemLibrary ItemLibrary => _itemLibrary;
        
        protected override void Awake()
        {
            base.Awake();

            _itemLibrary.Init();
        }

        private void Start()
        {
            _inventory = new();
            for (int i = 0; i < _startingItems.Length; ++i)
            {
                ItemData itemData = _startingItems[i];
                _inventory.Add(itemData.id, itemData.count);
            }
            
            HideInventory();
        }

        public void ShowInventory()
        {
            InventoryOpened = true;
        }

        public void HideInventory()
        {
            InventoryOpened = false;
        }

        public void ToggleInventory()
        {
            InventoryOpened = !InventoryOpened;
        }
    }
}