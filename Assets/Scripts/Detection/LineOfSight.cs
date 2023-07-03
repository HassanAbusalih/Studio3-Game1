using System;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] [Range(2, 90)] int rayCount;
    [SerializeField] float fov;
    [SerializeField] float viewDistance;
    [SerializeField] LayerMask mask;
    Vector3[] vertices;
    int[] triangles;
    Mesh mesh;
    MeshFilter viewCone;
    //bool caught;
    public static event Action CaughtPlayer;

    void Start()
    {
        viewCone = GetComponent<MeshFilter>();
        mesh = new();
        vertices = new Vector3[rayCount + 1];
        triangles = new int[3 * (rayCount - 1)];
    }

    void LateUpdate()
    {
        Detection();
    }

    void Detection()
    {
        float angle = 90 - fov / 2;
        vertices[0] = Vector3.zero;
        for (int i = 1; i <= rayCount; i++)
        {
            Vector3 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(direction), viewDistance, mask);
            if (hit.collider != null)
            {
                vertices[i] = transform.InverseTransformPoint(hit.point);
                if (hit.transform.TryGetComponent(out PlayerMovement player))
                {
                    //caught = true;
                    CaughtPlayer?.Invoke();
                }
            }
            else
            {
                vertices[i] = direction * viewDistance;
            }
            angle += fov / (rayCount - 1);
        }
        for (int i = 0; i < rayCount - 1; i++)
        {
            triangles[i * 3] = 0;
            triangles[(i * 3) + 1] = i + 1;
            triangles[(i * 3) + 2] = i + 2;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        viewCone.mesh = mesh;
    }
}
