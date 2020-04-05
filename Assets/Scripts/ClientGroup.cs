using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;

public class ClientGroup : MonoBehaviour
{
    [SerializeField, BoxGroup("Settings")] private float contactRadius = 3f;
    [SerializeField, BoxGroup("Settings")] private float patienceTime = 60f;
    [SerializeField, BoxGroup("Settings")] private float timeToOrder = 3f;
    [SerializeField, BoxGroup("Settings")] private float eatingTime = 10f;
    [Range(0, 1), BoxGroup("Settings")] public float SatisfactionAmount;
    [SerializeField, BoxGroup("References")] private GameObject orderPrefab;
    [SerializeField, BoxGroup("References")] private GameObject satisfactionSlider;
    [ReorderableList, BoxGroup("References")] public List<Client> clients;

    private Table table;
    private Vector3 orignalPos;
    private Dragger dragger;
    private bool eating;
    private bool orded;

    private Order order;

    public enum ClientGroupState { Waiting, WaitingMenu, Order, WaitingOrder, Eating, Finish }

    [ShowNativeProperty]
    public ClientGroupState State { get; set; }

    public bool IsSitting { get; set; }

    public bool isDragging { get; private set; }

    public GameObject SatisfactionSlider => satisfactionSlider;

    private void Start()
    {
        orignalPos = transform.position;
        dragger = gameObject.AddComponent<Dragger>();
        SatisfactionAmount = 1;
    }

    private void Update()
    {
        switch (State)
        {
            case ClientGroupState.Waiting:
                {
                    SatisfactionAmount -= Time.deltaTime / patienceTime;

                    if (IsSitting)
                        ChangeState(ClientGroupState.WaitingMenu);
                }
                break;

            case ClientGroupState.WaitingMenu:
                {
                    SatisfactionAmount -= Time.deltaTime / patienceTime;

                    if (table.TryGetItem<Menu>(out IInteractableItem item))
                    {
                        if (item is Menu)
                        {
                            table.ClearItem();
                            StartCoroutine(Ording());
                        }
                        else
                        {
                            table.SetItem(item);
                        }
                    }
                }
                break;

            case ClientGroupState.Order:
                {
                    order = Instantiate(orderPrefab).GetComponent<Order>();
                    order.Setup(clients.Count);

                    table.SetItem(order);
                    ChangeState(ClientGroupState.WaitingOrder);
                }
                break;

            case ClientGroupState.WaitingOrder:
                {
                    SatisfactionAmount -= Time.deltaTime / patienceTime;

                    if (table.TryGetItem<Order>(out IInteractableItem item))
                    {
                        if ((item as Order) == order && (item as Order).IsCooked)
                            ChangeState(ClientGroupState.Eating);
                        else
                            table.SetItem(item);
                    }
                }
                break;

            case ClientGroupState.Eating:
                if (!eating)
                    StartCoroutine(Eat());
                break;

            case ClientGroupState.Finish:

                // TODO if satisfaction is cero, leave restaurant
                Restaurant.instance.LeaveRestaurant(this);
                //Destroy(gameObject);
                break;
        }
        if (SatisfactionAmount <= 0)
            Restaurant.instance.LeaveRestaurant(this);

    }

    private IEnumerator Ording()
    {
        yield return new WaitForSeconds(timeToOrder);
        ChangeState(ClientGroupState.Order);
    }

    private IEnumerator Eat()
    {
        eating = true;
        yield return new WaitForSeconds(eatingTime);
        Destroy(order.gameObject);
        ChangeState(ClientGroupState.Finish);
    }

    private void ChangeState(ClientGroupState nextState)
    {
        SatisfactionAmount = Mathf.Clamp01(SatisfactionAmount * 1.2f);
        State = nextState;
    }

    #region Drag&Drop
    private void OnMouseDown()
    {
        if (!IsSitting && Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragger.CanDrag = true;
            transform.position += Vector3.up * 0.5f;
        }
    }

    private void OnMouseUp()
    {
        if (IsSitting)
            return;

        if (!Input.GetMouseButtonDown(0))
        {
            isDragging = false;
            dragger.CanDrag = false;

            var colliders = Physics.OverlapSphere(transform.position, contactRadius);
            Table table = null;

            foreach (var collider in colliders)
            {
                table = collider.GetComponent<Table>();

                if (table != null && !table.IsTaken && table.SeatCount >= clients.Count)
                {
                    IsSitting = true;
                    this.table = table;
                    table.SetClientGroup(this);
                    GetComponent<Collider>().enabled = false;

                    return;
                }
                else
                    transform.position = orignalPos;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, contactRadius);
    }

    #endregion Drag&Drop
}
