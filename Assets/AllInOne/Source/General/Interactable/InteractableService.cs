using System;
using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    public class InteractableService : MonoServiceBase, IInteractableService
    {
        private Dictionary<Collider, IInteractable> _interactables = new();
        
        public override Type Type { get; } = typeof(IInteractableService);

        public void AddInteractable(Collider collider, IInteractable interactable)
        {
            _interactables.Add(collider, interactable);
        }

        public void RemoveInteractable(Collider collider, IInteractable interactable)
        {
            _interactables.Remove(collider);
        }

        public bool CanInteract(Collider collider, out IInteractable interactable)
        {
            return _interactables.TryGetValue(collider, out interactable);
        }
    }
}