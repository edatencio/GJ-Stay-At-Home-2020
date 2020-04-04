using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ClientGroup : MonoBehaviour
{
    public bool IsSitting { get; set; }
    public float contactRadius = 3f;
    [SerializeField] private Order order;
    public Order Order => order;
    public List<Costumer> costumers;
    public State State { get; set; }
    public float patienceTime = 60f;
    public float timeToOrder = 3f;
    public float eatingTime = 10f;
    [Range(0, 1)] public float SatisfactionAmount;
    private Vector3 orignalPos;
    private Dragger dragger;
    public bool isDragging { get; private set; }
    private bool eating;
    private bool orded;
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
            case State.WAITING:
                SatisfactionAmount -= Time.deltaTime / patienceTime;

                if (IsSitting)
                    ChangeState(State.SIT);
                break;
            case State.SIT:
                SatisfactionAmount -= Time.deltaTime / patienceTime;
                if (orded)
                    StartCoroutine(Ording());
                break;
            case State.WAITING_ORDER:
                SatisfactionAmount -= Time.deltaTime / patienceTime;
                //TODO añadir uan forma que el juegador interactue para pasar;
                if (Order.IsReady)
                    ChangeState(State.EATING);
                break;
            case State.EATING:
                if (!eating)
                    StartCoroutine(Eat());
                break;
            case State.FINISH:
                Restaurant.instance.LeaveRestaurant(this);
                break;
        }
    }
    private IEnumerator Ording()
    {
        orded = true;
        yield return new WaitForSeconds(timeToOrder);
        ChangeState(State.WAITING_ORDER);
    }
    private IEnumerator Eat()
    {
        eating = true;
        yield return new WaitForSeconds(eatingTime);
        ChangeState(State.FINISH);
    }
    private void ChangeState(State nextState)
    {
        SatisfactionAmount = Mathf.Clamp01(SatisfactionAmount * 1.2f);
        State = nextState;
    }
    #region Drag&Drop
    private void OnMouseDown()
    {
        if (!IsSitting)
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                dragger.CanDrag = true;

                transform.position += Vector3.up * 0.5f;
            }
    }
    private void OnMouseUp()
    {
        if (IsSitting) return;

        if (!Input.GetMouseButtonDown(0))
        {
            isDragging = false;
            dragger.CanDrag = false;

            var colliders = Physics.OverlapSphere(transform.position, contactRadius);
            Table table = null;

            foreach (var collider in colliders)
            {
                table = collider.GetComponent<Table>();

                if (table != null && !table.IsTaken && table.Seats.Count >= costumers.Count)
                {
                    IsSitting = true;

                    table.SetCostumer(this);
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
    #endregion
}
