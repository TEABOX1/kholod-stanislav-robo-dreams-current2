using UnityEngine;

namespace AllInOne
{
    public interface IInteractableService : IService
    {
        void AddInteractable(Collider collider, IInteractable interactable);
        void RemoveInteractable(Collider collider, IInteractable interactable);

        bool CanInteract(Collider collider, out IInteractable interactable);
    }
}