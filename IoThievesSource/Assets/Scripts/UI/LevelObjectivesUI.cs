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
        
        private const float YOffset = 0f;
        
        private readonly List<LevelObjectivesToUI> _objectivesToUI = new();
        
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
            objectiveUI.Initialize(objectiveScrObj.objectiveDescription);
            
            _objectivesToUI.Add(new LevelObjectivesToUI(objectiveUI, objectiveScrObj));
            
            UpdateHeight();
        }

        public void RemoveObjective(LevelObjectiveScrObj objectiveScrObj)
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
            windowRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _objectivesToUI.Count * gridLayout.cellSize.y + YOffset);
            
            objectiveParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical, _objectivesToUI.Count * gridLayout.cellSize.y);
        }

        public void OpenClose()
        {
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
