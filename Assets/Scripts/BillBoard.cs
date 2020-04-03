using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera cam;
    private void Start() {
        cam = Camera.main;
    }
    private void Update() 
    {
        transform.LookAt(cam.transform , Vector3.up);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,90,0);
    }
}

