using UnityEngine;

public interface IInteractable
{
    Transform NavMeshTarget { get; }

    Transform transform { get; }

    void Interact();
}
