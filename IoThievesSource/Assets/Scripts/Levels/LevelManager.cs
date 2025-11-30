using System;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelScrObj level;
        
        [SerializeField] private GameObject objectivePrefab;
        [SerializeField] private Transform objectiveParent;
        
        private readonly List<ObjectiveToScriptDict> _objectivesToScript = new();
        private readonly List<LevelObjectiveScrObj> _missingObjectives = new();

        private void Awake()
        {
            InitializeGraphics();
        }

        private void OnEnable()
        {
            level.Register(this);
        }

        private void OnDisable()
        {
            level.Unregister(this);
        }

        private void InitializeGraphics()
        {
            _objectivesToScript.Clear();
            _missingObjectives.Clear();
            
            foreach (var objective in level.objectives)
            {
                ObjectivePrefab script =Instantiate(objectivePrefab, objectiveParent).GetComponent<ObjectivePrefab>();
                
                script.Initialize(objective.objectiveDescription);
                
                _objectivesToScript.Add(new ObjectiveToScriptDict(objective, script));
                
                _missingObjectives.Add(objective);
            }
        }

        public void Complete(LevelObjectiveScrObj objective)
        {
            foreach (ObjectiveToScriptDict data in _objectivesToScript)
            {
                if (data.objective == objective)
                {
                    // Set as completed
                    data.script.SetCompleted();

                    _missingObjectives.Remove(data.objective);

                    break;
                }
            }
        }

        public void CheckLevelCompletion()
        {
            if (_missingObjectives.Count == 0)
            {
                // Level can be considered complete
            }
        }
    }

    [Serializable]
    public class ObjectiveToScriptDict
    {
        public LevelObjectiveScrObj objective;
        public ObjectivePrefab script;

        public ObjectiveToScriptDict(LevelObjectiveScrObj objective, ObjectivePrefab script)
        {
            this.objective = objective;
            this.script = script;
        }
    }
}
