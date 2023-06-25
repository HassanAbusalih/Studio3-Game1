using System.Collections.Generic;
using UnityEngine;

public class AStar 
{
    public AStar(AStarGrid grid)
    {
        this.grid = grid;
    }

    AStarGrid grid;
    List<Node> openList = new();
    List<Node> neighbors = new();
    Node currentNode;
    Node end;
    Node start;
    int version;


    public List<Vector3> GetPath(Vector3 startPos, Vector3 endPos)
    {
        version = grid.version;
        grid.version++;
        start = grid.WorldToGrid(startPos);
        end = grid.WorldToGrid(endPos);
        if (!start.walkable)
        {
            start = grid.GetNearestWalkable(start);
        }
        if (!end.walkable)
        {
            end = grid.GetNearestWalkable(end);
        }
        start.Reset(version);
        end.Reset(version);
        openList.Add(start);
        version++;
        while (end.parent == null)
        {
            openList.Sort();
            currentNode = openList[0];
            currentNode.closed = true;
            openList.RemoveAt(0);
            AddNeighbors(currentNode);
            foreach (Node neighbor in neighbors)
            {
                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
            neighbors.Clear();
            if (openList.Count == 0)
            {
                Debug.Log("No valid path.");
                return null;
            }
        }
        openList.Clear();
        List<Vector3> path = new();
        currentNode = end;
        int safetyCounter = 0;
        while (currentNode != start)
        {
            path.Add(currentNode.WorldPos);
            currentNode = currentNode.parent;
            safetyCounter++; 
            if (safetyCounter > grid.grid.Length - 1)
            {
                Debug.Log("Node parent overwritten. Path lost.");
                return null;
            }
        }
        path.Add(currentNode.WorldPos);
        path.Reverse();
        return path;
    }

    void AddNeighbors(Node currentNode)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int actualX = Mathf.Clamp((int)currentNode.GridPos.x + x, 0, grid.gridX - 1);
                int actualY = Mathf.Clamp((int)currentNode.GridPos.y + y, 0, grid.gridY - 1);
                Node node = grid.GetNode(actualX, actualY);
                if (node.version != version)
                {
                    node.Reset(version);
                }
                if (!node.walkable || node.closed)
                {
                    continue;
                }
                float newGCost = currentNode.Gcost + Mathf.Sqrt(x * x + y * y);
                if (node.Gcost == 0 || node.Gcost > newGCost)
                {
                    node.Gcost = newGCost;
                    float deltaX = Mathf.Abs(actualX - end.GridPos.x);
                    float deltaY = Mathf.Abs(actualY - end.GridPos.y);
                    float diagonal = Mathf.Sqrt(2) * Mathf.Min(deltaX, deltaY);
                    float straight = Mathf.Abs(deltaX - deltaY);
                    node.Hcost = diagonal + straight;
                    //node.Hcost = Mathf.Abs(x - end.GridPos.x) + Mathf.Abs(y - end.GridPos.y);
                    node.parent = currentNode;
                }
                neighbors.Add(node);
            }
        }
    }
}
