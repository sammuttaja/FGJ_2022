using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FGJ_2022.NPC
{
    public class PrizeTrigger : MonoBehaviour
    {
        Transform Player;
        public GameObject WinningUI;

        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            if (Vector2.Distance(Player.transform.position, new Vector2(transform.position.x, transform.position.y)) <= 1f)
            {
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