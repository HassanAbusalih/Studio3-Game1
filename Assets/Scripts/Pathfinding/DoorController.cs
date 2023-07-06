using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoorController : MonoBehaviour
{
    public float openAngle = 90.0f; 
    public float smoothSpeed = 2.0f; 
    private Quaternion openRotation; 
    private Quaternion closedRotation; 
    private bool isOpen = false; 

    private void Start()
    {
      
        openRotation = Quaternion.Euler(0.0f, openAngle, 0.0f) * transform.rotation;
        closedRotation = transform.rotation;
    }

    private void Update()
    {
    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isOpen = !isOpen; 
        }

        
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
