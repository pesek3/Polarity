using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;

    private void Start()
    {
        if (string.IsNullOrEmpty(promptMessage))
        {
            promptMessage = "Press E to use the button";
        }

        if (door == null)
        {
            Debug.LogWarning("Door is not assigned to the button.", this);
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse is over the button.");
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse left the button.");
    }

    private void OnMouseDown()
    {
        Debug.Log("Button clicked.");
        BaseInteract();
    }

    protected override void Interact()
    {
        doorOpen = !doorOpen;
        Debug.Log("Button triggered. Door open: " + doorOpen);

        if (door != null)
        {
            door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
        }
    }
}
