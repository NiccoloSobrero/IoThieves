using System.Collections.Generic;
using UnityEngine;

namespace AI.NavigationAndWaypoints
{
	public class PathFinding
	{
        public static List<Waypoint> FindPath(Waypoint start, Waypoint goal)
        {
            var openSet = new List<Waypoint> { start };
            var cameFrom = new Dictionary<Waypoint, Waypoint>();
        
            var gScore = new Dictionary<Waypoint, float> { [start] = 0 };
            var fScore = new Dictionary<Waypoint, float> { [start] = EuclideanDistance(start, goal) };

            while (openSet.Count > 0)
            {
                //Best Node Estimated
                Waypoint current = GetMostPromisingNode(openSet, fScore);

                if (current == goal)
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(current);

                foreach (Waypoint neighbor in current.neighbours)
                {
                    float tentativeG = gScore[current] + EuclideanDistance(current, neighbor);

                    if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeG;
                        fScore[neighbor] = tentativeG + EuclideanDistance(neighbor, goal);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            return null; //No Path 
        }

        static float EuclideanDistance(Waypoint a, Waypoint b)
        {
            return Vector3.Distance(a.transform.position, b.transform.position);
        }

        static Waypoint GetMostPromisingNode(List<Waypoint> openSet, Dictionary<Waypoint, float> fScore)
        {
            Waypoint best = openSet[0];
            float bestScore = fScore.ContainsKey(best) ? fScore[best] : Mathf.Infinity;

            foreach (var wp in openSet)
            {
                float score = fScore.ContainsKey(wp) ? fScore[wp] : Mathf.Infinity;

                if (score < bestScore)
                {
                    bestScore = score;
                    best = wp;
                }
            }

            return best;
        }

        static List<Waypoint> ReconstructPath(Dictionary<Waypoint, Waypoint> cameFrom, Waypoint current)
        {
            var path = new List<Waypoint> { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }

            return path;
        }
    
	}
}