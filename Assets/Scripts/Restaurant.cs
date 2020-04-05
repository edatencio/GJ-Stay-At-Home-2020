using UnityEngine;
using System.Collections.Generic;
using System;

public class Restaurant : MonoBehaviour
{
    public static Restaurant instance;
    public int MaxCapacity = 5;

    public static event Action<ClientGroup> ClientLeave;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private List<ClientGroup> clientsInRestaurant = new List<ClientGroup>();
    public List<ClientGroup> ClientsInRestaurant => clientsInRestaurant;
    public float SatisfactionTotal;
    public int clientCount;

    public void AddClient(ClientGroup client)
    {
        ClientsInRestaurant.Add(client);
    }
    public void LeaveRestaurant(ClientGroup client)
    {
        SatisfactionTotal += client.SatisfactionAmount;
        if (client.SatisfactionAmount >= 0)
            clientCount++;

        ClientsInRestaurant.Remove(client);
        ClientLeave?.Invoke(client);
        Destroy(client.gameObject);
    }
}
