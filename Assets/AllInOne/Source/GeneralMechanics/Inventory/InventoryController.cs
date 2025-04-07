using UnityEngine;

namespace AllInOne
{
    public class InventoryController : MonoBehaviour
    {
        private InputController _inputController;
        private IInventoryService _inventoryService;
        
        private void Start()
        {
            _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inventoryService = ServiceLocator.Instance.GetService<IInventoryService>();

            _inputController.OnInventory += InventoryHandler;
        }

        private void InventoryHandler()
        {
            _inventoryService.ToggleInventory();
        }
    }
}