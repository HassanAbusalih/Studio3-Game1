using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    public static Vector3 currentRespawnPosition = Vector3.zero;
    public static Quaternion currentRespawnRotation;
    PlayerMovement player;

    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        if (currentRespawnPosition == Vector3.zero)
        {
            currentRespawnPosition = player.transform.position;
            currentRespawnRotation = player.transform.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == player.transform && respawnPoint.position != currentRespawnPosition)
        {
            currentRespawnPosition = respawnPoint.position;
            currentRespawnRotation = respawnPoint.rotation;
        }
    }
}
