using UnityEngine;

public class Order : MonoBehaviour
{
    public bool IsReady { get; private set; }
    public int id_mesa;
    [SerializeField] private Arepa arepa;
    public Arepa Arepa => arepa;
    private void Start()
    {
        arepa = ArepaManager.instance?.GetRandomArepa();
    }
}
