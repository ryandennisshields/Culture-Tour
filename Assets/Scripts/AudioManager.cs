using com.cyborgAssets.inspectorButtonPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GCU.CultureTour
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("Audio Manager");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<AudioManager>();
                }

                return _instance;
            }
        }

        private static AudioManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;

                return;
            }

            Destroy(gameObject);
        }

        [SerializeField]
        private AudioClip[] soundEffectClips;

        public AudioClip[] SoundEffect => soundEffectClips;

        [SerializeField]
        private AudioClip[] musicClips;

        public AudioClip[] Music => soundEffectClips;

        /// <summary>
        /// Written by Chat GPT 2024-04-07.
        /// </summary>
        private AudioSource musicSource;
        private AudioSource soundEffectSource;

        [ProButton]
        public void PlaySoundEffect(int index)
        {
            if (soundEffectSource == null)
            {
                soundEffectSource = gameObject.AddComponent<AudioSource>();
            }

            if (index >= 0 && index < soundEffectClips.Length)
            {
                soundEffectSource.clip = soundEffectClips[index];
                soundEffectSource.PlayOneShot(soundEffectClips[index]);
            }
        }

        [ProButton]
        public void PlayMusic(int index)
        {
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
            }

            if (index >= 0 && index < musicClips.Length)
            {
                musicSource.clip = musicClips[index];
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        [ProButton]
        public void StopSoundEffects()
        {
            if (soundEffectSource != null && soundEffectSource.isPlaying)
            {
                soundEffectSource.Stop();
            }
        }

        [ProButton]
        public void StopMusic()
        {
            if (musicSource != null && musicSource.isPlaying)
            {
                musicSource.Stop();
            }
        }
    }
}