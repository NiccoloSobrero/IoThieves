using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames
{
    public class DownloadsManager : MonoBehaviour
    {
        public static DownloadsManager Instance;
        private static readonly int IsHidden = Animator.StringToHash("isHidden");

        [SerializeField] private GameObject downloadProgressPrefab;
        [SerializeField] private Transform downloadProgressParent;
        
        private GridLayoutGroup _downloadProgressGrid;
            
        [SerializeField] private Animator downloadSectionAnimator;
        private bool _isSectionHidden;
        
        private readonly List<string> _downloadedFiles = new();
        public List<string> DownloadedFiles => _downloadedFiles;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _isSectionHidden = true;
            downloadSectionAnimator.SetBool(IsHidden, _isSectionHidden);
            
            _downloadProgressGrid = downloadProgressParent.GetComponent<GridLayoutGroup>();
        }

        public void AddDownload(string filePath, float time)
        {
            if (_downloadedFiles.Contains(filePath))
            {
                return;
            }
            
            _isSectionHidden = false;
            
            downloadSectionAnimator.SetBool(IsHidden, _isSectionHidden);
            
            StartCoroutine(DownloadProgressCoroutine(filePath, time));
            
            UpdateContentHeight();
        }

        private IEnumerator DownloadProgressCoroutine(string filePath, float time)
        {
            float timer = 0f;
            DownloadingResource resource = Instantiate(downloadProgressPrefab, downloadProgressParent).GetComponent<DownloadingResource>();
            
            resource.Init(filePath);

            while (timer < time)
            {
                timer += Time.deltaTime;
                
                resource.UpdateProgress(timer / time);
                
                yield return null;
            }
            
            _downloadedFiles.Add(filePath);
            
            resource.Complete();
        }

        public void ShowHide()
        {
            _isSectionHidden = !_isSectionHidden;
            
            downloadSectionAnimator.SetBool(IsHidden, _isSectionHidden);
        }

        private void UpdateContentHeight()
        {
            RectTransform rect = downloadProgressParent.GetComponent<RectTransform>();
            
            float newHeight = _downloadProgressGrid.cellSize.y * downloadProgressParent.childCount +
                              _downloadProgressGrid.spacing.y * (downloadProgressParent.childCount - 1) +
                              _downloadProgressGrid.padding.bottom + 
                              _downloadProgressGrid.padding.top;
            
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
        }
    }
}