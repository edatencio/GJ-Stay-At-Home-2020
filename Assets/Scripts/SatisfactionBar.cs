using UnityEngine;

using UnityEngine.UI;
[RequireComponent(typeof(ClientGroup))]
public class SatisfactionBar : MonoBehaviour
{
    [SerializeField]private Gradient colorGradient;
    [SerializeField] private ClientGroup clientGroup;
    public Image imageToFill;
    private void LateUpdate()
    {
        imageToFill.fillAmount = clientGroup.SatisfactionAmount;
        imageToFill.color = colorGradient.Evaluate(imageToFill.fillAmount);
    }
}

