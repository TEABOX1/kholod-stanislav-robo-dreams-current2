using System;
using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    public class InventoryService : MonoServiceBase, IInventoryService
    {
        [SerializeField] private ItemLibrary _itemLibrary;

        [SerializeField] private ItemSaveData[] _startingItems;
        [SerializeField] private InventoryView _inventoryView;
        
        private Inventory _inventory;

        private bool _inventoryOpened;
        private ISaveService _saveService;

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
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();

            if (_saveService.SaveData.items != null && _saveService.SaveData.items.Length > 0)
            {
                _startingItems = _saveService.SaveData.items;
            }

            if (_startingItems != null)
            {
                for (int i = 0; i < _startingItems.Length; ++i)
                {
                    ItemSaveData itemData = _startingItems[i];
                    _inventory.Add(itemData.id, itemData.count);
                }
            }

            HideInventory();
        }

        protected override void OnDestroy()
        {
            _saveService.SaveData.items = _inventory.GetAllItemsForSave();

            _saveService.SaveAll();

            base.OnDestroy();
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