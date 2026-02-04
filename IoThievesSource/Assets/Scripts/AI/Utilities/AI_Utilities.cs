using AI.AI_States;
using AI.NavigationAndWaypoints;
using UnityEngine;

namespace AI
{
    public class AIUtilities
    {
        public static Waypoint FindNearestWaypoint(Vector3 position)
        {
            LayerMask mask = LayerMask.GetMask("Default");
            
            Waypoint[] wps = Object.FindObjectsByType<Waypoint>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None);
           
            float maxCheckDistance = 10f;
            float bestDistance = Mathf.Infinity;
            Waypoint bestWaypoint = null;
           
            foreach (Waypoint wp in wps)
            { 
                Vector3 dir = wp.transform.position - position; 
                float distance = dir.magnitude;

                if (distance > maxCheckDistance)
                    continue;

                RaycastHit hit;
                if(!Physics.Raycast(
                        position,
                        dir.normalized,
                        out hit,
                        distance,
                        mask, 
                        QueryTriggerInteraction.Ignore))
                {
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestWaypoint = wp;
                    }
                }
             /*  else
                {
                    Debug.Log("There is" +hit.collider.gameObject.name+" in between the npc and the waypoint "+ wp.name);
                }*/
            }
            
            return bestWaypoint;
        }

        public static void MoveTarget(Transform npcTransform, Vector3 dir, float speed)
        {
            Vector3 velocity = dir * (speed * Time.deltaTime);
            npcTransform.position  += new Vector3(velocity.x, 0, velocity.z);
        }

        public static float RotateTarget(Transform npcTransform, Vector3 forwardDir, Vector3 dir, float angularSpeed)
        {
            float angle = Vector3.SignedAngle(forwardDir, dir, Vector3.up);
            float angleMag = Mathf.Abs(angle);
            
            float rotateOf = angle * angularSpeed * Time.deltaTime;
            npcTransform.Rotate(0,rotateOf,0);        
            return angleMag;
        }

        public static AINames RetrieveAIName(AIState currentState)
        {
            switch (currentState)
            {
                case PatrolState : return AINames.Patrol;
                case SuspiciousState : return AINames.Suspicious;
                case IdleState : return AINames.Idle;
                case DeviceHackedState : return AINames.DeviceHacked;
                case NoiseHeardState : return AINames.NoiseHeardState;
                case PathCyclingState : return AINames.PathCycling;
                case StunnedState :  return AINames.StunnedState;
                case GoToDestination : return AINames.GoToDestination;
                case BackToPreviousPosition : return AINames.BackToPosition;
                case null : return AINames.NULL;
                default : return AINames.NAME_NOT_FOUND;
            }
        }

        public static bool IsVisible(GameObject from, GameObject to)
        {
            Vector3 direction = to.transform.position - from.transform.position;
            return !Physics.Raycast(from.transform.position, direction.normalized, direction.magnitude, 
                LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore);
        }
    }
}