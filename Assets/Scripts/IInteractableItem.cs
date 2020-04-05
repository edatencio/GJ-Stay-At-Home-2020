using UnityEngine;

public interface IInteractableItem
{
    GameObject gameObject { get; }

    Transform transform { get; }
}
