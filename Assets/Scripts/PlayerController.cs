using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private NavMeshAgent navMesh;

    private IInteractable destination;
    private bool interacted;

    private void Update()
    {
        if (destination != null)
        {
            Vector3 targetPosition = destination.NavMeshTarget.position;
            targetPosition.y = transform.position.y;

            if (Vector3.Distance(targetPosition, transform.position) < 0.2f)
            {
                if (!interacted)
                {
                    interacted = true;
                    destination.Interact();
                }

                Vector3 direction = (destination.transform.position.With(y: transform.position.y) - transform.position);

                Quaternion newRotation = direction == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(direction, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, navMesh.angularSpeed * Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, camera.farClipPlane))
            {
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    destination = interactable;
                    navMesh.SetDestination(interactable.NavMeshTarget.position);
                    interacted = false;
                }
            }
        }
    }
}
