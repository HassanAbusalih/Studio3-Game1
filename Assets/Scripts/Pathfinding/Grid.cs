using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int gridX;
    public int gridY;
    [SerializeField] float worldX;
    [SerializeField] float worldY;
    public Node[] grid;

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
                Vector3 gridPos = new Vector3(y , x);
                Vector3 worldPos = new Vector3(worldX * (0.5f + y - gridX / 2f), worldY * (0.5f + x - gridY / 2f));
                grid[currentNode] = new Node(gridPos, worldPos);
                grid[currentNode].walkable = Physics2D.OverlapBox(worldPos, new Vector2(worldX, worldY), 0) == null;
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        return grid[y * gridX + x];
    }

    void OnDrawGizmos()
    {
        if (grid == null) { return; }
        foreach (Node node in grid)
        {
            if (node.walkable)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(node.WorldPos, new Vector3(worldX, worldY));
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(node.WorldPos, new Vector3(worldX, worldY));
            }
        }
    }
}
