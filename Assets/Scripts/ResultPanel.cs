using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    public TextMeshProUGUI displaySatisfaction;
    public TextMeshProUGUI displayCostumers;
    public TextMeshProUGUI displayMoney;
    private void OnEnable()
    {
      displaySatisfaction.text = Restaurant.instance?.SatisfactionTotal.ToString();   
      displayMoney.text = "TODO";   
      displayCostumers.text = Restaurant.instance?.clientCount.ToString("D2");   
    }

}
