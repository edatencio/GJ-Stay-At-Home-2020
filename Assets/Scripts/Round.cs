using UnityEngine;
using System;
using UnityEngine.UI;
public class Round : MonoBehaviour
{
    [SerializeField] private float elapsedTime;
    [SerializeField] private float roundTime;
    [SerializeField] private Image watch;
    [SerializeField] private TMPro.TextMeshProUGUI displayWatch;
    [SerializeField] private RoundStarter roundStarter;
    public int level = 0;
    public float Roundtime => roundTime;
    private bool timeOver = false;
    private bool roundFinished = false;
    private bool isRunning;

    public static event Action RoundStart;
    public static event Action CloseTime;
    public static event Action RoundOver;

    private void Start()
    {
        displayWatch.text = "";
        watch.fillAmount = 0;
    }
    public void StartRound()
    {
        var stats = RoundManager.instance.GetRound();
        roundTime = stats.roundTime;
        timeOver = false;
        roundFinished = false;
        elapsedTime = 0;
        level++;
        isRunning = true;
        RoundStart?.Invoke();
        watch.fillAmount = 0;
        displayWatch.text = "Abierto";


    }
    private void Update()
    {
        if (!isRunning) return;
        if (elapsedTime >= 1)
        {
            if (!timeOver)
            {
                Debug.Log("Time is Over!");
                displayWatch.text = "Cerrado";
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
