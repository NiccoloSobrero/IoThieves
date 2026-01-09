using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class AudioPlayerUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text songTitleText;
        [SerializeField] private Image coverImage;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text timeLeftText;
        [SerializeField] private Slider progressSlider;

        [SerializeField] private CanvasGroup volumePopUpCanvasGroup;
        [SerializeField] private Transform volumeBoxesParent;
        
        [SerializeField] private Sprite genericCoverSprite;
        
        [SerializeField] private float volumePopUpDuration = 2f;
        
        private Coroutine _volumePopUpCoroutine;

        public int MaxVolume => volumeBoxesParent.childCount;

        private void Awake()
        {
            volumePopUpCanvasGroup.alpha = 0f;
        }

        public void UpdateSong(string songName, Sprite cover)
        {
            songTitleText.text = songName;
            coverImage.sprite = cover;
        }

        public void UpdateProgress(float progress, float currentTime, float length)
        {
            progressSlider.value = progress;

            timeText.text = AudioManager.FormatTime(currentTime);
            timeLeftText.text = "-" + AudioManager.FormatTime(length - currentTime);
        }
        
        public void UpdateVolume(int currentVolume)
        {
            for (int i = 0; i < volumeBoxesParent.childCount; i++)
            {
                Transform box = volumeBoxesParent.GetChild(i);
                box.gameObject.SetActive(i < currentVolume);
            }

            if (_volumePopUpCoroutine != null)
            {
                StopCoroutine(_volumePopUpCoroutine);
            }
            _volumePopUpCoroutine = StartCoroutine(ShowAndHideVolumePopUp());
        }
        
        private IEnumerator ShowAndHideVolumePopUp()
        {
            volumePopUpCanvasGroup.alpha = 1f;
            yield return new WaitForSeconds(2f);

            while (volumePopUpCanvasGroup.alpha > 0f)
            {
                volumePopUpCanvasGroup.alpha -= Time.deltaTime * 2;
                yield return null;
            }
            
            volumePopUpCanvasGroup.alpha = 0f;
        }
    }
}
