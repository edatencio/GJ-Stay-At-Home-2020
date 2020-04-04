using System.Collections.Generic;
using UnityEngine;

public class ArepaManager : MonoBehaviour
{
    public static ArepaManager instance;

    [SerializeField] private List<Arepa> arepas;

    public Arepa GetRandomArepa()
    {
        return arepas[Random.Range(0, arepas.Count)];
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}