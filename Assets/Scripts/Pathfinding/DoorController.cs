using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject door;
    public bool doorOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (doorOpen)
                CloseDoor();
            else
                OpenDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorOpen = false;
        }
    }

    private void OpenDoor()
    {
        door.SetActive(false);
        doorOpen = true;
    }

    private void CloseDoor()
    {
        door.SetActive(true);
        doorOpen = false;
    }
}
