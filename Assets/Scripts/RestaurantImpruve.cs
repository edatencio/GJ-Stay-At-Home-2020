using UnityEngine;
using System.Collections.Generic;
public class RestaurantImpruve : MonoBehaviour
{
    public List<GameObject> kitchens;
    public List<GameObject> tables;

    private void Awake()
    {
        Bar.AddedPoint += ChangeRestaurant;
    }
    private void OnDestroy()
    {
        Bar.AddedPoint -= ChangeRestaurant;
    }
    private void Start()
    {
        ChangeRestaurant();
    }
    public void ChangeRestaurant()
    {
        foreach (var kitchen in kitchens)
        {
            kitchen.SetActive(false);
        }
        foreach (var table in tables)
        {
            table.SetActive(false);
        }
        switch (AdministrationSystem.instance.restaurantBar.Count)
        {
            case 0:
                kitchens[0].SetActive(true);
                tables[0].SetActive(true);
                Restaurant.instance.MaxCapacity = 4;
                break;
            case 1:
                kitchens[0].SetActive(true);
                tables[0].SetActive(true);
                Restaurant.instance.MaxCapacity = 5;
                break;
            case 2:
                kitchens[1].SetActive(true);
                tables[1].SetActive(true);
                Restaurant.instance.MaxCapacity = 5;
                break;
            case 3:
                kitchens[2].SetActive(true);
                tables[2].SetActive(true);
                Restaurant.instance.MaxCapacity = 6;
                break;

        }
    }
}
