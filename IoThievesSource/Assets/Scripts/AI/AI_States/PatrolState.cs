using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI.AI_States
{
    public class PatrolState : AIState
    {
         private float speed = 2.0f;
         private float angularSpeed = 2f;
         private bool isMoving = true;
         private bool isInteracting = false;
        
         private Waypoint target;
         private Waypoint lastWaypoint;
         private float threshold = 0.8f; 
         private float angularThreshold = 45.0f; //degrees

        
        private Queue<Movement> recentMovements = new Queue<Movement>();
        private const int MAX_MOVEMENT_HISTORY = 15;


        public PatrolState(AIController npc) : base(npc){}

        public Waypoint ChooseNextWaypoint(Waypoint currentWaypoint)
        {
            if (currentWaypoint.neighbours == null || currentWaypoint.neighbours.Count == 0)
                return null;

            List<Waypoint> availableWaypoints = new List<Waypoint>();

            foreach (Waypoint neighbour in currentWaypoint.neighbours)
            {
                if (!IsInRecentMovements(currentWaypoint, neighbour))
                {
                    availableWaypoints.Add(neighbour);
                }
            }

            if (availableWaypoints.Count == 0)
            {
                availableWaypoints.AddRange(currentWaypoint.neighbours);
            }

            Waypoint nextWaypoint = availableWaypoints[Random.Range(0, availableWaypoints.Count)];

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

        public override void Enter() //Chooses the First waypoint to reach
        {
            LayerMask mask = LayerMask.GetMask("Default");
            if (!(lastWaypoint is null))        
            {
                Vector3 direction = lastWaypoint.transform.position - Npc.transform.position;
                float distance = direction.magnitude;
                
                if (!Physics.Raycast(
                        Npc.transform.position,
                        direction.normalized,
                        distance,
                        mask))           //If the last targeted destination is still reachable
                {
                    target = lastWaypoint;
                    return;
                }
            }

            Waypoint[] wps = UnityEngine.Object.FindObjectsByType<Waypoint>(
               FindObjectsInactive.Exclude,
               FindObjectsSortMode.None);
           
            float maxCheckDistance = 50f;
            float bestDistance = Mathf.Infinity;
            Waypoint bestWaypoint = null;
           
            foreach (Waypoint wp in wps)
            { 
                Vector3 dir = wp.transform.position - Npc.transform.position; 
                float distance = dir.magnitude;

                if (distance > maxCheckDistance)
                   continue; 
               
                if (!Physics.Raycast(
                       Npc.transform.position,
                       dir.normalized,
                       distance,
                       mask))
                { 
                    if (distance < bestDistance)
                    {
                       bestDistance = distance;
                       bestWaypoint = wp;
                    }
                }
            }
            target = bestWaypoint;
        }

        public override void Execute()
        {
            if (target is null)
                return;
            
            Vector3 dir = target.transform.position - Npc.transform.position;
            dir.Normalize();

            Vector3 forwardDir = Npc.transform.forward.normalized;
            
            float angle = Vector3.SignedAngle(forwardDir, dir, Vector3.up);
            float angleMag = Mathf.Abs(angle);
            
            isMoving = angleMag < angularThreshold;

            float rotateOf = angle * angularSpeed * Time.deltaTime;
            Npc.transform.Rotate(0,rotateOf,0);

            if (isMoving)
            {
                Vector3 velocity = dir * (speed * Time.deltaTime);
                Npc.transform.position  += new Vector3(velocity.x, 0, velocity.z);
            }

            float distance = (target.transform.position - Npc.transform.position).magnitude;
            
            if (distance < threshold)
            {
                if (target is PointOfInterest poi)
                {
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

        public override void Exit()
        {
            lastWaypoint = target;
        }
    }
    
    struct Movement
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
