using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class FillColorChange : MonoBehaviour
{
    public Gradient colorGradient;

    private Image image;
    private float originalValue;
    public bool debugMode;
    private void Start()
    {
        image = GetComponent<Image>();

    }

    private void Update()
    {

        if (image.fillAmount != originalValue)
        {
            UpdateGradient();
            originalValue = image.fillAmount;
        }

        if (debugMode)
            image.color = colorGradient.Evaluate(image.fillAmount);

    }

    private void UpdateGradient()
    {
        image.color = colorGradient.Evaluate(image.fillAmount);
    }
}
