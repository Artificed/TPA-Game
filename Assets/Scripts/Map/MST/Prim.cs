using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prim
{
    private List<Room> Rooms = new List<Room>();

    public Prim(List<Room> rooms)
    {
        Rooms = rooms;
    }

    public List<(Vector2, Vector2)> GenerateMST()
    {
        List<(Vector2, Vector2)> mst = new List<(Vector2, Vector2)>();
        HashSet<int> visited = new HashSet<int>();
        List<(int, int, float)> edges = new List<(int, int, float)>();
        
        visited.Add(0);

        while (visited.Count < Rooms.Count)
        {
            foreach (int i in visited)
            {
                for (int j = 0; j < Rooms.Count; j++)
                {
                    if (!visited.Contains(j))
                    {
                        float distance = Vector2.Distance(Rooms[i].Center, Rooms[j].Center);
                        edges.Add((i, j, distance));
                    }
                }
            }

            var shortestEdge = (from: 0, to: 0, weight: float.MaxValue);
            foreach (var edge in edges)
            {
                if (edge.Item3 < shortestEdge.weight)
                {
                    shortestEdge = edge;
                }
            }

            mst.Add((Rooms[shortestEdge.from].Center, Rooms[shortestEdge.to].Center));
            visited.Add(shortestEdge.to);

            edges.RemoveAll(e => e.Item2 == shortestEdge.to);
        }

        return mst;
    }
}
