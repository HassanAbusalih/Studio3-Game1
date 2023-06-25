using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public int gridX;
    public int gridY;
    [SerializeField] float cellX;
    [SerializeField] float cellY;
    public Node[] grid;
    public int version;

    private void Awake()
    {
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

    public Node GetNearestWalkable(Node node)
    {
        Queue<Node> queue = new();
        HashSet<Node> visited = new();
        queue.Enqueue(node);
        visited.Add(node);
        while (queue.Count > 0)
        {
            Node newNode = queue.Dequeue();
            if (newNode.walkable)
            {
                return newNode;
            }
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int actualX = Mathf.Clamp((int)newNode.GridPos.x + x, 0, gridX - 1);
                    int actualY = Mathf.Clamp((int)newNode.GridPos.y + y, 0, gridY - 1);
                    Node neighbor = GetNode(actualX, actualY);
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }

    /*void OnDrawGizmos()
    {
        if (grid == null) { return; }
        foreach (Node node in grid)
        {
            if (node.walkable)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(node.WorldPos, new Vector3(cellX, cellY));
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(node.WorldPos, new Vector3(cellX, cellY));
            }
        }
    }*/
}
