using System.Collections;
using Interactables.ThrowableDistraction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Interactables
{
    public class InteractToCreateDistraction : Interactable
    {
        [Header("Hackable Settings")]
        [SerializeField] private float timeToProduce;
        
        [SerializeField] private LayerMask playerLayer;
        
        [SerializeField] private InteractableTriggerArea hackTriggerArea;
        [SerializeField] private InteractableTriggerArea interactTriggerArea;
        [SerializeField] private ParticleSystem smokeVFX;
        [SerializeField] private Animator shakeAnimator;
        
        [Header("Throwable Settings")]
        [SerializeField] private GameObject distractionPrefab;
        [SerializeField] private float attractionRange;
        
        [Header("Progress UI")]
        [SerializeField] private GameObject progressBillboard;
        [SerializeField] private Image fillImage;
        [SerializeField] private GameObject checkObj;
        
        [HideInInspector] public bool canStartProduction = true;
        private bool _hasCreatedDistraction;

        #region Events

        [HideInInspector] public UnityEvent onPickUpThrowable;

        #endregion

        private void Awake()
        {
            progressBillboard.SetActive(false);
            checkObj.SetActive(false);
            fillImage.fillAmount = 0;
            
            interactTriggerArea.Init(this);
            hackTriggerArea.Init(this);
            
            hackTriggerArea.Lock(false);
            interactTriggerArea.Lock(true);
            
            // for testing
            shakeAnimator.SetBool("IsShaking", true);
        }

        public override void Interact()
        {
            if (timeToProduce == 0 && interactTriggerArea.IsPlayerInRange && interactTriggerArea.PlayerObject != null)
            {
                PlayerThrower player = interactTriggerArea.PlayerObject.GetComponent<PlayerThrower>();

                if (player.TryPickUp(Instantiate(
                            distractionPrefab, 
                            gameObject.transform.position, 
                            Quaternion.identity).GetComponent<Throwable>(), 
                        attractionRange))
                {
                    _hasCreatedDistraction = false;
                    canStartProduction = true;

                    onPickUpThrowable?.Invoke();
                }
            }
            
            if (canStartProduction && hackTriggerArea.IsPlayerInRange)
            {
                StartCoroutine(ProduceDistraction(timeToProduce));
            }
            else if (_hasCreatedDistraction && interactTriggerArea.IsPlayerInRange && interactTriggerArea.PlayerObject != null)
            {
                PlayerThrower player = interactTriggerArea.PlayerObject.GetComponent<PlayerThrower>();

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
                    
                    interactTriggerArea.Lock(true);
                    hackTriggerArea.Lock(false);

                    onPickUpThrowable?.Invoke();
                }
            }
        }

        private IEnumerator ProduceDistraction(float productionTime)
        {
            canStartProduction = false;
            smokeVFX.Play();
            shakeAnimator.SetBool("IsShaking", true);
            
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
            
            hackTriggerArea.Lock(true);
            interactTriggerArea.Lock(false);
            smokeVFX.Stop();
            shakeAnimator.SetBool("IsShaking", false);
        }
    }
}