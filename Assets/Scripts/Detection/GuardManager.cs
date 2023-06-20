using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    enum GuardState { Patrol, Search, Chase }
    GuardState state = GuardState.Patrol;
    Coroutine currentAction;
    [SerializeField] Transform[] path;
    int currentPos;
    [SerializeField] float moveSpeed;

    void Start()
    {
        FindNearestPoint();
        currentAction = StartCoroutine(PatrolPath());
    }

    void Update()
    {
        switch (state)
        {
            case GuardState.Patrol:
                if (currentAction != null)
                {
                    break;
                }
                currentAction = StartCoroutine(PatrolPath());
                break;
            case GuardState.Search:
                //Select random points around search location (either the player's last seen position, or sound)
                break;
            case GuardState.Chase:
                //Move towards the player
                break;
        }
    }

    //Needs changing later after A* is implemented.
    void FindNearestPoint()
    {
        float distance = float.MaxValue;
        for (int i = 0; i < path.Length; i++)
        {
            float newDistance = (path[i].position - transform.position).magnitude;
            if (newDistance < distance)
            {
                distance = newDistance;
                currentPos = i;
            }
        }
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, path[currentPos].transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360);
    }

    IEnumerator PatrolPath()
    {
        while (true)
        {
            float distance = (path[currentPos].transform.position - transform.position).magnitude;
            if (distance > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, path[currentPos].transform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                currentPos++;
                currentPos %= path.Length;
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, path[currentPos].transform.position - transform.position);
                float angle = Vector2.SignedAngle(transform.up, (path[currentPos].transform.position - transform.position).normalized);
                bool rotatedLeft = angle > 0;
                while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 40 * Time.deltaTime);
                    yield return null;
                }
                yield return Scan(gameObject, 90, 40, rotatedLeft);
            }
            yield return null;
        }
    }

    IEnumerator Scan(GameObject gameObject, float fov, float rotationSpeed, bool rotation)
    {
        Quaternion startRotation = gameObject.transform.rotation;
        Quaternion left = Quaternion.Euler(gameObject.transform.eulerAngles + new Vector3(0, 0, fov / 2));
        Quaternion right = Quaternion.Euler(gameObject.transform.eulerAngles - new Vector3(0, 0, fov / 2));
        if (rotation)
        {
            yield return RotationOrder(gameObject, rotationSpeed, left, right);
        }
        else
        {
            yield return RotationOrder(gameObject, rotationSpeed, right, left);
        }
        while (Quaternion.Angle(gameObject.transform.rotation, startRotation) > 0.1f)
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, startRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RotationOrder(GameObject gameObject, float rotationSpeed, Quaternion rotation1, Quaternion rotation2)
    {
        while (Quaternion.Angle(gameObject.transform.rotation, rotation1) > 0.1f)
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, rotation1, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        while (Quaternion.Angle(gameObject.transform.rotation, rotation2) > 0.1f)
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, rotation2, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
