using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGJ_2022.NPC
{
    [AddComponentMenu("FGJ_2022/NPC NPC behavior")]
    public class NPCBehavior : MonoBehaviour
    {
        //TODO Create NPC behavior!

        public float Speed = 4;
        public float MovementRadius = 2;
        public float ViewRadius = 3;
        public float caughtMinDistance = 0.4f;
        public Transform Player;
        public NPCEnums Behavior;
        public List<Transform> PatrolPattern = new List<Transform>();

        private Vector3 InitialPos;
        private bool IsFollowing = false;
        private bool IsMoving = false;
        [SerializeField]
        private float idleTime = 0;
        private Vector2 newPos;

        private List<Vector2> PatrollPatterPoints = new List<Vector2>();

        private void Start()
        {
            InitialPos = transform.position;
            idleTime = Random.Range(5, 10);

            if(PatrolPattern.Count > 0)
            {
                for( int i = 0; i < PatrolPattern.Count; i++)
                {
                    PatrollPatterPoints.Add(new Vector2(PatrolPattern[i].position.x, PatrolPattern[i].position.y));
                }
            }
        }

        private void Update()
        {

            if(Vector2.Distance(transform.position, new Vector2(Player.position.x, Player.position.y)) <= ViewRadius)
            {
                IsFollowing = true;
            }

            if (!IsFollowing)
            {
                switch (Behavior)
                {
                    case NPCEnums.Stay:

                        break;

                    case NPCEnums.InsideCircle:
                        {
                            CircularPatroll();
                            break;
                        }

                    case NPCEnums.Patrol:
                        {
                            PatrollBehavior();
                            break;
                        }
                }
                if(Behavior != NPCEnums.Stay)
                    idleTime -= Time.deltaTime;
            }
            else
            {
                Vector2 PlayerPos = new Vector2(Player.position.x, Player.position.y);
                Vector2 NpcPosition = new Vector2(transform.position.x, transform.position.y);
                Vector2 PlayerDir = (PlayerPos - NpcPosition).normalized;
                transform.position = NpcPosition + PlayerDir * Speed * Time.deltaTime;
                if(Vector2.Distance(PlayerPos, NpcPosition) < caughtMinDistance)
                {
                    //TODO Add game over
                }


            }
        }
        int TargetIndex = 0;

        private void PatrollBehavior()
        {
            if (Vector2.Distance(transform.position, PatrollPatterPoints[TargetIndex]) >= 0.2f)
            {
                Vector2 dir = (PatrollPatterPoints[TargetIndex] - new Vector2(transform.position.x, transform.position.y)).normalized;
                transform.position = new Vector2(transform.position.x, transform.position.y) + dir * Speed * Time.deltaTime;
            }
            else if (Vector2.Distance(transform.position, PatrollPatterPoints[TargetIndex]) <= 0.4f)
            {
                TargetIndex++;
                if (TargetIndex >= PatrollPatterPoints.Count)
                    TargetIndex = 0;
            }
        }

        private void CircularPatroll()
        {
            if (idleTime <= 0 && !IsMoving)
            {
                newPos = new Vector2(Random.Range(-MovementRadius, MovementRadius), Random.Range(-MovementRadius, MovementRadius)) + new Vector2(InitialPos.x, InitialPos.y);
                idleTime = Random.Range(7, 13);
                IsMoving = true;
            }
            else if (Vector2.Distance(transform.position, newPos) >= 0.4f && IsMoving)
            {
                Vector2 dir = (newPos - new Vector2(transform.position.x, transform.position.y)).normalized;
                transform.position = new Vector2(transform.position.x, transform.position.y) + dir * Speed * Time.deltaTime;
            }
            else if (Vector2.Distance(transform.position, newPos) <= 0.4f)
            {
                IsMoving = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, ViewRadius);
        }

    }
}
