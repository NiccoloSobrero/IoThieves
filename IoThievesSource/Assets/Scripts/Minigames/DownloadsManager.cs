using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Managers;
using UnityEngine;
using UnityEngine.Events;
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

        public DownloadedFile DownloadedFiles { get; } = new();

        #region Events

        [HideInInspector] public UnityEvent onDownloadTargetFile;

        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _isSectionHidden = true;
            downloadSectionAnimator.SetBool(IsHidden, _isSectionHidden);
            
            _downloadProgressGrid = downloadProgressParent.GetComponent<GridLayoutGroup>();
        }

        public void AddDownload(FileDetails fileDetails, float time)
        {
            if (DownloadedFiles.Contains(fileDetails))
            {
                return;
            }
            
            _isSectionHidden = false;
            
            downloadSectionAnimator.SetBool(IsHidden, _isSectionHidden);
            
            DownloadedFiles.Add(fileDetails);
            
            StartCoroutine(DownloadProgressCoroutine(fileDetails.filePath, time, fileDetails.isTarget));
            
            UpdateContentHeight();
        }

        private IEnumerator DownloadProgressCoroutine(string filePath, float time, bool isTarget)
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
            
            resource.Complete();

            if (isTarget)
            {
                onDownloadTargetFile?.Invoke();
            }
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
    
    [Serializable]
    public record FileDetails
    {
        public Level levelOfDownload;
        public string filePath;
        public Sprite documentSprite;
        public bool isTarget;
        
        public string FileName => Path.GetFileName(filePath);
    
        public FileDetails(Level levelOfDownload, string filePath, Sprite documentSprite, bool isTarget)
        {
            this.levelOfDownload = levelOfDownload;
            this.filePath = filePath;
            this.documentSprite = documentSprite;
            this.isTarget = isTarget;
        }
    }

    [Serializable]
    public class DownloadedFile : List<FileDetails>
    {
        public int GetTargetFilesNumFromLevel(Level level)
        {
            int res = 0;
            
            foreach (var file in AsReadOnly())
            {
                if (file.levelOfDownload == level && file.isTarget)
                {
                    res++;
                }
            }

            return res;
        }

        public void PrintList()
        {
            foreach (var file in AsReadOnly())
            {
                Debug.Log(file.levelOfDownload + ": " + file.filePath);
            }
        }
    }
}