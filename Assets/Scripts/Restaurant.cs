using UnityEngine;
using System.Collections.Generic;
using System;

public class Restaurant : MonoBehaviour
{
    public static Restaurant instance;
    public int MaxCapacity = 5;
    public static event Action<ClientGroup> ClientLeave;
    public static event Action<float> OnLeaveTip;
    private List<ClientGroup> clientsInRestaurant = new List<ClientGroup>();
    public List<ClientGroup> ClientsInRestaurant => clientsInRestaurant;
    public float SatisfactionTotal;
    public float RoundMoney = 0;
    public float Wallet = 0;
    public int clientCount;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        Round.RoundStart += StartRound;
    }

    private void OnDestroy()
    {
        Round.RoundStart -= StartRound;

    }
    private void StartRound()
    {
        RoundMoney = 0;
    }

    public void AddClient(ClientGroup client)
    {
        ClientsInRestaurant.Add(client);
    }
    public void LeaveRestaurant(ClientGroup client, float tip)
    {
        SatisfactionTotal += client.SatisfactionAmount;
        RoundMoney += tip;

        if (client.SatisfactionAmount >= 0)
            clientCount++;

        Wallet = RoundMoney;
        ClientsInRestaurant.Remove(client);
        ClientLeave?.Invoke(client);
        OnLeaveTip?.Invoke(tip);
        Destroy(client.gameObject);
    }
}
