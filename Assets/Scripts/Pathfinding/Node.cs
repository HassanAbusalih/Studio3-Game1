using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Node : IComparable
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

    public int CompareTo(object obj)
    {
        Node node = obj as Node;
        if (node == null)
        {
            return 0;
        }
        if (Fcost > node.Fcost)
        {
            return -1;
        }
        else if (Fcost < node.Fcost)
        {
            return 1;
        }
        return 0;
    }
}
