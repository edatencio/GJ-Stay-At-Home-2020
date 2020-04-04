using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField, ReorderableList] private Transform[] itemsPosition = new Transform[2];

    [SerializeField, BoxGroup("Debug")] private GameObject menuPrefab;

    private IInteractableItem[] items = new IInteractableItem[2];
    private Interactable destination;
    private bool interacted;

    private void Start()
    {
        items[0] = Instantiate(menuPrefab).GetComponent<IInteractableItem>();
        items[0].Model.transform.position = itemsPosition[0].position;
        items[0].Model.transform.SetParent(itemsPosition[0]);
    }

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

                    switch (destination.State)
                    {
                        case Interactable.InteractableState.Emit:
                            GetItem();
                            break;

                        case Interactable.InteractableState.Receive:
                            SetItem();
                            break;
                    }
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
                Interactable interactable = hit.transform.GetComponent<Interactable>();

                if (interactable != null)
                {
                    destination = interactable;
                    navMesh.SetDestination(interactable.NavMeshTarget.position);
                    interacted = false;
                }
            }
        }
    }

    private void GetItem()
    {
        bool spaceAvailable = false;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                spaceAvailable = true;
        }

        if (!spaceAvailable)
            return;

        if (!destination.TryGetItem<IInteractableItem>(out IInteractableItem item))
            return;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                items[i].Model.transform.position = itemsPosition[i].position;
                items[i].Model.transform.SetParent(itemsPosition[i]);
                return;
            }
        }
    }

    private void SetItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && destination.SetItem(items[i]))
            {
                items[i].Model.transform.SetParent(null);
                items[i] = null;
            }
        }
    }
}

