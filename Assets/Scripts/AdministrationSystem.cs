using UnityEngine;
using TMPro;
using System;

public class AdministrationSystem : MonoBehaviour
{
    public static AdministrationSystem instance;
    public static event Action OnClose;
    public GameObject resultPanel;
    public GameObject administrationPanel;
    public TextMeshProUGUI titleDisplay;
    public TextMeshProUGUI descriptionDisplay;
    public TextMeshProUGUI walletDisplay;
    public Bar familyBar;
    public Bar impuestoBar;
    public Bar gotaGotaBar;
    public Bar restaurantBar;

    private bool once = false;
    private void OpenResultPanel()
    {
        if (RoundManager.instance.CurrentRoundStats.TargetMoney < Restaurant.instance.Wallet)
        {
            resultPanel.gameObject.SetActive(true);
            walletDisplay.text = Restaurant.instance.Wallet.ToString("F0");
        }
    }
    public void OpenAdminPanel()
    {
        resultPanel.SetActive(false);
        administrationPanel.SetActive(true);
        once = false;
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

        administrationPanel.SetActive(false);
        resultPanel.SetActive(false);
    }
    private void Start()
    {

        Bar.AddedPoint += PointAdded;
        Bar.OnMouseEnter += ShowDescription;
        Bar.OnMouseExit += HideDescription;
        Round.RoundOver += OpenResultPanel;
    }

    private void HideDescription()
    {
        titleDisplay.text = "Toma una decision";
        descriptionDisplay.text = "";
        walletDisplay.text = Restaurant.instance.Wallet.ToString("F0") + "$";

    }

    private void ShowDescription(string title, string description)
    {
        titleDisplay.text = title;
        descriptionDisplay.text = description;
        if (once) return;
        walletDisplay.text = 0 + "$";

    }

    private void OnDestroy()
    {
        Bar.AddedPoint -= PointAdded;
        Bar.OnMouseEnter -= ShowDescription;
        Bar.OnMouseExit -= HideDescription;
    }

    private void PointAdded()
    {
        familyBar.plusButton.interactable = false;
        impuestoBar.plusButton.interactable = false;
        gotaGotaBar.plusButton.interactable = false;
        restaurantBar.plusButton.interactable = false;
        once = true;
        Restaurant.instance.Wallet = 0;
        walletDisplay.text = "0$";
    }
    public void ClosePanel()
    {
        familyBar.plusButton.interactable = true;
        impuestoBar.plusButton.interactable = true;
        gotaGotaBar.plusButton.interactable = true;
        restaurantBar.plusButton.interactable = true;
        OnClose?.Invoke();
        resultPanel.gameObject.SetActive(false);
        administrationPanel.gameObject.SetActive(false);
    }
}

