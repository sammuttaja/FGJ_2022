using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FGJ_2022.NPC
{
    public class PrizeTrigger : MonoBehaviour
    {
        Transform Player;
        public GameObject WinningUI;

        [SerializeField]
        private GameObject quitButton;

        [SerializeField]
        private AudioClip fanfare;

        private AudioSource audioSrc;
        private bool hasPlayed;

        private void Start()
        {
            EventSystem.current.SetSelectedGameObject(quitButton);
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            audioSrc = GetComponent<AudioSource>();

            hasPlayed = false;
        }

        private void Update()
        {
            if (Vector2.Distance(Player.transform.position, new Vector2(transform.position.x, transform.position.y)) <= 1f)
            {
                if (hasPlayed == false)
                {
                    audioSrc.PlayOneShot(fanfare);
                    hasPlayed = true;
                }
                WinningUI.SetActive(true);

                NPC.NPCBehavior[] sheeps = GameObject.FindGameObjectsWithTag("NPC").Select(x => x.GetComponent<NPC.NPCBehavior>()).ToArray();
                foreach (NPC.NPCBehavior sheep in sheeps)
                {
                    sheep.StopFollowing();
                }
            }
        }

      
    }
}