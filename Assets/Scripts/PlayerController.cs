using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class PlayerController : MonoBehaviour
{
    [SerializeField, BoxGroup("Mesh")] private Mesh zorayaMesh;
    [SerializeField, BoxGroup("Mesh")] private Material zorayaMaterial;
    [SerializeField, BoxGroup("Mesh")] private Mesh jojotoMesh;
    [SerializeField, BoxGroup("Mesh")] private Material jojotoMaterial;
    [SerializeField, BoxGroup("Mesh")] private new SkinnedMeshRenderer renderer;
    [SerializeField, BoxGroup("References")] private new Camera camera;
    [SerializeField, BoxGroup("References")] private NavMeshAgent navMesh;
    [SerializeField, BoxGroup("References")] private Animator animator;
    [SerializeField, BoxGroup("References")] private ParticleSystem runParticles;
    [SerializeField, BoxGroup("References"), ReorderableList] private Transform[] itemsPosition = new Transform[2];

    private IInteractableItem[] items = new IInteractableItem[2];
    private Interactable destination;
    private Menu menu;
    private bool interacted;
    private bool alternateHand;

    private void Start()
    {
        menu = new GameObject("Menu").AddComponent<Menu>();
    }

    private void Update()
    {
        if (destination != null)
        {
            Vector3 targetPosition = destination.NavMeshTarget.position;
            targetPosition.y = transform.position.y;

            if (Vector3.Distance(targetPosition, transform.position) < 0.2f)
            {
                Interact();

                Vector3 direction = (destination.transform.position.With(y: transform.position.y) - transform.position);

                Quaternion newRotation = direction == Vector3.zero ? Quaternion.identity : Quaternion.LookRotation(direction, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, navMesh.angularSpeed * Time.deltaTime);
            }
        }

        // Animations
        animator.SetFloat(Constants.Player.Animations.Velocity, navMesh.velocity.magnitude);
        animator.SetFloat(Constants.Player.Animations.RandomValue, Random.value);

        // Particle Systems
        if (navMesh.velocity.magnitude > 0.2f)
        {
            if (runParticles.isStopped)
                runParticles.Play();
        }
        else
        {
            if (runParticles.isPlaying)
                runParticles.Stop();
        }
    }

    private void Interact()
    {
        if (!interacted)
        {
            interacted = true;

            if (destination is Table)
            {
                switch (destination.State)
                {
                    case Interactable.InteractableState.Emit:
                        GetItem();
                        break;

                    case Interactable.InteractableState.Receive:
                        {
                            Table table = destination as Table;

                            // if client waiting for menu
                            if (table.clientGroup?.State == ClientGroup.ClientGroupState.WaitingMenu)
                            {
                                destination.SetItem(menu);
                                return;
                            }

                            // If client waiting for cooked order
                            for (int i = 0; i < items.Length; i++)
                            {
                                if (items[i] != null && (items[i] as Order).IsCooked && (items[i] as Order) == table.clientGroup?.Order)
                                {
                                    TrySetItem(i);
                                    return;
                                }
                            }

                            // TODO recoger el pago de los clientes
                        }
                        break;
                }
            }
            else if (destination is Kitchen)
            {
                switch (destination.State)
                {
                    // If kitchen is busy, do nothing
                    case Interactable.InteractableState.Idle:
                        return;

                    // if kitchen is receiving, give order
                    case Interactable.InteractableState.Receive:
                        {
                            for (int i = 0; i < items.Length; i++)
                            {
                                if (items[i] != null && !(items[i] as Order).IsCooked)
                                {
                                    TrySetItem(i);
                                    return;
                                }
                            }
                        }
                        break;

                    case Interactable.InteractableState.Emit:
                        {
                            // if kitchen is emitting and there is an empty hand, get order
                            for (int i = 0; i < items.Length; i++)
                            {
                                if (items[i] == null)
                                {
                                    GetItem();
                                    return;
                                }
                            }

                            // if kitchen is emitting, and both hands are full, but there is an
                            // uncooked order in one of them, swap orders
                            for (int i = 0; i < items.Length; i++)
                            {
                                if (items[i] != null && !(items[i] as Order).IsCooked)
                                {
                                    if (destination.TryGetItem<Order>(out IInteractableItem item))
                                    {
                                        TrySetItem(i);

                                        items[i] = item;
                                        items[i].transform.position = itemsPosition[i].position;
                                        items[i].transform.SetParent(itemsPosition[i]);
                                    }

                                    return;
                                }
                            }
                        }
                        break;
                }
            }
            else if (destination is PlatesWasher)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null && items[i] is PlatesDirty)
                    {
                        TrySetItem(i);
                        return;
                    }
                }
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
                    // TODO Queue destinations

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
            //TODO: sometimes get's null exception
            if (items[i] == null)
            {
                try
                {
                    items[i] = item;
                    items[i].transform.position = itemsPosition[i].position;
                    items[i].transform.rotation = itemsPosition[i].rotation;
                    items[i].transform.SetParent(itemsPosition[i]);
                }
                catch (System.Exception)
                {
                    return;
                }
                return;
            }
        }
    }

    private void TrySetItem(int index)
    {
        if (items[index] != null && destination.SetItem(items[index]))
        {
            items[index].transform.SetParent(null);
            items[index] = null;
        }
    }

    public void SetMeshJOJO()
    {
        renderer.sharedMesh = jojotoMesh;
        renderer.material = jojotoMaterial;
    }

    public void SetMeshZoraya()
    {
        renderer.sharedMesh = zorayaMesh;
        renderer.material = zorayaMaterial;
    }

    public void ChangeSpeed(int level)
    {
        switch (level)
        {
            case 0:
                navMesh.acceleration = 30;
                navMesh.speed = 5f;
                break;

            case 1:
                navMesh.acceleration = 50;
                navMesh.speed = 7f;
                break;

            default:
                navMesh.acceleration = 50;
                navMesh.speed = 7f;
                break;
        }
    }
    public void SetPositionInGame(Transform targetPos)
    {
        navMesh.SetDestination(targetPos.position);
        
    }
}

