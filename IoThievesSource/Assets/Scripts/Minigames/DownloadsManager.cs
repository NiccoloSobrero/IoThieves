using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames
{
    public class DownloadsManager : MonoBehaviour
    {
        public static DownloadsManager Instance;
        
        [SerializeField] private GameObject downloadProgressPrefab;
        [SerializeField] private Transform downloadProgressParent;
        
        private readonly List<string> _downloadedFiles = new();

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
        }

        public void AddDownload(string filePath, float time)
        {
            if (_downloadedFiles.Contains(filePath))
            {
                return;
            }
            
            _downloadedFiles.Add(filePath);
            StartCoroutine(DownloadProgressCoroutine(filePath, time));
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
            
            resource.Complete();
        }
    }
}