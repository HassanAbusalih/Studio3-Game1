using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar 
{
    Grid grid;
    List<Node> openList = new List<Node>();
    List<Node> closedList = new List<Node>();
    List<Node> neighbors = new List<Node>();
    Node end;
    Node currentNode;

    public List<Vector3> GetPath(Node start, Node end, Grid grid)
    {
        this.grid = grid;
        this.end = end;
        openList.Add(start);
        while (end.parent == null)
        {
            openList.Sort();
            currentNode = openList[0];
            openList.Remove(currentNode);
            AddNeighbors(currentNode);
            foreach (Node neighbor in neighbors)
            {
                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
            neighbors.Clear();
        }
        openList.Clear();

        List<Vector3> path = new List<Vector3>();
        currentNode = end;
        while (currentNode != start)
        {
            path.Add(currentNode.WorldPos);
            currentNode = currentNode.parent;
        }
        path.Add(currentNode.WorldPos);
        path.Reverse();
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
                if (!node.walkable || closedList.Contains(node))
                {
                    continue;
                }
                float deltaX = x - currentNode.GridPos.x - 1;
                float deltaY = y - currentNode.GridPos.y - 1;
                float newGCost = currentNode.Gcost + Mathf.Sqrt(deltaX * deltaX + deltaY * deltaY);
                if (node.Gcost == 0 || node.Gcost > newGCost)
                {
                    node.Gcost = newGCost;
                    float straight = Mathf.Abs(x - y);
                    float diagonal = (Mathf.Max(x, y) - straight) * Mathf.Sqrt(2);
                    node.Hcost = diagonal + straight;
                    node.parent = currentNode;
                }
                neighbors.Add(node);
            }
        }
    }
}
