using System.Collections;
using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class LevelSelectorManager : MonoBehaviour
    {
        [Header("Camera movement")]
        [SerializeField] private Transform defaultCameraTransform;
    
        [SerializeField] private List<Transform> cameraTransforms;
        private int _currentLevelIndex;
        
        private Camera _mainCamera;
        private Transform _mainCameraTransform;
        private CameraProjectionBlender _cameraProjectionBlender;

        private bool _inOrthographicView;

        [Header("Level description")] 
        [SerializeField] private GameObject levelDetailsObj;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private RectTransform contentRect;

        private const float YOffset = 50f;

        private const char FullProgressChar = '█';
        private const char EmptyProgressChar = '░';
        
        [SerializeField] private List<LevelDescriptionScrObj> levelsScriptableObjects = new();
        public Level CurrentSelectedLevel => levelsScriptableObjects[_currentLevelIndex] != null ? 
            levelsScriptableObjects[_currentLevelIndex].levelNumber : Level.None;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _currentLevelIndex = 0;
            _inOrthographicView = true;

            if (_mainCamera == null) return;
        
            _mainCameraTransform = _mainCamera.transform;
            _cameraProjectionBlender = _mainCamera.GetComponent<CameraProjectionBlender>();
        }
        
        public void EnterLevelSelection()
        {
            if (!_inOrthographicView) return;
            
            _inOrthographicView = false;
            
            StopAllCoroutines();
            StartCoroutine(SetToPerspective());
        }
        
        public void BackToMainMenu()
        {
            if (_inOrthographicView) return;
            
            _inOrthographicView = true;
            
            StopAllCoroutines();
            StartCoroutine(ResetToDefault());
        }

        public void PreviousLevel()
        {
            if (_inOrthographicView) return;
            
            StopAllCoroutines();
            StartCoroutine(PreviousLevelCor());
        }
        
        public void NextLevel()
        {
            if (_inOrthographicView) return;
            
            StopAllCoroutines();
            StartCoroutine(NextLevelCor());
        }
        
        private IEnumerator PreviousLevelCor()
        {
            levelDetailsObj.SetActive(false);
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            // Cycle backwards in the list
            _currentLevelIndex = _currentLevelIndex - 1 < 0 ? cameraTransforms.Count - 1 : _currentLevelIndex - 1;
            
            yield return StartCoroutine(MoveCameraCoroutine(_mainCameraTransform, cameraTransforms[_currentLevelIndex]));
            
            SetLevelDescription(levelsScriptableObjects[_currentLevelIndex]);
            levelDetailsObj.SetActive(true);
            PlayerInputManager.Instance.SetContext(InputContext.LevelSelection);
        }
        
        private IEnumerator NextLevelCor()
        {
            levelDetailsObj.SetActive(false);
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            _currentLevelIndex = (_currentLevelIndex + 1) % cameraTransforms.Count;
            
            yield return StartCoroutine(MoveCameraCoroutine(_mainCameraTransform, cameraTransforms[_currentLevelIndex]));
            
            SetLevelDescription(levelsScriptableObjects[_currentLevelIndex]);
            levelDetailsObj.SetActive(true);
            PlayerInputManager.Instance.SetContext(InputContext.LevelSelection);
        }

        private void SetLevelDescription(LevelDescriptionScrObj levelScrObj)
        {
            string description = $"<color=green>> hack --target level_{(int)levelScrObj.levelNumber:00}</color>\n\n";
            description += "<b>TARGET_NAME</b>\n";
            description += $"   level_{(int)levelScrObj.levelNumber:00} - {levelScrObj.levelName}\n\n";
            
            description += "<b>DESCRIPTION</b>\n";
            description += $"   {levelScrObj.levelDescription}\n\n";
            
            description += "<b>OBJECTIVES</b>\n";
            foreach (string levelObjective in levelScrObj.levelObjectives)
            {
                description += $"   --{levelObjective}\n";
            }
            description += "\n";
            
            description += "<b>DIFFICULTY</b>\n";
            description += $"   <color=#{ColorFromDifficulty(levelScrObj.difficulty)}>";
            for (int i = 0; i < levelScrObj.difficulty; i++)
            {
                description += FullProgressChar;
            }
            for (int i = levelScrObj.difficulty; i < 10; i++)
            {
                description += EmptyProgressChar;
            }
            description += "</color>\n\n";

            description += "> Press <b>ENTER</b> to play...";
            
            descriptionText.text = description;
            descriptionText.ForceMeshUpdate();
            
            contentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, descriptionText.preferredHeight + YOffset);
        }

        private string ColorFromDifficulty(int difficulty)
        {
            difficulty = Mathf.Clamp(difficulty, 0, 10);

            if (difficulty <= 5)
            {
                byte r = (byte)Mathf.RoundToInt((difficulty / 5f) * 255f);
                return $"{r:X2}FF00";
            }

            byte g = (byte)Mathf.RoundToInt((1f - (difficulty - 5) / 5f) * 255f);
            return $"FF{g:X2}00";
        }
        
        private IEnumerator MoveCameraCoroutine(Transform start, Transform end)
        {
            Vector3 startPos = start.position;
            Vector3 endPos = end.position;

            Quaternion startRot = start.rotation;
            Quaternion endRot = end.rotation;

            float eval = 0f;

            while (eval < 1f)
            {
                Vector3 newPos = Vector3.Slerp(startPos, endPos, eval);
                Quaternion newRot = Quaternion.Slerp(startRot, endRot, eval);
            
                _mainCameraTransform.SetPositionAndRotation(newPos, newRot);
            
                eval += Time.deltaTime;
            
                yield return null;
            }
        }
    
        private IEnumerator SetToPerspective()
        {
            _cameraProjectionBlender.Perspective();

            yield return new WaitForSeconds(1f);
        
            yield return StartCoroutine(MoveCameraCoroutine(_mainCameraTransform, cameraTransforms[0]));

            _currentLevelIndex = 0;
            SetLevelDescription(levelsScriptableObjects[_currentLevelIndex]);
            levelDetailsObj.SetActive(true);
            
            PlayerInputManager.Instance.SetContext(InputContext.LevelSelection);
        }
    
        private IEnumerator ResetToDefault()
        {
            levelDetailsObj.SetActive(false);
            
            yield return StartCoroutine(MoveCameraCoroutine(_mainCameraTransform, defaultCameraTransform));
        
            _cameraProjectionBlender.Orthographic();
            
            PlayerInputManager.Instance.SetContext(InputContext.Disabled);
            
            EventsManager.OnBackToMainMenu?.Invoke();
        }
    }
}
