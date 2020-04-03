using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<Seat> Seats;
    [HideInInspector]public ClientGroup clientGroup;
    public bool IsTaken { get; private set; }

    public void SetCostumer(ClientGroup clientGroup)
    {
        this.clientGroup = clientGroup;
        IsTaken = true;
        for (int i = 0; i < clientGroup.costumers.Count; i++)
        {
            clientGroup.costumers[i].transform.position = Seats[i].transform.position;
        }
    }
}
