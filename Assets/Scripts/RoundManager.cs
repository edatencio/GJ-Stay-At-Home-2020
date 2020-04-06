using UnityEngine;
using System.Collections.Generic;
using System;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    [SerializeField] private RoundStats DefaultRound;
    [SerializeField] private List<RoundStats> rounds;
    private Queue<RoundStats> roundQueue = new Queue<RoundStats>();
    public Queue<RoundStats> RoundQueue => roundQueue;
    public RoundStats CurrentRoundStats { get; private set; }
    public event Action NoMoreRounds;
    public event Action NextRound;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


    }
    public void Start()
    {
        foreach (var item in rounds)
        {
            roundQueue.Enqueue(item);
        }
    }
    public RoundStats GetRound()
    {
        if (roundQueue.Count > 0)
        {
            CurrentRoundStats = roundQueue.Dequeue();
            NextRound?.Invoke();

            return CurrentRoundStats;
        }
        else
        {
            return DefaultRound;
        }
    }

}
