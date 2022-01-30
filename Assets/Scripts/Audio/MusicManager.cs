using UnityEngine;
using System;

namespace FGJ_2022.Audio
{
    [AddComponentMenu("FGJ_2022/Music Music Manager")]
    public class MusicManager : MonoBehaviour
    {
        public bool isMasked { set; get; }
        public AudioClip walkbassoloop;
        public AudioClip hiipbassoloop;
        public bool walkPlaying = false;
        public bool hiipiPlaying = false;
        [SerializeField]
        private AudioSource audioSrc;

        private void Awake()
        {
            audioSrc = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Set the audio.
        /// </summary>
        /// <param name="audio"></param>
        public void PlayAudio(AudioClip audio)
        {
            audioSrc.Stop();

            try
            {
                audioSrc.loop = true;
                audioSrc.clip = audio;
                audioSrc.volume = 0.2f;
                audioSrc.Play();
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError("Fatal error: " + e.ToString());
# endif
            }
        }
    }
}
