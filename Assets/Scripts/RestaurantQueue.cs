using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class RestaurantQueue : MonoBehaviour
{
    private List<ClientGroup> clientQueue = new List<ClientGroup>();
    public float delayBetweenClient = 5f;
    [SerializeField]private bool canAdd = true;
    public int MaxCapacity = 5;
    private void Awake()
    {
        Round.CloseTime += CloseQueue;
        Round.RoundStart += StartQueue;
        Round.RoundOver += Clean;
        Restaurant.ClientLeave += OnclientLeave;
    }
    private void OnDestroy()
    {
        Round.CloseTime -= CloseQueue;
        Round.RoundStart -= StartQueue;
        Round.RoundOver -= Clean;
        Restaurant.ClientLeave -= OnclientLeave;
    }
    private void Clean()
    {
        foreach (var item in clientQueue)
        {
            Destroy(item);
        }
    }

    private void StartQueue()
    {
        clientQueue = new List<ClientGroup>();
        canAdd = true;
    }

    private void CloseQueue()
    {
        canAdd = false;
        StopCoroutine(AddToTheQueue());
    }

    private void Update()
    {
        if (Restaurant.instance.ClientsInRestaurant.Count < MaxCapacity && canAdd)
            StartCoroutine(AddToTheQueue());
    }

    IEnumerator AddToTheQueue()
    {
        canAdd = false;
        yield return new WaitForSeconds(delayBetweenClient);

        canAdd = true;
        var prefabs = RoundManager.instance.CurrentRoundStats.clientsPrefabs;
        var client = Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Count)], transform);
        clientQueue.Add(client);
        Restaurant.instance.AddClient(client);
    }

    private void OnclientLeave(ClientGroup client)
    {
        clientQueue.Remove(client);
    }

    private void LateUpdate()
    {
        UpdatePos();

    }
    //TODO:Find other way to call it
    private void UpdatePos()
    {
        for (int i = 0; i < clientQueue.Count; i++)
        {
            if (clientQueue[i].isDragging)
            {
                continue;
            }
            if (clientQueue[i].IsSitting)
            {
                clientQueue.Remove(clientQueue[i]);
                continue;
            }
            clientQueue[i].transform.localPosition = clientQueue[i].transform.localPosition.With(z: -1.5f * i);
        }
    }
}
