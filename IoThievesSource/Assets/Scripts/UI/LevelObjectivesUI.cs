using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelObjectivesUI : MonoBehaviour
    {
        [SerializeField] private RectTransform windowRect;
        
        [SerializeField] private GameObject objectivePrefab;
        [SerializeField] private Transform objectiveParent;
        [SerializeField] private GridLayoutGroup gridLayout;
        private const float YOffset = 70f;
        
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
            ObjectivePrefab objectiveUI = newObjective.GetComponent<ObjectivePrefab>();
            objectiveUI.Initialize(objectiveScrObj.objectiveDescription);
            
            _objectivesToUI.Add(new LevelObjectivesToUI(newObjective, objectiveScrObj));
            
            UpdateHeight();
        }

        public void RemoveObjective(LevelObjectiveScrObj objectiveScrObj)
        {
            foreach (var objective in _objectivesToUI)
            {
                if (objective.ObjectiveScrObj == objectiveScrObj)
                {
                    Destroy(objective.ObjectiveUI);
                    
                    _objectivesToUI.Remove(objective);
                    
                    UpdateHeight();
                    return;
                }
            }
        }

        private void UpdateHeight()
        {
            windowRect.sizeDelta = new Vector2(
                windowRect.sizeDelta.x, 
                _objectivesToUI.Count * gridLayout.cellSize.y + YOffset);
        }
    }

    public class LevelObjectivesToUI
    {
        public GameObject ObjectiveUI;
        public LevelObjectiveScrObj ObjectiveScrObj;

        public LevelObjectivesToUI(GameObject objectiveUI, LevelObjectiveScrObj objectiveScrObj)
        {
            ObjectiveUI = objectiveUI;
            ObjectiveScrObj = objectiveScrObj;
        }
    }
}
