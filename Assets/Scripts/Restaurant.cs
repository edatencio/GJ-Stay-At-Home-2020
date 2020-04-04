using UnityEngine;
using System.Collections.Generic;
using System;

public class Restaurant : MonoBehaviour
{
    public static Restaurant instance;

    public static event Action ClientLeave;
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

    public void AddClient(ClientGroup client)
    {
        ClientsInRestaurant.Add(client);
    }
    public void LeaveRestaurant(ClientGroup client)
    {
        SatisfactionTotal += client.SatisfactionAmount;
        ClientsInRestaurant.Remove(client);
        Destroy(client.gameObject,1f);
        ClientLeave?.Invoke();
    }
}
