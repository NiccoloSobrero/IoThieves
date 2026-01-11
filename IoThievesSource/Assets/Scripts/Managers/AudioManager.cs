using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    [Serializable]
    public class SongData
    {
        public string songName;
        public Sprite cover;
        public AudioClip clip;

        public SongData(string songName, Sprite cover, AudioClip clip)
        {
            this.songName = songName;
            this.cover = cover;
            this.clip = clip;
        }
    }
    
    public class AudioManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioPlayerUI audioPlayerUI;
        
        [Header("Setup")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private List<SongData> playlist = new();

        [Header("Behavior")]
        [SerializeField] private bool playOnStart = true;
        [SerializeField] private bool loopSingle;

        [Header("UI Update")]
        [SerializeField] private float uiUpdateInterval = 0.3f;

        private int _currentIndex = -1;

        private bool IsPlaying => musicSource != null && musicSource.isPlaying;

        private float Progress01 => Length <= 0.01f ? 0f : Mathf.Clamp01(CurrentTime / Length);
        private float CurrentTime => (musicSource != null) ? musicSource.time : 0f;
        private float Length => (musicSource != null && musicSource.clip != null) ? musicSource.clip.length : 0f;

        private float _uiUpdateTimer;

        private int _currentVolume = 5;

        private void Reset()
        {
            musicSource = GetComponent<AudioSource>();
            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
        }

        private void Awake()
        {
            if (musicSource == null)
            {
                musicSource = GetComponent<AudioSource>();
                if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            }

            musicSource.playOnAwake = false;
        }

        private void Start()
        {
            if (playlist.Count > 0)
                SetSong(0, playOnStart);
        }

        private void Update()
        {
            if (musicSource == null) return;

            // Auto-advance when song ends (unless looping single)
            if (!loopSingle && musicSource.clip != null && !musicSource.isPlaying)
            {
                if (musicSource.time >= Mathf.Max(0f, musicSource.clip.length - 0.02f))
                {
                    Next();
                    return;
                }
            }

            // UI progress tick
            _uiUpdateTimer += Time.deltaTime;
            
            if (_uiUpdateTimer >= uiUpdateInterval)
            {
                _uiUpdateTimer = uiUpdateInterval;
                audioPlayerUI.UpdateProgress(Progress01, CurrentTime, Length);
            }
        }

        // ---------- Public controls ----------

        public void Play()
        {
            if (musicSource == null) return;
            if (musicSource.clip == null) return;

            musicSource.Play();
            musicSource.volume = Mathf.Clamp01(_currentVolume / (float)audioPlayerUI.MaxVolume);
        }

        public void Pause()
        {
            if (musicSource == null) return;

            musicSource.Pause();
            audioPlayerUI.UpdateProgress(Progress01, CurrentTime, Length);
        }

        public void TogglePlayPause()
        {
            if (IsPlaying) Pause();
            else Play();
        }

        public void Next()
        {
            if (playlist.Count == 0) return;
            
            int next = (_currentIndex + 1) % playlist.Count;
            SetSong(next, true);
        }

        public void Prev()
        {
            if (playlist.Count == 0) return;
            
            int prev = (_currentIndex - 1 + playlist.Count) % playlist.Count;
            SetSong(prev, true);
        }

        private void SetSong(int index, bool startPlaying)
        {
            if (index < 0 || index >= playlist.Count) return;

            _currentIndex = index;
            var song = playlist[index];

            musicSource.clip = song.clip;
            musicSource.time = 0f;
            musicSource.loop = loopSingle;

            audioPlayerUI.UpdateSong(song.songName, song.cover);
            audioPlayerUI.UpdateProgress(0f, 0f, Length);

            if (startPlaying && song.clip != null)
            {
                musicSource.Play();
            }
        }

        public void IncreaseVolume()
        {
            if (_currentVolume + 1 > audioPlayerUI.MaxVolume) return;
            
            _currentVolume++;
            SetVolume(_currentVolume / (float)audioPlayerUI.MaxVolume);
        }

        public void DecreaseVolume()
        {
            if (_currentVolume - 1 < 0) return;
            
            _currentVolume--;
            SetVolume(_currentVolume / (float)audioPlayerUI.MaxVolume);
        }

        private void SetVolume(float volume01)
        {
            if (musicSource == null) return;
            
            audioPlayerUI.UpdateVolume(_currentVolume);
            musicSource.volume = Mathf.Clamp01(volume01);
        }

        // ---------- Helpers for UI formatting ----------

        public static string FormatTime(float seconds)
        {
            seconds = Mathf.Max(0f, seconds);
            int total = Mathf.FloorToInt(seconds);
            int min = total / 60;
            int sec = total % 60;
            
            return $"{min:00}:{sec:00}";
        }
    }
}
