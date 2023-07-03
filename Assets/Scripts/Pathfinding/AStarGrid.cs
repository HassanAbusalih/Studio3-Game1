using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public int gridX;
    public int gridY;
    [SerializeField] float cellX;
    [SerializeField] float cellY;
    int mask;
    public Node[] grid;
    public int version;

    private void Awake()
    {
        mask = ~(1 << LayerMask.NameToLayer("Projectile"));
        grid = new Node[gridX * gridY];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int y = 0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                int currentNode = y * gridX + x;
                Vector3 gridPos = new Vector3(x , y);
                //offset grid by half so the center is at world position (0,0), and offset each node's world pos by half of the cell's size
                Vector3 worldPos = new Vector3(cellX * (0.5f + x - gridX / 2f), cellY * (0.5f + y - gridY / 2f));
                grid[currentNode] = new Node(gridPos, worldPos);
                grid[currentNode].walkable = Physics2D.OverlapCircle(worldPos, cellX/4) == null;
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        int num = y * gridX + x;
        if (num > grid.Length)
        {
            Debug.Log($"{num} is invalid, at x: {x} and y: {y}");
        }
        return grid[num];
    }

    public Node WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt((worldPos.x / cellX) + (gridX / 2f) - 0.5f);
        int y = Mathf.RoundToInt((worldPos.y / cellY) + (gridY / 2f) - 0.5f);
        x = Mathf.Clamp(x, 0, gridX - 1);
        y = Mathf.Clamp(y, 0, gridY - 1);
        return GetNode(x, y);
    }

    public Node GetNearestWalkable(Node currentNode, Vector2 worldPos)
    {
        int size = 6;
        Node closest = null;
        float minDistance = float.MaxValue;
        for (int x = -size/2; x <= size/2; x++)
        {
            for (int y = -size/2; y <= size/2; y++)
            {
                int actualX = Mathf.Clamp((int)currentNode.GridPos.x + x, 0, gridX - 1);
                int actualY = Mathf.Clamp((int)currentNode.GridPos.y + y, 0, gridY - 1);
                Node newNode = GetNode(actualX, actualY);
                if (newNode.walkable)
                {
                    float distance = Vector2.Distance(worldPos, newNode.WorldPos);
                    if (distance < minDistance)
                    {
                        closest = newNode;
                        minDistance = distance;
                    }
                }
            }
        }
        return closest;
    }

    public Vector3 GetRandomNearbyPoint(Vector3 worldPos)
    {
        int size = 4;
        Node currentNode = WorldToGrid(worldPos);
        List<Node> nodes = new List<Node>();
        for (int x = -size/2; x <= size/2; x++)
        {
            for (int y = -size/2; y <= size/2; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int actualX = Mathf.Clamp((int)currentNode.GridPos.x + x, 0, gridX - 1);
                int actualY = Mathf.Clamp((int)currentNode.GridPos.y + y, 0, gridY - 1);
                Node newNode = GetNode(actualX, actualY);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, newNode.WorldPos, (worldPos - newNode.WorldPos).magnitude, mask);
                if (newNode.walkable && hit.collider == null)
                {
                    nodes.Add(newNode);
                }
            }
        }
        if (nodes.Count == 0)
        {
            return currentNode.WorldPos;
        }
        return nodes[Random.Range(0, nodes.Count)].WorldPos;
    }

    /*void OnDrawGizmos()
    {
        if (grid == null) { return; }
        foreach (Node node in grid)
        {
            if (node.walkable)
            {
                Gizmos.color = new(0, 1, 0, 0.1f);
                Gizmos.DrawWireCube(node.WorldPos, new Vector3(cellX, cellY));
            }
            else
            {
                Gizmos.color = new(1, 0, 0, 0.2f);
                Gizmos.DrawWireCube(node.WorldPos, new Vector3(cellX, cellY));
            }
        }
    }*/
}
