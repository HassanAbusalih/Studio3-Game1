using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public float Gcost;
    public float Hcost;
    public float Fcost { get => Gcost + Hcost; }
    Vector3 gridPos;
    Vector3 worldPos;
    public Node parent;
    public bool walkable;

    public Node(Vector3 grid, Vector3 world)
    {
        gridPos = grid;
        worldPos = world;
    }

    public Vector3 GridPos { get => gridPos; }
    public Vector3 WorldPos { get => worldPos; }
}
