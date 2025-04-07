using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AllInOne
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private InventoryEntry _entryPrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private TextMeshProUGUI _description;
        
        private readonly List<InventoryEntry> _entries = new();
        
        private IInventoryService _inventoryService;
        
        private void Start()
        {
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();
        }

        public void Show()
        {
            _canvas.enabled = true;
            
            for (int i = 0; i < _entries.Count; ++i)
            {
                Destroy(_entries[i].gameObject);
            }
            _entries.Clear();

            for (int i = 0; i < _inventoryService.Inventory.Count; ++i)
            {
                if (_inventoryService.Inventory[i].Item.Id == "Money")
                    continue;
                InventoryEntry inventoryEntry = Instantiate(_entryPrefab, _content);
                inventoryEntry.gameObject.SetActive(true);
                inventoryEntry.SetItem(_inventoryService.Inventory[i]);
                inventoryEntry.onPressed += ItemPressed;
                _entries.Add(inventoryEntry);
            }
            
            _description.text = _inventoryService.Inventory[0].Item.Description;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }
        
        private void ItemPressed(ItemEntry itemEntry)
        {
            _description.text = itemEntry.Item.Description;
        }
    }
}