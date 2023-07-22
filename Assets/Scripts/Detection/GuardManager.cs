using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AStar;

public class GuardManager : MonoBehaviour
{
    enum GuardState { Idle, Patrol, Search, Chase }
    GuardState state = GuardState.Patrol;
    Coroutine currentAction;
    Coroutine navigation;
    PlayerMovement player;
    List<Vector3> aStarPath = new();
    AStarGrid grid;
    AStar.AStar aStar;
    int currentPos;
    Vector3 targetPos;
    float timer = 0;
    [SerializeField] bool idle;
    bool recalculate;
    Vector3 startPos;
    Quaternion startRot;
    [SerializeField] Transform[] patrolPath;
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
        foreach (var point in patrolPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(point.position, 0.25f);
            if (grid != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(grid.GetNearestWalkable(grid.WorldToGrid(point.position), point.position).WorldPos, 0.25f);
            }
        }
        Handles.DrawWireDisc(transform.position, transform.forward, hearingDistance);
    }

    void Start()
    {
        grid = FindObjectOfType<AStarGrid>();
        player = FindObjectOfType<PlayerMovement>();
        aStar = new(grid);
        startPos = transform.position;
        startRot = transform.rotation;
        if (idle)
        {
            state = GuardState.Idle;
            currentAction = StartCoroutine(Idle());
        }
        else
        {
            currentAction = StartCoroutine(PatrolPath());
        }
    }

    void Update()
    {
        if (state == GuardState.Chase)
        {
            timer += Time.deltaTime;
            if (timer > 0.25f && recalculate)
            {
                timer = 0;
                if (navigation != null)
                {
                    StopCoroutine(navigation);
                    navigation = null;
                }
            }
        }
        if (state != GuardState.Chase && TacticalVisor())
        {
            StopCoroutine(currentAction);
            state = GuardState.Chase;
            currentAction = StartCoroutine(Chase());
        }
        if (currentAction != null)
        {
            return;
        }
        switch (state)
        {
            case GuardState.Patrol:
                currentAction = StartCoroutine(PatrolPath());
                break;
            case GuardState.Search:
                state = GuardState.Patrol;
                currentAction = StartCoroutine(PatrolPath());
                break;
            case GuardState.Chase:
                state = GuardState.Search;
                currentAction = StartCoroutine(SearchArea());
                break;
            case GuardState.Idle:
                currentAction = StartCoroutine(Idle());
                break;
        }
    }

    void SoundHeard(Vector2 position)
    {
        if (state != GuardState.Chase && Vector2.Distance(position, transform.position) < hearingDistance) 
        {
            state = GuardState.Search;
            StopCoroutine(currentAction);
            targetPos = position;
            aStarPath = aStar.GetPath(transform.position, targetPos);
            currentAction = StartCoroutine(SearchArea());
        }
    }

    bool TacticalVisor()
    {
        Vector2 direction = player.transform.position - transform.position;
        if (direction.magnitude > hearingDistance)
        {
            return false;
        }
        float angle = Vector2.Angle(transform.up, direction);
        if (angle > 45)
        {
            return false;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, hearingDistance);
        if (hit && hit.transform != player.transform)
        {
            if (state == GuardState.Chase)
            {
                targetPos = grid.WorldToGrid(hit.transform.position).WorldPos;
                return false;
            }
            return false;
        }
        targetPos = player.transform.position;
        return true;
    }

    IEnumerator Idle()
    {
        while (state == GuardState.Idle)
        {
            Vector3 distance = startPos - transform.position;
            if (distance.magnitude > 0.1f)
            {
                aStarPath = aStar.GetPath(transform.position, startPos);
                yield return Navigate();
                while (Quaternion.Angle(transform.rotation, startRot) > 0.1f)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, startRot, rotationSpeed * Time.deltaTime);
                    yield return null;
                }
                transform.rotation = startRot;
            }
            yield return Scan(90, rotationSpeed / 2, true);
        }
    }
    IEnumerator SearchArea()
    {
        int loops = 0;
        while (loops < 3)
        {
            yield return Navigate();
            yield return LookAround(patrolPath[(currentPos + 1) % patrolPath.Length].position);
            Vector3 nearbyPoint = grid.GetRandomNearbyPoint(transform.position);
            aStarPath = aStar.GetPath(transform.position, nearbyPoint);
            loops++;
        }
        if (idle)
        {
            state = GuardState.Idle;
        }
        currentAction = null;
    }

    IEnumerator Chase()
    {
        while (state == GuardState.Chase)
        {
            navigation = StartCoroutine(Navigate());
            yield return new WaitUntil(()=> navigation == null);
            if (!TacticalVisor())
            {
                aStarPath = aStar.GetPath(transform.position, targetPos);
                currentAction = null;
                yield break;
            }
            aStarPath = aStar.GetPath(transform.position, targetPos);
            yield return null;
        }
    }

    IEnumerator PatrolPath()
    {
        FindNearestPoint();
        while (state == GuardState.Patrol)
        {
            if (aStarPath == null || aStarPath.Count == 0)
            {
                currentPos++;
                currentPos %= patrolPath.Length;
                targetPos = patrolPath[currentPos].position;
                aStarPath = aStar.GetPath(transform.position, targetPos);
            }
            yield return Navigate();
            yield return LookAround(patrolPath[(currentPos + 1) % patrolPath.Length].position);
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
        while (aStarPath != null && aStarPath.Count > 0)
        {
            Vector3 targetPoint = aStarPath[0];
            if ((targetPoint - transform.position).magnitude > 0.1f)
            {
                recalculate = false;
                Quaternion targetRotation = Quaternion.LookRotation(transform.position - targetPoint, Vector3.forward);
                targetRotation = new Quaternion(0, 0, targetRotation.z, targetRotation.w);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.05f * rotationSpeed * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
            }
            else
            {
                recalculate = true;
                aStarPath.RemoveAt(0);
            }
            yield return null;
        }
        navigation = null;
    }

    IEnumerator LookAround(Vector3 targetPos)
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetPos - transform.position);
        float angle = Vector2.SignedAngle(transform.up, (targetPos - transform.position).normalized);
        bool rotatedLeft = angle > 0;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 2 * rotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return Scan(90, rotationSpeed, rotatedLeft);
    }

    IEnumerator Scan(float fov, float rotationSpeed, bool rotation)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion left = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, fov / 2));
        Quaternion right = Quaternion.Euler(transform.eulerAngles - new Vector3(0, 0, fov / 2));
        if (rotation)
        {
            yield return RotationOrder(rotationSpeed, left, right);
        }
        else
        {
            yield return RotationOrder(rotationSpeed, right, left);
        }
        while (Quaternion.Angle(transform.rotation, startRotation) > 0.1f)
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RotationOrder(float rotationSpeed, Quaternion rotation1, Quaternion rotation2)
    {
        while (Quaternion.Angle(transform.rotation, rotation1) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation1, rotationSpeed * Time.deltaTime);
            yield return null;
        }
        while (Quaternion.Angle(gameObject.transform.rotation, rotation2) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation2, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void ResetGuard()
    {
        transform.position = startPos;
        transform.rotation = startRot;
        if (navigation != null) 
        { 
            StopCoroutine(navigation);
        }
        if (currentAction != null)
        {
            StopCoroutine(currentAction);
        }
        currentAction = navigation = null;
        aStarPath.Clear();
        if (idle)
        {
            state = GuardState.Idle;
            currentAction = StartCoroutine(Idle());
        }
        else
        {
            currentAction = StartCoroutine(PatrolPath());
        }
    }

    private void OnEnable()
    {
        Yeet.SoundGenerated += SoundHeard;
        FailState.ResetGame += ResetGuard;
    }

    private void OnDisable()
    {
        Yeet.SoundGenerated -= SoundHeard;
        FailState.ResetGame -= ResetGuard;
    }
}
