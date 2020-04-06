using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 repeatRate;
    [SerializeField] private Transform boundary;
    [SerializeField] private float carSpeed = 5f;
    [SerializeField, ReorderableList] private GameObject[] carPrefabs;

    private List<GameObject> cars = new List<GameObject>();

    private Timer timer = new Timer();
    private float repeatRandom;

    private void Start()
    {
    }

    private void Update()
    {
        if (!timer.isRunning)
        {
            timer.Start();
            repeatRandom = Random.Range(repeatRate.x, repeatRate.y);
        }
        else if (timer.ElapsedSeconds >= repeatRandom)
        {
            timer.Stop();

            GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)]);
            car.transform.SetParent(transform);
            car.transform.position = transform.position;
            car.transform.rotation = transform.rotation;
            car.AddComponent<Car>().Speed = carSpeed;

            cars.Add(car);
        }

        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].transform.localPosition.z >= boundary.localPosition.z)
            {
                Destroy(cars[i]);
                cars.RemoveAt(i);
            }
        }
    }
}
