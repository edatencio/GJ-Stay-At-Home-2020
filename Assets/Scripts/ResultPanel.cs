using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    public TextMeshProUGUI displaySatisfaction;
    public TextMeshProUGUI displayCostumers;
    public TextMeshProUGUI displayMoney;
    private void OnEnable()
    {
        displayCostumers.text = Restaurant.instance?.clientCount.ToString("D2");
        var percentage = (Restaurant.instance?.SatisfactionTotal / Restaurant.instance?.clientCount) * 100;

        if(percentage == null) return;

        displaySatisfaction.text = ((float)percentage).ToString("F2") + "%";
        displayMoney.text = Restaurant.instance?.RoundMoney.ToString("F0") + "$";
    }

}
