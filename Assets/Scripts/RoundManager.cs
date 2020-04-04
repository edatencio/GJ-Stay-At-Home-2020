using UnityEngine;
using System.Collections.Generic;
using System;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    [SerializeField] private List<RoundStats> rounds;
    private Queue<RoundStats> roundQueue = new Queue<RoundStats>();

    public Queue<RoundStats> RoundQueue => roundQueue;
    public event Action NoMoreRounds;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        foreach (var item in rounds)
        {
            roundQueue.Enqueue(item);
        }
    }
    public RoundStats GetRound()
    {
        if (roundQueue.Count > 0)
            return roundQueue.Dequeue();
        else
        {
            NoMoreRounds.Invoke();
            return null;
        }
    }

}
