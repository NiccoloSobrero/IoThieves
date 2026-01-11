using System.Collections;
using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelObjectivesUI : MonoBehaviour
    {
        private static readonly int Open = Animator.StringToHash("Open");
        [SerializeField] private RectTransform windowRect;
        
        [SerializeField] private GameObject objectivePrefab;
        [SerializeField] private Transform objectiveParent;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private Animator animator;
        private bool _isOpened;
        
        private const float MaxHeight = 400f;
        
        private readonly List<LevelObjectivesToUI> _objectivesToUI = new();
        
        private const float AutoCloseDelay = 5f;
        private Coroutine _autoCloseCoroutine;
        
        public void AddObjective(LevelObjectiveScrObj objectiveScrObj)
        {
            foreach (var objective in _objectivesToUI)
            {
                if (objective.ObjectiveScrObj == objectiveScrObj)
                {
                    return;
                }
            }
            
            GameObject newObjective = Instantiate(objectivePrefab, objectiveParent);
            newObjective.transform.SetAsFirstSibling();
            
            ObjectivePrefab objectiveUI = newObjective.GetComponent<ObjectivePrefab>();
            objectiveUI.Initialize(objectiveScrObj.Init());
            
            _objectivesToUI.Add(new LevelObjectivesToUI(objectiveUI, objectiveScrObj));

            AutoOpen();
            
            UpdateHeight();
        }

        public void UpdateObjective(LevelObjectiveScrObj objectiveScrObj)
        {
            foreach (var objective in _objectivesToUI)
            {
                if (objective.ObjectiveScrObj == objectiveScrObj)
                {
                    string nextStep = objective.ObjectiveScrObj.NextStep(out bool noMoreSteps);

                    if (noMoreSteps)
                    {
                        objective.ObjectiveUI.transform.SetAsLastSibling();
                        StartCoroutine(StrikeTextCor(nextStep, objective, 0.03f));
                    }
                    else
                    {
                        objective.ObjectiveUI.transform.SetAsFirstSibling();
                        objective.ObjectiveUI.UpdateText(nextStep);
                    }
                    
                    AutoOpen();
                    
                    return;
                }
            }
        }

        private void AutoOpen()
        {
            if (_autoCloseCoroutine != null)
            {
                StopCoroutine(_autoCloseCoroutine);
                _autoCloseCoroutine = StartCoroutine(AutoCloseCoroutine());
                
                return;
            }
            
            if (_isOpened) return;
            
            _isOpened = true;
            animator.SetBool(Open, _isOpened);
            _autoCloseCoroutine = StartCoroutine(AutoCloseCoroutine());
        }

        private IEnumerator AutoCloseCoroutine()
        {
            yield return new WaitForSeconds(AutoCloseDelay);

            _autoCloseCoroutine = null;
            _isOpened = false;
            animator.SetBool(Open, _isOpened);
        }

        private IEnumerator StrikeTextCor(string text, LevelObjectivesToUI objectiveToUI, float delayPerChar)
        {
            string original = text;

            for (int i = 1; i <= original.Length; i++)
            {
                string barred =
                    "<s>" + original.Substring(0, i) + "</s>" +
                    original.Substring(i);

                objectiveToUI.ObjectiveUI.UpdateText(barred);
                yield return new WaitForSeconds(delayPerChar);
            }
            
            RemoveObjective(objectiveToUI.ObjectiveScrObj);
        }

        private void RemoveObjective(LevelObjectiveScrObj objectiveScrObj)
        {
            foreach (var objective in _objectivesToUI)
            {
                if (objective.ObjectiveScrObj == objectiveScrObj)
                {
                    objective.ObjectiveUI.transform.SetAsLastSibling();
                    objective.ObjectiveUI.SetCompleted();
                    
                    UpdateHeight();
                    return;
                }
            }
        }

        private void UpdateHeight()
        {
            float newHeight = _objectivesToUI.Count * gridLayout.cellSize.y;
            
            objectiveParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical, newHeight);
            
            if (newHeight > MaxHeight)
            {
                newHeight = MaxHeight;
            }
            
            windowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
        }

        public void OpenClose()
        {
            if (_autoCloseCoroutine != null)
            {
                StopCoroutine(_autoCloseCoroutine);
            }
            
            _isOpened = !_isOpened;
            
            animator.SetBool(Open, _isOpened);
        }
    }

    public class LevelObjectivesToUI
    {
        public ObjectivePrefab ObjectiveUI;
        public LevelObjectiveScrObj ObjectiveScrObj;

        public LevelObjectivesToUI(ObjectivePrefab objectiveUI, LevelObjectiveScrObj objectiveScrObj)
        {
            ObjectiveUI = objectiveUI;
            ObjectiveScrObj = objectiveScrObj;
        }
    }
}
