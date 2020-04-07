using UnityEngine;
using System;

public class PlatesWasher : Interactable
{
    [SerializeField] private AudioSource platesAudio;

    public override Type ItemType => typeof(PlatesDirty);

    private void Start()
    {
        State = InteractableState.Receive;
    }

    private void Update()
    {
    }

    protected override void OnItemGet()
    {
    }

    protected override void OnItemSet()
    {
        platesAudio.Play();
        Destroy(CurrentItem.gameObject);
    }
}
