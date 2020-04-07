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
            case ClientGroupState.Order:
                {
                    //animator.SetTrigger("Order");
                }
                break;

            case ClientGroupState.Eating:
                animator.SetBool("Eat", true);
                break;

            case ClientGroupState.Finish:
                animator.SetBool("Eat", false);

                //animator.SetTrigger("Finish");

                break;
        }
    }
}

