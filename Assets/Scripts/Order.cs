using UnityEngine;

public class Order : MonoBehaviour, IInteractableItem
{
    [SerializeField] private GameObject orderModel;
    [SerializeField] private GameObject foodModel;

    [HideInInspector] public Table table;
    private bool _isCooked;
    private GameObject _mesh;

    private static GameObject parent;
    private static int ordersCount;

    public int Count { get; set; }

    public bool IsCooked
    {
        get => _isCooked;

        set
        {
            _isCooked = value;
            SetModels();
        }
    }

    private void SetModels()
    {
        if (IsCooked)
        {
            foodModel?.SetActive(true);
            orderModel?.SetActive(false);
        }
        else
        {
            foodModel?.SetActive(false);
            orderModel?.SetActive(true);
        }
    }

    public void Setup(int count)
    {
        Count = count;
        IsCooked = false;
    }
}
