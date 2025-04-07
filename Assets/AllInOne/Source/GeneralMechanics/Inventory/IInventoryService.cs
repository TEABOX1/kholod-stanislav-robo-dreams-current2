using System.Collections.Generic;

namespace AllInOne
{
    public interface IInventoryService : IService
    {
        Inventory Inventory { get; }
        void ShowInventory();
        void HideInventory();
        void ToggleInventory();
        ItemLibrary ItemLibrary { get; }
    }
}