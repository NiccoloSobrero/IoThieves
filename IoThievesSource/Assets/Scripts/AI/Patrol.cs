using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using UnityEngine;

namespace AI
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField] private float speed = 2.0f;
        [SerializeField] private float angularSpeed = .4f;
        [SerializeField] private bool isMoving = true;
        [SerializeField] private bool isInteracting = false;
        
        [SerializeField] private Waypoint target;
        private Waypoint[,] lastchoices = new Waypoint[10, 2];
        [SerializeField] private float threshold; 
        [SerializeField] private float angularThreshold = 30.0f; //degrees

        
        private Queue<Movement> recentMovements = new Queue<Movement>();
        private const int MAX_MOVEMENT_HISTORY = 15;
        
        void Update()
        {
            if (target is null)
                return;
            
            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();

            Vector3 forwardDir = transform.forward.normalized;
            
            float angle = Vector3.SignedAngle(forwardDir, dir, Vector3.up);
            float angleMag = Mathf.Abs(angle);
            
            isMoving = angleMag < angularThreshold;

            float rotateOf = angle * angularSpeed * Time.deltaTime;
            transform.Rotate(0,rotateOf,0);

            if (isMoving)
            {
                Vector3 velocity = dir * (speed * Time.deltaTime);
                transform.position  += new Vector3(velocity.x, 0, velocity.z);
            }

            float distance = (target.transform.position - transform.position).magnitude;
            
            if (distance < threshold)
            {
                if (target is PointOfInterest poi)
                {
                    Debug.Log("THIS IS A POI");
                    isMoving = false;
                    isInteracting = !poi.interact();
                }

                if (!isInteracting)
                {
                    target = ChooseNextWaypoint(target);
                    isMoving = true;
                }
                
            }
        }
        public Waypoint ChooseNextWaypoint(Waypoint currentWaypoint)
        {
            if (currentWaypoint.neighbours == null || currentWaypoint.neighbours.Count == 0)
                return null;

            // Crea lista di waypoint disponibili escludendo quelli già visitati
            List<Waypoint> availableWaypoints = new List<Waypoint>();

            foreach (Waypoint neighbour in currentWaypoint.neighbours)
            {
                if (!IsInRecentMovements(currentWaypoint, neighbour))
                {
                    availableWaypoints.Add(neighbour);
                }
            }

            // Se tutti i movimenti sono stati già fatti, sceglie da tutti i vicini
            if (availableWaypoints.Count == 0)
            {
                availableWaypoints.AddRange(currentWaypoint.neighbours);
            }

            // Sceglie casualmente
            Waypoint nextWaypoint = availableWaypoints[Random.Range(0, availableWaypoints.Count)];

            // Salva il movimento
            AddMovement(currentWaypoint, nextWaypoint);

            return nextWaypoint;
        }

        private void AddMovement(Waypoint from, Waypoint to)
        {
            // Rimuove il movimento più vecchio se abbiamo raggiunto il limite
            if (recentMovements.Count >= MAX_MOVEMENT_HISTORY)
            {
                recentMovements.Dequeue();
            }

            recentMovements.Enqueue(new Movement(from, to));
        } 
        
        private bool IsInRecentMovements(Waypoint from, Waypoint to)
        {
            foreach (Movement movement in recentMovements)
            {
                if (movement.Equals(from, to))
                    return true;
            }
            return false;
        }
        
        private struct Movement
        {
            public Waypoint from;
            public Waypoint to;

            public Movement(Waypoint from, Waypoint to)
            {
                this.from = from;
                this.to = to;
            }

            public bool Equals(Waypoint fromWaypoint, Waypoint toWaypoint)
            {
                return from == fromWaypoint && to == toWaypoint;
            }
        } 
    }
}
