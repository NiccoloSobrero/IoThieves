using AI;
using AI.AI_States;
using AI.NavigationAndWaypoints;
using Enemy;
using UnityEngine;
using UnityEngine.Events;

namespace Interactables
{
    [RequireComponent(typeof(Device))]
    public class InteractToAttract2: Interactable
    {
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float attractionRange;
        
        [SerializeField] private InteractableTriggerArea triggerArea;
        
        private const int MaxAttractableEnemies = 10;
        private readonly Collider[] _enemies = new Collider[MaxAttractableEnemies];
        [SerializeField] PointOfInterest pointOfInterest;

        #region Events

        [HideInInspector] public UnityEvent onInteract;

        #endregion

        private void Awake()
        {
            triggerArea.Init(this);
        }

        public override void Interact()
        {
            if (!triggerArea.IsPlayerInRange) return;
            
            int size = Physics.OverlapSphereNonAlloc(gameObject.transform.position, attractionRange, _enemies, enemyLayer);
            Vector3 attractPos = transform.position;
            attractPos.y = 0;

            Debug.Log("I found this number of enemies : " + size);
            for (int i = 0; i < size; i++)
            {
                AIController enemy =  _enemies[i].GetComponentInParent<AIController>();
                if (enemy is not null)
                {
                    print("Devo attrarre questo simpaticone");
                    DeviceHackedState hacked = (DeviceHackedState)enemy.StateFactory.GetState(AINames.DeviceHacked);
                    hacked.SetDevice(pointOfInterest);
                    pointOfInterest.AssignedDevice.HasBeenHacked();
                    enemy.ChangeState(AINames.DeviceHacked);
                    //
                }
            }

            onInteract?.Invoke();
        }
    }
}
