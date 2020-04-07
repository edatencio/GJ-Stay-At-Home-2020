using UnityEngine;

public class GotaAGotaImprover : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private void Awake()
    {
        Bar.AddedPoint += ImproveGotaGota;
    }

    private void OnDestroy()
    {
        Bar.AddedPoint -= ImproveGotaGota;
    }

    private void ImproveGotaGota()
    {

        switch (AdministrationSystem.instance.gotaGotaBar.Count)
        {
            case 0:
                player.ChangeSpeed(0);
                break;
            case 1:
                player.ChangeSpeed(1);
                break;
            case 2:
                RoundManager.instance.CurrentRoundStats.TargetMoney /= 1.2f;
                break;
            case 3:

                RoundManager.instance.CurrentRoundStats.TargetMoney /= 2;
                break;
        }
    }

    private void Start()
    {
        ImproveGotaGota();
    }
}
