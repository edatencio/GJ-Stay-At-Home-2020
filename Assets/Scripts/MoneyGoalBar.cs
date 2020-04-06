using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MoneyGoalBar : MonoBehaviour
{
    public TextMeshProUGUI GoalDisplay;
    public TextMeshProUGUI currentMoneyDisplay;
    public Image fill;
    private void Start()
    {
        Restaurant.OnLeaveTip += UpdateDisplays;
        Round.RoundStart += OnStartRound;
        fill.fillAmount = 0;
        GoalDisplay.text = "Meta: ";
        currentMoneyDisplay.text = "0$";
        RoundManager.instance.NextRound += () =>
        {
            GoalDisplay.text = "Meta: " + RoundManager.instance.CurrentRoundStats.TargetMoney + "$";
            fill.fillAmount = 0;
        };
    }

    private void OnStartRound()
    {
        currentMoneyDisplay.text = "0$";
        fill.fillAmount = 0;
    }

    private void UpdateDisplays(float tip)
    {
        var current = Restaurant.instance.RoundMoney;
        var target = RoundManager.instance.CurrentRoundStats.TargetMoney;
        currentMoneyDisplay.text = current.ToString("F0") + "$";
        fill.fillAmount = current / target;
        GoalDisplay.text = "Meta: " + target.ToString() + "$";
    }

    private void OnDestroy()
    {
        Restaurant.OnLeaveTip -= UpdateDisplays;
        Round.RoundStart -= OnStartRound;
    }
}
