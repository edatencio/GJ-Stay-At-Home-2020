using UnityEngine;
using System;
using UnityEngine.UI;
public class Round : MonoBehaviour
{
    [SerializeField] private float elapsedTime;
    [SerializeField] private float roundTime;
    [SerializeField] private Image watch;
    public int level = 0;
    public float Roundtime => roundTime;
    private bool timeOver = false;
    private bool roundFinished = false;
    private bool isRunning;

    public static event Action RoundStart;
    public static event Action CloseTime;
    public static event Action RoundOver;

    public void StartRound()
    {
        Invoke(nameof(StartWithDelay), 2f);
    }
    private void StartWithDelay()
    {
        var stats = RoundManager.instance.GetRound();
        roundTime = stats.roundTime;
        timeOver = false;
        roundFinished = false;
        elapsedTime = 0;
        level++;
        isRunning = true;
        RoundStart?.Invoke();
    }
    private void Awake()
    {
        AdministrationSystem.OnClose += StartRound;
    }
    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        if (!isRunning) return;
        if (elapsedTime >= 1)
        {
            if (!timeOver)
            {
                Debug.Log("Time is Over!");
                CloseTime?.Invoke();
                timeOver = true;
            }
        }
        else
        {
            elapsedTime += Time.deltaTime / roundTime;
            watch.fillAmount = elapsedTime;
        }
        if (timeOver)
        {
            if (Restaurant.instance.ClientsInRestaurant.Count <= 0)
            {
                Invoke(nameof(FinishRound), 3f);
                isRunning = false;
            }
        }
    }

    private void FinishRound()
    {
        if (!roundFinished)
        {
            roundFinished = true;
            Debug.Log("Round is Over!");
            RoundOver?.Invoke();
        }
    }

    public float calculateDay()
    {
        return 0;
    }
}
