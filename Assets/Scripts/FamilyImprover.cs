using UnityEngine;

public class FamilyImprover : MonoBehaviour
{
    public static FamilyState familyState { get; private set; }

    [SerializeField] private GameObject zorayaSitModel;
    [SerializeField] private Transform zorayaSitPos;
    [SerializeField] private PlayerController player;
    private void Awake()
    {
        Bar.AddedPoint += ImproveFamily;
    }
    private void OnDestroy()
    {
        Bar.AddedPoint -= ImproveFamily;
    }

    private void ImproveFamily()
    {
        zorayaSitModel.SetActive(false);

        switch (AdministrationSystem.instance.familyBar.Count)
        {
            case 0:
                familyState = FamilyState.None;
                break;
            case 1:
                familyState = FamilyState.OneKitchen;
                break;
            case 2:
                familyState = FamilyState.AllKitchen;
                break;
            case 3:
                zorayaSitModel.SetActive(true);
                zorayaSitModel.transform.position = zorayaSitPos.position;
                zorayaSitModel.transform.rotation = zorayaSitPos.rotation;
                player.SetMeshJOJO();
                break;
        }
    }

    private void Start()
    {
        ImproveFamily();
    }
}
