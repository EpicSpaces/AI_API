using System.Collections.Generic;
using Priority_Queue;
using UnityEngine;
public static class AStar
{
    public static List<Node> Search(Node[] graph, Node start, Node goal)
    {
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();
        Dictionary<Node, float> cost_so_far = new Dictionary<Node, float>();

        List<Node> path = new List<Node>();

        FastPriorityQueue frontier = new FastPriorityQueue(160);
        frontier.Enqueue(new FastPriorityQueueNode(start), 0);

        came_from.Add(start, start);
        cost_so_far.Add(start, 0);

        Node current = graph[0];
        while (frontier.Count > 0)
        {
            current = frontier.Dequeue().Element;
            if (current == goal) break; // Early exit

            foreach (int next in graph[current.idx].Neighbours)
            {
                float new_cost = cost_so_far[current] + 1;
                if (!cost_so_far.ContainsKey(graph[next]) || new_cost < cost_so_far[graph[next]])
                {
                    cost_so_far[graph[next]] = new_cost;
                    came_from[graph[next]] = current;
                    float priority = new_cost + Mathf.Abs(graph[next].Position.x - goal.Position.x) + Mathf.Abs(graph[next].Position.y - goal.Position.y) + Mathf.Abs(graph[next].Position.z - goal.Position.z);
                    frontier.Enqueue(new FastPriorityQueueNode(graph[next]), priority);
                    graph[next].Priority = new_cost;
                }
            }
        }

        while (current != start)
        {
            path.Add(current);
            current = came_from[current];
        }
        path.Add(start);
        path.Reverse();

        return path;
    }
}