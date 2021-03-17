using System;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 Position;
    public float Priority;
    public List<int> Neighbours;
    public int idx;
    public Node(Vector3 v)
    {
        Position = new Vector3(v.x, v.y,v.z);
        Neighbours = new List<int>();
    }

    public int CompareTo(Node other)
    {
        if (this.Priority < other.Priority) return -1;
        else if (this.Priority > other.Priority) return 1;
        else return 0;
    }
}
