using UnityEngine;

public class Order : MonoBehaviour, IInteractableItem
{
    [SerializeField] private GameObject orderModel;
    [SerializeField] private GameObject foodModel;

    [HideInInspector] public Table table;
    private bool _isCooked;
    private GameObject _model;

    public int Count { get; private set; }

    public GameObject Model
    {
        get
        {
            if (_model == null)
            {
                _model = new GameObject();

                orderModel.transform.SetParent(_model.transform);
                orderModel.transform.localPosition = Vector3.zero;

                foodModel.transform.SetParent(_model.transform);
                foodModel.transform.localPosition = Vector3.zero;

                SetModels();
            }

            return _model;
        }
    }

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
            foodModel.SetActive(true);
            orderModel.SetActive(false);
        }
        else
        {
            foodModel.SetActive(false);
            orderModel.SetActive(true);
        }
    }
}
