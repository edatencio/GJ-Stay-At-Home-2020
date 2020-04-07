using UnityEngine;
using NaughtyAttributes;
using TMPro;
using System.Collections.Generic;
using System;
using System.Collections;

public class ClientGroup : MonoBehaviour
{
    [SerializeField, BoxGroup("Settings")] private float contactRadius = 3f;
    [SerializeField, BoxGroup("Settings")] private float patienceTime = 15f;
    [SerializeField, BoxGroup("Settings")] private float timeToOrder = 3f;
    [SerializeField, BoxGroup("Settings")] private float eatingTime = 10f;
    [SerializeField, BoxGroup("Settings")] private float emotionDisplayTime = 20f;
    [Range(0, 1), BoxGroup("Settings")] public float SatisfactionAmount;
    [SerializeField, BoxGroup("References")] private GameObject orderPrefab;
    [SerializeField, BoxGroup("References")] private GameObject dirtyPlatesPrefab;
    [SerializeField, BoxGroup("References")] private Dragger dragger;
    [SerializeField, BoxGroup("References")] private SpriteRenderer emotionSprite;
    [SerializeField, BoxGroup("References")] private GameObject satisfactionSlider;
    [ReorderableList, BoxGroup("References")] public List<Client> clients;

    private Table table;
    private Vector3 orignalPos;
    public TMPro.TextMeshProUGUI comisionDisplay;
    private bool eating;
    private bool ordered;
    private Timer timer = new Timer();
    private Timer emotionTimer = new Timer();

    public enum ClientGroupState { Waiting, WaitingMenu, Menu, Order, WaitingOrder, Eating, Finish }

    [ShowNativeProperty]
    public ClientGroupState State { get; set; }

    public bool IsSitting { get; set; }

    public bool isDragging { get; private set; }

    public Order Order { get; private set; }

    public GameObject SatisfactionSlider => satisfactionSlider;

    private void Start()
    {
        orignalPos = transform.position;
        SatisfactionAmount = 1;
        patienceTime *= clients.Count;
        eatingTime += (eatingTime * 0.1f) * clients.Count;
        comisionDisplay.gameObject.SetActive(false);
        emotionSprite.gameObject.SetActive(false);
        emotionTimer.Start();
        emotionDisplayTime = UnityEngine.Random.Range(emotionDisplayTime, emotionDisplayTime + 6f);
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
                            ChangeState(ClientGroupState.Menu);
                        }
                        else
                        {
                            table.SetItem(item);
                        }
                    }
                }
                break;

            case ClientGroupState.Menu:
                {
                    if (!timer.isRunning)
                        timer.Start();
                    else if (timer.ElapsedSeconds >= timeToOrder)
                        ChangeState(ClientGroupState.Order);
                }
                break;

            case ClientGroupState.Order:
                {
                    Order = Instantiate(orderPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<Order>();
                    Order.Setup(clients.Count);
                    Order.transform.rotation = Quaternion.identity;

                    table.SetItem(Order);
                    ChangeState(ClientGroupState.WaitingOrder);
                }
                break;

            case ClientGroupState.WaitingOrder:
                {
                    SatisfactionAmount -= Time.deltaTime / patienceTime;

                    if (table.TryGetItem<Order>(out IInteractableItem item))
                    {
                        if ((item as Order) == Order && (item as Order).IsCooked)
                            ChangeState(ClientGroupState.Eating);
                        else
                            table.SetItem(item);
                    }
                }
                break;

            case ClientGroupState.Eating:
                {
                    if (!timer.isRunning)
                    {
                        timer.Start();
                        table.State = Interactable.InteractableState.Idle;
                    }
                    else if (timer.ElapsedSeconds >= eatingTime)
                    {
                        Destroy(Order.gameObject);
                        table.State = Interactable.InteractableState.Receive;
                        table.SetItem(Instantiate(dirtyPlatesPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero)).GetComponent<PlatesDirty>());
                        ChangeState(ClientGroupState.Finish);
                    }
                }
                break;

            case ClientGroupState.Finish:
                {
                    LeaveRestaurant();
                }
                break;
        }

        if (emotionTimer.ElapsedSeconds >= emotionDisplayTime)
        {
            StartCoroutine(DisplayEmote());
            if (eating)
            {
                emotionTimer.Stop();
                return;
            }

            emotionTimer.Start();
        }
        if (SatisfactionAmount <= 0)
            LeaveRestaurant();

        //textMesh.text = State.ToString();
    }

    private IEnumerator DisplayEmote()
    {
        EmotionType type = EmotionType.Happy;
        emotionSprite.gameObject.SetActive(true);
        if (SatisfactionAmount >= 1f && SatisfactionAmount >= 0.5f)
            type = EmotionType.Happy;
        if (SatisfactionAmount <= 0.5f && SatisfactionAmount >= 0.3f)
            type = EmotionType.Angry;
        if (SatisfactionAmount <= 0.3f && SatisfactionAmount >= 0f)
            type = EmotionType.Worried;

        emotionSprite.sprite = EmotionManager.instance.GetEmotion(type).sprite;
        yield return new WaitForSeconds(2f);
        emotionSprite.gameObject.SetActive(false);
    }

    private IEnumerator DisplayEmote(EmotionType type)
    {
        emotionSprite.gameObject.SetActive(true);
        var emotion = EmotionManager.instance.GetEmotion(type);
        emotionSprite.sprite = emotion.sprite;
        yield return new WaitForSeconds(2f);
        emotionSprite.gameObject.SetActive(false);
    }

    private IEnumerator DisplayEmote(ClientGroupState state)
    {
        var type = EmotionType.Talking;
        switch (state)
        {
            case ClientGroupState.WaitingMenu:
                type = EmotionType.Waiting;
                break;

            case ClientGroupState.Eating:
                type = EmotionType.Happy;
                break;

            case ClientGroupState.Finish:
                type = EmotionType.Money;
                break;
        }
        var emotion = EmotionManager.instance.GetEmotion(type);

        emotionSprite.gameObject.SetActive(true);
        emotionSprite.sprite = emotion.sprite;
        yield return new WaitForSeconds(2f);
        emotionSprite.gameObject.SetActive(false);
    }

    private void ChangeState(ClientGroupState nextState)
    {
        SatisfactionAmount = Mathf.Clamp01(SatisfactionAmount * 1.2f);
        State = nextState;
        StartCoroutine(DisplayEmote(nextState));
        timer.Stop();
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

    private void LeaveRestaurant()
    {
        if (State == ClientGroupState.WaitingOrder)
            Destroy(Order.gameObject);

        var tip = (clients.Count * UnityEngine.Random.Range(0.5f, 1f)) / SatisfactionAmount;
        if (SatisfactionAmount <= 0) tip = 0;

        Restaurant.instance.LeaveRestaurant(this, tip);
    }
}
