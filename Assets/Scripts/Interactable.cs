using UnityEngine;
using NaughtyAttributes;
using System;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Transform navMeshTarget;
    protected IInteractableItem currentItem;

    abstract protected Type itemType { get; }

    public enum InteractableState { Idle, Emit, Receive }

    [ShowNativeProperty]
    public InteractableState State { get; protected set; }

    public Transform NavMeshTarget => navMeshTarget;

    protected abstract void OnItemSet();

    protected abstract void OnItemGet();

    public virtual bool SetItem(IInteractableItem item)
    {
        if (State == InteractableState.Receive && SameType(itemType, item.GetType()))
        {
            currentItem = item;
            OnItemSet();
            return true;
        }

        return false;
    }

    public bool TryGetItem<T>(out IInteractableItem item)
    {
        if (currentItem is T)
        {
            item = currentItem;
            currentItem = null;
            OnItemGet();
            return true;
        }

        item = null;
        return false;
    }

    protected bool SameType(Type type1, Type type2)
    {
        return type1.IsAssignableFrom(type2) || type2.IsAssignableFrom(type1) || type1 == type2;
    }
}
