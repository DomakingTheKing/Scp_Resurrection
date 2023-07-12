using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : Interactable
{
    [SerializeField] private GameObject door;
    private bool doorOpen;

    private Animator doorAnimator;
    [SerializeField] private float doorCloseDelay = 4f;
    private float doorCloseTimer;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = door.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doorOpen && doorCloseTimer <= 0f)
        {
            doorOpen = false;
            doorAnimator.SetBool("IsOpen", doorOpen);
        }
        else
        {
            doorCloseTimer -= Time.deltaTime;
        }
    }

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        doorAnimator.SetBool("IsOpen", doorOpen);
        doorCloseTimer = doorCloseDelay;
    }
}
