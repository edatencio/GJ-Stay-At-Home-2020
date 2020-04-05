using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


public class Kitchen : Interactable
{
    [SerializeField] private float cookTime;
    [SerializeField] private KitchenClock clock;
    [SerializeField] private Transform itemTarget;

    protected override Type itemType => typeof(Order);

    private void Start()
    {
        clock.cookTime = cookTime;
        clock.gameObject.SetActive(false);
        State = InteractableState.Receive;
    }

    // Had to override SetItem to check if order is cooked
    public override bool SetItem(IInteractableItem item)
    {
        if (State == InteractableState.Receive && SameType(itemType, item.GetType()) && !(item as Order).IsCooked)
        {
            CurrentItem = item;
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
        CurrentItem.transform.position = itemTarget.position;
        StartCoroutine(Cook());
    }

    private IEnumerator Cook()
    {
        clock.gameObject.SetActive(true);
        State = InteractableState.Idle;
        yield return new WaitForSeconds(cookTime);
        (CurrentItem as Order).IsCooked = true;
        clock.gameObject.SetActive(false);
        State = InteractableState.Emit;
    }
}


