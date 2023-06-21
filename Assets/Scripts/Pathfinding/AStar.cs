using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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
        start = grid.WorldToGrid(startPos);
        end = grid.WorldToGrid(endPos);
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
                return null;
            }
        }
        openList.Clear();
        List<Vector3> path = new();
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
            if (x < 0 || x >= grid.gridX - 1)
            {
                continue;
            }
            for (int y = (int)currentNode.GridPos.y - 1; y < (int)currentNode.GridPos.y + 2; y++)
            {
                if (y < 0 || y >= grid.gridY - 1)
                {
                    continue;
                }
                if (x == (int)currentNode.GridPos.x && y == (int)currentNode.GridPos.y)
                {
                    continue;
                }
                Node node = grid.GetNode(x, y);
                if (node.version != version)
                {
                    node.Reset(version);
                }
                if (!node.walkable || node.closed)
                {
                    continue;
                }
                float relativeX = x - currentNode.GridPos.x;
                float relativeY = y - currentNode.GridPos.y;
                float newGCost = currentNode.Gcost + Mathf.Sqrt(relativeX * relativeX + relativeY * relativeY);
                if (node.Gcost == 0 || node.Gcost > newGCost)
                {
                    node.Gcost = newGCost;
                    float deltaX = Mathf.Abs(x - end.GridPos.x);
                    float deltaY = Mathf.Abs(y - end.GridPos.y);
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
