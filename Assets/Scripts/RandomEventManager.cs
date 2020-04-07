using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RandomEventManager : MonoBehaviour
{
    public GameObject randomEventPanel;
    public GameObject resultPanel;
    public TextMeshProUGUI titleDisplay;
    public TextMeshProUGUI descriptionDisplay;
    public List<Bar> bars;
    private void Awake()
    {
        Round.RoundOver += Play;
    }

    private void OnDestroy()
    {
        Round.RoundOver -= Play;

    }
    private void Play()
    {
        if (RoundManager.instance.CurrentRoundStats.TargetMoney <= Restaurant.instance.RoundMoney)
            if (UnityEngine.Random.value < 0.3f)
            {
                var rndEvent = bars[Random.Range(0, bars.Count - 1)].RandomEvent();
                if (rndEvent == null) return;
                randomEventPanel.SetActive(true);
                resultPanel.SetActive(false);
                titleDisplay.text = rndEvent.title;
                descriptionDisplay.text = rndEvent.content;
            }
    }


}