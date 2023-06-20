using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    Grid grid;
    List<Node> openList = new List<Node>();
    List<Node> closedList = new List<Node>();
    List<Node> neighbors = new List<Node>();
    Node currentNode;

    void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    List<Vector3> FindPath(Node start, Node end)
    {
        currentNode = start;
        while (currentNode != end)
        {
            AddNeighbors(currentNode);

        }
        List<Vector3> path = new List<Vector3>();
        //when looping through the parents, add to path
        return path;
    }

    void AddNeighbors(Node currentNode)
    {
        for (int x = (int)currentNode.GridPos.x - 1; x < (int)currentNode.GridPos.x + 2; x++)
        {
            if (x < 0 || x > grid.gridX)
            {
                continue;
            }
            for (int y = (int)currentNode.GridPos.y - 1; y < (int)currentNode.GridPos.y + 2; y++)
            {
                if (y < 0 || y > grid.gridY)
                {
                    continue;
                }
                if (x == (int)currentNode.GridPos.x && y == (int)currentNode.GridPos.y)
                {
                    y++;
                }
                Node node = grid.GetNode(x, y);
                neighbors.Add(node);
            }
        }
    }
}
