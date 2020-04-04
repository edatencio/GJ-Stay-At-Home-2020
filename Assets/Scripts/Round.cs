using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System;
public class Round : MonoBehaviour
{
    [SerializeField] private float elapsedTime;
    [SerializeField] private float roundTime;
    public int level = 0;
    public float Roundtime => roundTime;
    private bool timeOver = false;
    private bool roundFinished = false;

    public static event Action CloseTime;
    public static event Action RoundOver;

    public void StartRound()
    {
        var stats = RoundManager.instance.GetRound();
        roundTime = stats.roundTime;
        timeOver = false;
        roundFinished = false;
        elapsedTime = 0;
        level++;
    }
    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime / roundTime;
        if (elapsedTime >= 1)
        {
            if (!timeOver)
            {
                CloseTime?.Invoke();
                timeOver = true;
            }
        }
        if (timeOver)
        {
            if (Restaurant.instance.ClientsInRestaurant.Count <= 0)
            {
                Invoke(nameof(FinishRound), 3f);
            }
        }

    }

    private void FinishRound()
    {
        if (!roundFinished)
        {
            roundFinished = true;
            RoundOver?.Invoke();
        }
    }

    public float calculateDay()
    {
        return 0;
    }
}
