using UnityEngine;
using System.Collections;

public class Kitchen : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform navMeshTarget;
    [SerializeField] private float cookTime;
    [SerializeField] private GameObject clock;

    private void Start()
    {
        clock.SetActive(false);
    }

    public Transform NavMeshTarget => navMeshTarget;

    public void Interact()
    {
        StartCoroutine(Cook());
    }

    private IEnumerator Cook()
    {
        clock.SetActive(true);
        yield return new WaitForSeconds(cookTime);
        clock.SetActive(false);
    }
}
