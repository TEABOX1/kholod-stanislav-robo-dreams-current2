using System;
using UnityEngine;

namespace AllInOne
{
    public interface IInteractable
    {
        event Action<IInteractable> onDestroy;
        InteractableType Type { get; }
        Vector3 Position { get; }
        void Highlight(bool active);
        void Interact();
    }
}