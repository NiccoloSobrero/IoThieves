using System;
using System.Collections.Generic;
using AI.NavigationAndWaypoints;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI.AI_States
{
    public class SuspiciousState : AIState
    {
        private float speed = 1.50f;
        private float angularSpeed = 2f;
        private bool isMoving = true;
        
        private Waypoint destinationWaypoint;
        
        private Vector3 nextDestination;
        
        private float threshold = 0.3f; 
        private float angularThreshold = 30.0f; //degrees
        private float minDistance = 1.5f;        //Below this distance no intermediate points will be added
        private Vector3[] positions;
        private int positionIndex = 0;
        private float radius = 0.5f;
        
        private Queue<Movement> recentMovements = new Queue<Movement>();
        private const int MAX_MOVEMENT_HISTORY = 15;
        public SuspiciousState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            //Text : What's that -> Looking around animation??
            LayerMask mask = LayerMask.GetMask("Default");
            
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
            destinationWaypoint = bestWaypoint;
            ComputeMiddlePoints();
        }
        
        //Guardarsi intorno
        //Camminare in giro cercando di rimanere nell'area attorno al trigger
        //Lo fa per un tot e poi torna a "cazzeggiare??"
           
        //Idea:
        //Simile a Patrol MA: 
        //   -Raggiungere il prossimo waypoint usando un simil randomWalk -> Splittare la total distance e ogni tot scegliere a caso un intorno da raggiungere
        //   -Far ruotare la visuale del Npc rispetto al displacement della linea direttaa (diretto o inverso)
        public override void Execute()
        {
            if (destinationWaypoint is null)
                return;
            
            Debug.Log("Destination; "+ nextDestination);
            Vector3 dir = nextDestination - Npc.transform.position;
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

            float distance = (nextDestination - Npc.transform.position).magnitude;
            
            if (distance < threshold)
            { 
                    NextDestination();
                    isMoving = true;
            }
        }

        public override void Exit()
        {
          //Text: Must have been the wind 
        }
        
        public void ChooseNextWaypoint(Waypoint currentWaypoint)
        {
            if (currentWaypoint.neighbours == null || currentWaypoint.neighbours.Count == 0)
                return;

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

            destinationWaypoint = nextWaypoint;
            ComputeMiddlePoints();
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

        private void NextDestination()
        {
            if (Vector3.Distance(nextDestination, destinationWaypoint.transform.position) < 0.05f)
            { //If I reached a Waypoint ---> New Waypoint is Needed
                ChooseNextWaypoint(destinationWaypoint);
            }      
            else if (positionIndex >= positions.Length-1)
            {
                //I finished the middlePoints --> I have to reach the DestinationWayPoint
                nextDestination = destinationWaypoint.transform.position;
            }
            else
            {
                nextDestination = positions[positionIndex];
                positionIndex++;
            }
        }

        private void ComputeMiddlePoints()
        {
           float distance =  Vector3.Distance(Npc.transform.position, destinationWaypoint.transform.position);
           int points = Mathf.FloorToInt(distance / minDistance);
           positions = new Vector3[points];
           Debug.Log("There are this intermediate points : " + points);

           for (int i = 0; i < points; i++)
           {
               float t = (float)(i + 1) / (points + 1);

               float angle = Random.Range(0f, Mathf.PI * 2f);
               float x = Mathf.Cos(angle) * radius;
               float z = Mathf.Sin(angle) * radius;

               positions[i] = Vector3.Lerp(Npc.transform.position, destinationWaypoint.transform.position, t) +
                              new Vector3(x, .1f, z);
           }

           positionIndex = 0;
           NextDestination(); 
        }
        
    }
    
    /* Per gli Hackables
     *  - FindObjectOfType PointOfIntersts
     *  - Trova quello che ha il device impazzuto
     *  - A* per trovare la strada ed arrivarci
     *  - Fine solving --> Previous State (patrolling / suspicion)
     * 
     */
}