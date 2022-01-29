using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGJ_2022.NPC
{
    [AddComponentMenu("FGJ_2022/NPC NPC behavior")]
    public class NPCBehavior : MonoBehaviour
    {
        //TODO Create NPC behavior!

        public float speed = 4;
        public float movementRadius = 2;

        private Vector3 InitialPos;
        private bool IsFollowing = false;
        private bool IsMoving = false;
        private float idleTime = 0;
        private Vector2 newPos;

        private void Start()
        {
            InitialPos = transform.position;
            idleTime = Random.Range(5, 10);
        }

        private void Update()
        {
            if (!IsFollowing)
            {
                if(idleTime <= 0 && !IsMoving)
                {
                    newPos = new Vector2(Random.Range(-movementRadius, movementRadius), Random.Range(-movementRadius, movementRadius)) + new Vector2(InitialPos.x, InitialPos.y);
                    idleTime = Random.Range(7, 13);
                    IsMoving = true;
                }
                else if(Vector2.Distance(transform.position, newPos) >= 0.4f && IsMoving)
                {
                    Vector2 dir = (newPos - new Vector2(transform.position.x, transform.position.y)).normalized;
                    transform.position = new Vector2(transform.position.x, transform.position.y) + dir * speed * Time.deltaTime;
                }else if(Vector2.Distance(transform.position, newPos) <= 0.4f)
                {
                    IsMoving = false;
                }
               
                idleTime -= Time.deltaTime;
            }
            else
            {

            }
        }

       
    }
}
