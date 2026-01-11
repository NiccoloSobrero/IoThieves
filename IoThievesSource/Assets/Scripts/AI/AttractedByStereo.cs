using System;
using Interactables;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace AI
{
    public class AttractedByStereo : MonoBehaviour
    {
        public event Action<bool> OnMovementVarChanged;
        public event Action<bool> OnDancingVarChanged;
        
        [SerializeField] private NpcScriptableObject data;
        [SerializeField] private Vector3 startingPosition;
        [SerializeField] private Vector3 dancingPosition;
        [SerializeField] private Stereo stereo;
        [SerializeField] private bool isDancing = false;
        [SerializeField] private float StartingYRotation;
        [SerializeField] private float DancingYRotation;
        private bool _isMoving = false;
        
        private bool IsMoving
        {
            get => _isMoving;
            set
            {
                if (_isMoving == value) return;
                _isMoving = value;
                OnMovementVarChanged?.Invoke(_isMoving); 
            }
        }

        private bool IsDancing
        {
            get => isDancing;
            set
            {
                if (isDancing == value) return;
                
                isDancing = value;
                OnDancingVarChanged?.Invoke(isDancing);
            }
        }

        private void Awake()
        {
            StartingYRotation = Mathf.Atan2(transform.forward.x, transform.forward.z) * Mathf.Rad2Deg;
            
            var stereoDir = stereo.transform.position - dancingPosition;
            DancingYRotation = Mathf.Atan2(stereoDir.x, stereoDir.z) * Mathf.Rad2Deg;
            
            Debug.Log("DancingYRotation : "+ DancingYRotation+"\nStartingYRotation : " + StartingYRotation);
            startingPosition = transform.position;
           dancingPosition.y =  transform.position.y;
           
        }

        private void Update()
        {
            if (stereo.IsPlayingMusic)
            {
                //Debug.Log("Stereo? "+ stereo.IsPlayingMusic);
                if(Vector3.Distance(transform.position, dancingPosition) > 0.1f){   //You must reach the dancing area
                    IsMoving = true;
                    PlanMovement(dancingPosition);
                }
                else
                {                                                                   //Alor on dance
                    Debug.Log("rotate towards the stereo");
                    IsMoving = false;
                    PlanRotation(DancingYRotation);
                    IsDancing = true;
                    //DanceAnimation()
                }
            }
            else  // stereo is off
            {
                //Debug.Log("Stereo? "+ stereo.IsPlayingMusic);
                if (IsDancing)              //Stop dancing
                {
                    IsDancing = false;
                }

                if (Vector3.Distance(transform.position, startingPosition) > data.DistanceThreshold)        //reach the startingPosition
                {
                    IsMoving = true;
                    PlanMovement(startingPosition);
                }              
                else
                {
                    IsMoving = false;
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
            
            IsMoving = angleMag < data.AngularThreshold;

            if (IsMoving)
            {
                AIUtilities.MoveTarget(transform, dir, data.Speed);
            }
        }

        
        private void PlanRotation(float angle)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, angle)) <= 1.0f)
                return;
            
            float newY = Mathf.MoveTowardsAngle(transform.eulerAngles.y, angle, data.AngularSpeed* 50 * Time.deltaTime);
            
            transform.rotation = Quaternion.Euler(0, newY, 0);
        }
        
    }
    
}