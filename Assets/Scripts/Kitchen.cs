using UnityEngine;
using System.Collections;
using System;

public class Kitchen : Interactable
{
    [SerializeField] private float cookTime;
    [SerializeField] private GameObject clock;
    [SerializeField] private Transform itemTarget;

    protected override Type itemType => typeof(Order);

    private void Start()
    {
        clock.SetActive(false);
        State = InteractableState.Receive;
    }

    // Had to override SetItem to check if order is cooked
    public override bool SetItem(IInteractableItem item)
    {
        if (State == InteractableState.Receive && SameType(itemType, item.GetType()) && !(item as Order).IsCooked)
        {
            currentItem = item;
            OnItemSet();
            return true;
        }

        return false;
    }

    protected override void OnItemGet()
    {
        State = InteractableState.Receive;
    }

    protected override void OnItemSet()
    {
        currentItem.Model.transform.position = itemTarget.position;
        StartCoroutine(Cook());
    }

    private IEnumerator Cook()
    {
        clock.SetActive(true);
        State = InteractableState.Idle;
        yield return new WaitForSeconds(cookTime);
        (currentItem as Order).IsCooked = true;
        clock.SetActive(false);
        State = InteractableState.Emit;
    }
}

