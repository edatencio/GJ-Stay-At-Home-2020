using UnityEngine;
using TMPro;

public class RoundStarter : MonoBehaviour
{
    [SerializeField] private Round round;
    public float delayBtwRounds;

    public TextMeshProUGUI display;
    private float elapsedTime;
    private bool started;

    private void Start()
    {
        AdministrationSystem.OnClose += InitTimer;
        InitTimer();
    }
    private void OnDestroy()
    {
        AdministrationSystem.OnClose -= InitTimer;

    }
    public void InitTimer()
    {
        started = true;
        display.gameObject.SetActive(true);
        elapsedTime = delayBtwRounds;
    }
    private void Update()
    {
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