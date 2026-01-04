using System;
using Interactables;
using UnityEditor;
using UnityEngine;

namespace AI
{
    public class AttractedByStereo : MonoBehaviour
    {
        [SerializeField] private NpcScriptableObject data;
        [SerializeField] private Vector3 startingPosition;
        [SerializeField] private Vector3 dancingPosition;
        [SerializeField] private Stereo stereo;
        [SerializeField] private bool isDancing = false;
        [SerializeField] private float StartingYRotation;
        [SerializeField] private float DancingYRotation;

        private void Awake()
        {
            StartingYRotation = transform.rotation.y;
            DancingYRotation = stereo.transform.rotation.y;
            startingPosition = transform.position;
           dancingPosition.y =  transform.position.y;
        }

        private void Update()
        {
            if (stereo.IsPlayingMusic)
            {
                if(Vector3.Distance(transform.position, dancingPosition) > 0.1f){   //You must reach the dancing area
                    PlanMovement(dancingPosition);
                }
                else
                {                                                                   //Alor on dance
                    PlanRotation(DancingYRotation);
                    isDancing = true;
                    //DanceAnimation()
                }
            }
            else  // stereo is off
            {
                if (isDancing)              //Stop dancing
                {
                    isDancing = false;
                }

                if (Vector3.Distance(transform.position, startingPosition) > data.DistanceThreshold)        //reach the startingPosition
                {
                    PlanMovement(startingPosition);
                }               //Else idle
                else
                {
                    PlanRotation(StartingYRotation);
                }
            }
        }

        private void PlanMovement(Vector3 targetPosition)
        {
            Vector3 dir = targetPosition - transform.position;
            dir.Normalize();

            Vector3 forwardDir = transform.forward.normalized;

            float angleMag = AIUtilities.RotateTarget(transform, forwardDir, dir, data.AngularSpeed); 
            
            bool _isMoving = angleMag < data.AngularThreshold;

            if (_isMoving)
            {
                AIUtilities.MoveTarget(transform, dir, data.Speed);
            }
        }

        /*
        private void PlanRotation(float angle)
        {
            if (Math.Abs(transform.rotation.y - angle) != 0)
            {
                
            }
        }
        */
    }
    
}