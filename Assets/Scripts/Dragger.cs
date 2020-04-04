using UnityEngine;

public class Dragger : MonoBehaviour
{
    private new Camera camera;

    public bool CanDrag { get; set; }

    private void Start()
    {
        camera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (!CanDrag)
            return;

        if (Input.GetButton("Fire1"))
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, camera.farClipPlane))
                transform.position = hit.point.With(y: transform.position.y);
        }
    }
}
