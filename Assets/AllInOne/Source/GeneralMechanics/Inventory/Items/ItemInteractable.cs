using UnityEngine;

namespace AllInOne
{
    public sealed class ItemInteractable : InteractableBase
    {
        [SerializeField, ItemId] private string itemId;
        [SerializeField] private int itemAmount;

        public override InteractableType Type => InteractableType.PickUp;

        public override void Interact()
        {
            ServiceLocator.Instance.GetService<IInventoryService>().Inventory.Add(itemId, itemAmount);

            base.Interact();
        }
    }
}