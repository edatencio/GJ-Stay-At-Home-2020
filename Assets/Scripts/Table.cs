using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;
using System;

public class Table : Interactable
{
    [SerializeField] private Transform itemTarget;
    [SerializeField] private Transform satisfactionSliderTarget;
    [SerializeField, ReorderableList] private List<Transform> seats;
    public ClientGroup clientGroup { get; private set; }

    public int SeatCount => seats.Count;

    public bool IsTaken { get; private set; }

    protected override Type itemType => typeof(IInteractableItem);

    private void Start()
    {
        Restaurant.ClientLeave += OnClientLeave;
        State = InteractableState.Receive;
    }

    public void SetClientGroup(ClientGroup clientGroup)
    {
        this.clientGroup = clientGroup;
        IsTaken = true;

        clientGroup.SatisfactionSlider.transform.position = satisfactionSliderTarget.position;

        for (int i = 0; i < clientGroup.clients.Count; i++)
            clientGroup.clients[i].transform.position = seats[i].transform.position;
    }

    private void OnClientLeave(ClientGroup client)
    {
        if (client == clientGroup)
            IsTaken = false;
    }

    protected override void OnItemSet()
    {
        CurrentItem.transform.position = itemTarget.position;
        State = InteractableState.Emit;
    }

    protected override void OnItemGet()
    {
        State = InteractableState.Receive;
    }

    public void ClearItem()
    {
        CurrentItem = null;
    }
}
