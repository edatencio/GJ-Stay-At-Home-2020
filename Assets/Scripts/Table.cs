using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

public class Table : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform navMeshTarget;
    [ReorderableList] public List<Seat> Seats;

    public ClientGroup clientGroup { get; private set; }

    public bool IsTaken { get; private set; }

    public Transform NavMeshTarget => navMeshTarget;
    private void Start()
    {
        Restaurant.ClientLeave += OnClientLeave;
    }
    public void SetCostumer(ClientGroup clientGroup)
    {
        this.clientGroup = clientGroup;
        IsTaken = true;

        for (int i = 0; i < clientGroup.costumers.Count; i++)
            clientGroup.costumers[i].transform.position = Seats[i].transform.position;
    }

    private void OnClientLeave()
    {

    }

    private void OnDestroy()
    {
        Restaurant.ClientLeave -= OnClientLeave;

    }
    public void Interact()
    {
        Log.Message(name, "Interact");
    }
}
