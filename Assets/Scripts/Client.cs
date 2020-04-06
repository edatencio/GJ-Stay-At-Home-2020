using UnityEngine;
using static ClientGroup;

public class Client : MonoBehaviour
{
    [SerializeField] private ClientGroup myClientGroup;
    public Animator animator;

    private void Update()
    {
        switch (myClientGroup.State)
        {
            case ClientGroupState.Waiting:
                {
                    animator.SetTrigger("Idle");
                }
                break;

            case ClientGroupState.WaitingMenu:
                {

                }
                break;

            case ClientGroupState.Order:
                {
                    animator.SetTrigger("Finish");
                }
                break;

            case ClientGroupState.WaitingOrder:
                {

                }
                break;

            case ClientGroupState.Eating:
                animator.SetTrigger("Eat");

                break;

            case ClientGroupState.Finish:
                animator.SetTrigger("Finish");

                break;

        }
    }
}

