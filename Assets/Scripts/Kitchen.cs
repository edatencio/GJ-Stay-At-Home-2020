using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class Kitchen : Interactable
{
    [SerializeField] private GameObject familyModel;
    [SerializeField] private float cookTime;
    [SerializeField] private KitchenClock clock;
    [SerializeField] private Transform itemTarget;
    [SerializeField] private bool helpThisKitchen;

    //[SerializeField] private TextMeshPro textMesh;

    public override Type ItemType => typeof(Order);

    private void Start()
    {
        clock.cookTime = cookTime;
        clock.gameObject.SetActive(false);
        State = InteractableState.Receive;
    }

    private void OnEnable()
    {
        if (FamilyImprover.familyState == FamilyState.AllKitchen)
        {
            familyModel.SetActive(true);
            cookTime = 3;
        }
        else if (FamilyImprover.familyState == FamilyState.OneKitchen && helpThisKitchen)
        {
            familyModel.SetActive(true);
            cookTime = 3;
        }
        else
            familyModel.SetActive(false);
    }

    //private void Update()
    //{
    //    textMesh.text = State.ToString();
    //}

    // Had to override SetItem to check if order is cooked
    public override bool SetItem(IInteractableItem item)
    {
        if (State == InteractableState.Receive && SameType(ItemType, item.GetType()) && !(item as Order).IsCooked)
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
        if (CurrentItem.gameObject == null) return;

        CurrentItem.transform.position = itemTarget.position;

        StartCoroutine(Cook());
    }

    private IEnumerator Cook()
    {
        clock.gameObject.SetActive(true);
        State = InteractableState.Idle;
        yield return new WaitForSeconds(cookTime);
        if (CurrentItem != null)
            (CurrentItem as Order).IsCooked = true;

        clock.gameObject.SetActive(false);
        State = InteractableState.Emit;
    }
}

