using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardManager : MonoBehaviour
{
    enum GuardState { Patrol, Search, Chase }
    GuardState state = GuardState.Patrol;
    Coroutine currentAction;
    [SerializeField] Transform[] patrolPath;
    List<Vector3> aStarPath = new();
    AStarGrid grid;
    AStar aStar;
    int currentPos;
    Vector3 targetPos;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float hearingDistance;

    private void OnDrawGizmos()
    {
        if (aStarPath != null) 
        {
            foreach (var path in aStarPath)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path, Vector3.one);
            }
        }
    }

    void Start()
    {
        grid = FindObjectOfType<AStarGrid>();
        aStar = new(grid);
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
                if (currentAction != null)
                {
                    break;
                }
                currentAction = StartCoroutine(PatrolPath());
                break;
            case GuardState.Chase:
                //Move towards the player
                break;
        }
    }

    void SoundHeard(Vector2 position)
    {
        if (Vector2.Distance(position, transform.position) < hearingDistance) 
        {
            state = GuardState.Search;
            StopCoroutine(currentAction);
            targetPos = position;
            aStarPath = aStar.GetPath(transform.position, targetPos);
            currentAction = StartCoroutine(SearchArea());
        }
    }

    IEnumerator SearchArea()
    {
        yield return Navigate();
        yield return LookAround();
        state = GuardState.Patrol;
        currentAction = StartCoroutine(PatrolPath());
    }

    IEnumerator PatrolPath()
    {
        FindNearestPoint();
        while (true)
        {
            yield return Navigate();
            if (aStarPath != null)
            { 
                currentPos++;
                currentPos %= patrolPath.Length;
                targetPos = patrolPath[currentPos].position;
                aStarPath = aStar.GetPath(transform.position, targetPos);
                yield return LookAround();
            }
            yield return null;
        }
    }

    void FindNearestPoint()
    {
        float distance = float.MaxValue;
        for (int i = 0; i < patrolPath.Length; i++)
        {
            float newDistance = (patrolPath[i].position - transform.position).magnitude;
            if (newDistance < distance)
            {
                distance = newDistance;
                currentPos = i;
            }
        }
        targetPos = patrolPath[currentPos].position;
        aStarPath = aStar.GetPath(transform.position, targetPos);
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, patrolPath[currentPos].transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360);
    }

    IEnumerator Navigate()
    {
        if (aStarPath == null)
        {
            aStarPath = aStar.GetPath(transform.position, targetPos);
            yield return null;
        }
        while (aStarPath != null && aStarPath.Count > 0)
        {
            Vector3 targetPoint = aStarPath[0];
            if ((targetPoint - transform.position).magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(transform.position - targetPoint, Vector3.forward);
                targetRotation = new Quaternion(0, 0, targetRotation.z, targetRotation.w);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10 * rotationSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
            }
            else
            {
                aStarPath.RemoveAt(0);
            }
            yield return null;
        }
    }

    IEnumerator LookAround()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetPos - transform.position);
        float angle = Vector2.SignedAngle(transform.up, (targetPos - transform.position).normalized);
        bool rotatedLeft = angle > 0;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return Scan(gameObject, 90, rotationSpeed, rotatedLeft);
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

    private void OnEnable()
    {
        Yeet.SoundGenerated += SoundHeard;
    }

    private void OnDisable()
    {
        Yeet.SoundGenerated -= SoundHeard;
    }
}