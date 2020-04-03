using UnityEngine;
public class Dragger : MonoBehaviour
{
    private Vector3 offSet;
    private float mZCoord;
    public bool canDrag = true;

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        offSet = gameObject.transform.position - GetMouseWorldPos();
    }
    private Vector3 GetMouseWorldPos()
    {
        var mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    private void OnMouseDrag()
    {
        if (!canDrag) return;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out var hit))
        {
            var targetPos =  GetMouseWorldPos() + offSet;
            transform.position = new Vector3(targetPos.x,transform.position.y,targetPos.z);
        }
    }
}
