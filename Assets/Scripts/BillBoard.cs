using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera cam;
    float orignalRotY;
    private void Start()
    {
        orignalRotY = transform.rotation.eulerAngles.y;
        cam = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(cam.transform);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,orignalRotY,transform.rotation.eulerAngles.z);
    }
}

