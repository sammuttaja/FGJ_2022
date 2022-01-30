using UnityEngine;

namespace FGJ_2022.Audio
{
    [AddComponentMenu("FGJ_2022/Music/ Ambient Sound")]
    public class AmbientSound : MonoBehaviour
    {
        public AudioClip ambient;

        private AudioSource audioSrc;

        // Start is called before the first frame update
        void Start()
        {
            audioSrc = GetComponent<AudioSource>();
            PlayAudio(ambient);
        }

        /// <summary>
        /// Set the audio.
        /// </summary>
        /// <param name="audio"></param>
        public void PlayAudio(AudioClip audio)
        {
            audioSrc.loop = true;
            audioSrc.clip = audio;
            audioSrc.volume = 0.2f;
            audioSrc.Play();
        }
    }
}
