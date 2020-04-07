using UnityEngine;
using System;

public class PlatesWasher : Interactable
{
    public override Type ItemType => typeof(PlatesDirty);

    private void Start()
    {
        State = InteractableState.Receive;
    }

    private void Update()
    {
    }

    protected override void OnItemGet()
    {
    }

    protected override void OnItemSet()
    {
        Destroy(CurrentItem.gameObject);
    }
}
