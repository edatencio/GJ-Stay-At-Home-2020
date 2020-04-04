using UnityEngine;

public class Menu : MonoBehaviour, IInteractableItem
{
    [SerializeField] private GameObject model;
    public GameObject Model => model;
}

