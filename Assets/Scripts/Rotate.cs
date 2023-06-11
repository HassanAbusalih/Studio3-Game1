using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float targetRotation = 90;
    bool rotateClockwise;
    [SerializeField] float rotationStep;
    [SerializeField] float number;

    void Update()
    {
        RotateObject();
    }

    private void RotateObject()
    {
        number = Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation);
        if (transform.rotation.eulerAngles.z <= targetRotation)
        {
            rotateClockwise = !rotateClockwise;
            if (rotateClockwise)
            {
                targetRotation = -90f;
            }
            else
            {
                targetRotation = 90f;
            }
        }
        if (rotateClockwise)
        {
            transform.Rotate(Vector3.forward, rotationStep * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.forward, -rotationStep * Time.deltaTime);
        }
    }
}
