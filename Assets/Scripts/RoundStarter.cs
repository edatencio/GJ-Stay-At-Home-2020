using UnityEngine;
using TMPro;
using System;

public class RoundStarter : MonoBehaviour
{
    [SerializeField] private Round round;
    public float delayBtwRounds;

    public TextMeshProUGUI display;
    private float elapsedTime;
    private bool started;
    Timer timer = new Timer();
    private void Start()
    {
        AdministrationSystem.OnClose += InitTimer;
        Round.RoundOver += RoundOver;
        InitTimer();
    }

    private void RoundOver()
    {
        if (RoundManager.instance.CurrentRoundStats.TargetMoney > Restaurant.instance.RoundMoney)
        {
            display.gameObject.SetActive(true);
            display.text = "Bancarrota :(";
            timer.Start();

        }
    }

    private void BackToMainMenu() => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    private void OnDestroy()
    {
        AdministrationSystem.OnClose -= InitTimer;
        Round.RoundOver -= RoundOver;

    }
    public void InitTimer()
    {
        started = true;
        display.gameObject.SetActive(true);
        elapsedTime = delayBtwRounds;
    }
    private void Update()
    {
        if (timer.ElapsedSeconds > 2f)
        {
            BackToMainMenu();
        }
        if (!started) return;

        if (elapsedTime > 1f)
        {
            elapsedTime -= Time.deltaTime;
            UpdateText();
        }
        else if (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            display.text = "A Cocinar!!";
        }
        else
        {
            started = false;
            display.gameObject.SetActive(false);
            round.StartRound();
        }
    }
    public void UpdateText()
    {
        display.text = Mathf.CeilToInt(elapsedTime).ToString();
    }

}