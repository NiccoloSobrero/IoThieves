using System.Collections;
using Interactables.ThrowableDistraction;
using Levels;
using UnityEngine;
using UnityEngine.UI;

namespace Interactables
{
    public class InteractToCreateDistraction : MonoBehaviour, IInteractable
    {
        [Header("Hackable Settings")]
        [SerializeField] private float timeToProduce;
        
        [SerializeField] private float maxDistanceToInteract;
        [SerializeField] private float maxDistanceToPickUp;
        
        [SerializeField] private LayerMask playerLayer;
        
        [Header("Throwable Settings")]
        [SerializeField] private GameObject distractionPrefab;
        [SerializeField] private float attractionRange;
        
        [Header("Progress UI")]
        [SerializeField] private GameObject progressBillboard;
        [SerializeField] private Image fillImage;
        [SerializeField] private GameObject checkObj;
        
        private Collider[] _playerColliders = new Collider[1];
        
        public bool canStartProduction = true;
        private bool _hasCreatedDistraction = false;
        
        private TutorialObjective _tutorialObjective;

        private void Awake()
        {
            progressBillboard.SetActive(false);
            checkObj.SetActive(false);
            fillImage.fillAmount = 0;
        }

        public void Interact()
        {
            if (canStartProduction)
            {
                if (Physics.OverlapSphereNonAlloc(
                        gameObject.transform.position, 
                        maxDistanceToInteract, 
                        _playerColliders, 
                        playerLayer) > 0)
                {
                    StartCoroutine(ProduceDistraction(timeToProduce));
                }
            }
            else if (_hasCreatedDistraction)
            {
                if (Physics.OverlapSphereNonAlloc(
                        gameObject.transform.position,
                        maxDistanceToPickUp,
                        _playerColliders,
                        playerLayer) > 0)
                {
                    PlayerThrower player = _playerColliders[0].GetComponent<PlayerThrower>();

                    if (player.TryPickUp(Instantiate(
                            distractionPrefab, 
                            gameObject.transform.position, 
                            Quaternion.identity).GetComponent<Throwable>(), 
                        attractionRange))
                    {
                        progressBillboard.SetActive(false);
                        checkObj.SetActive(false);
                        fillImage.fillAmount = 0;
                        
                        _hasCreatedDistraction = false;
                        canStartProduction = true;

                        if (_tutorialObjective != null)
                        {
                            _tutorialObjective.Complete();
                            _tutorialObjective = null;
                        }
                    }
                }
            }
        }

        private IEnumerator ProduceDistraction(float productionTime)
        {
            canStartProduction = false;
            
            progressBillboard.SetActive(true);
            checkObj.SetActive(false);
            fillImage.fillAmount = 0;

            float timer = 0f;

            while (timer < productionTime)
            {
                timer += Time.deltaTime;
                
                fillImage.fillAmount = timer / productionTime;
                
                yield return null;
            }
            
            checkObj.SetActive(true);
            
            _hasCreatedDistraction = true;
        }

        public void SetObjective(TutorialObjective tutorialObjective)
        {
            _tutorialObjective = tutorialObjective;
        }
    }
}