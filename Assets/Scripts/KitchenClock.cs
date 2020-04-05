using UnityEngine;
using UnityEngine.UI;

public class KitchenClock : MonoBehaviour
{
    public Image watch;
    public float cookTime;

    private void OnEnable()
    {
        watch.fillAmount = 0;
    }
    private void Update()
    {
        watch.fillAmount += Time.deltaTime / cookTime;
    }

}


