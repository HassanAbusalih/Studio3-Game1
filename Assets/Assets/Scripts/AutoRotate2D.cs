using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate2D : MonoBehaviour
{
    public enum RotationAxis { XAxis, YAxis }

    public RotationAxis rotationAxis = RotationAxis.XAxis;
    public float rotationSpeed = 60f; // Set the rotation speed in degrees per second

    private void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        // Calculate the rotation amount for the current frame
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Rotate the object around the chosen axis
        switch (rotationAxis)
        {
            case RotationAxis.XAxis:
                transform.Rotate(Vector3.right * rotationAmount);
                break;
            case RotationAxis.YAxis:
                transform.Rotate(Vector3.up * rotationAmount);
                break;
            default:
                Debug.LogWarning("Invalid Rotation Axis specified.");
                break;
        }
    }
}
