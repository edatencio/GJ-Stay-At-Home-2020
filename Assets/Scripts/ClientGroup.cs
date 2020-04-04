using UnityEngine;
using System.Collections.Generic;

public class ClientGroup : MonoBehaviour
{
    public bool IsSitting { get; set; }
    public float contactRadius = 3f;
    [SerializeField] private Order order;
    public Order Order => order;
    public List<Costumer> costumers;
    public State State { get; set; }
    public float patienceTime = 60f;
    [Range(0, 1)] public float SatisfactionAmount;
    private Vector3 orignalPos;
    private Dragger dragger;
    private bool draggin;
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
                ChangeState(State.SIT);
                break;
            case State.SIT:
                SatisfactionAmount -= Time.deltaTime / patienceTime;
                ChangeState(State.ORDING);
                break;
            case State.ORDING:
                SatisfactionAmount -= Time.deltaTime / patienceTime;
                ChangeState(State.WAITING_ORDER);
                break;
            case State.WAITING_ORDER:
                SatisfactionAmount -= Time.deltaTime / patienceTime;
                ChangeState(State.EATING);
                break;
            case State.EATING:
                ChangeState(State.FINISH);
                break;
            case State.FINISH:
                break;
        }
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
                draggin = true;
                dragger.canDrag = true;

                transform.position += Vector3.up * 0.5f;
            }
    }

    private void OnMouseUp()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            draggin = false;
            dragger.canDrag = false;

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
            }
            transform.position = orignalPos;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, contactRadius);
    }
    #endregion
}
