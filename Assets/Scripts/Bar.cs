using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action AddedPoint;
    public static event Action<string, string> OnMouseEnter;
    public static event Action OnMouseExit;
    [SerializeField] protected Image bar;
    [SerializeField, Range(0, 3)] protected int count = 0;
    public int Count => count;
    public Button plusButton;
    public BarDescription descriptions;


    private void Start()
    {
        bar.fillAmount = count / 3f;
    }
    private void OnEnable()
    {
        if (count > 3)
        {
            plusButton.interactable = false;
        }
    }
    public void Add()
    {
        if (count < 3)
        {
            count++;

            bar.fillAmount = count / 3f;
            Debug.Log(bar.fillAmount);
            AddedPoint?.Invoke();
        }
        else
            plusButton.interactable = false;
    }
    public void Reduce()
    {
        if (count > 0)
            bar.fillAmount -= count / 3;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (count < 3)
            OnMouseEnter?.Invoke(descriptions.info[count].title, descriptions.info[count].content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExit?.Invoke();
    }
}

