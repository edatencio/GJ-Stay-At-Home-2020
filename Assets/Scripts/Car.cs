using UnityEngine;

public class Car : MonoBehaviour
{
    public float Speed { get; set; }

    private void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
}
