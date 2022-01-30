using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using FGJ_2022.Player;
using System.Collections;

namespace FGJ_2022.NPC
{
    [AddComponentMenu("FGJ_2022/NPC/ NPC behavior")]
    public class NPCBehavior : MonoBehaviour
    {
        //TODO Create NPC behavior!
        [Header("NPC Properties")]
        public float Speed = 4;
        public float MovementRadius = 2;
        public float ViewRadius = 3;
        public float caughtMinDistance = 0.4f;
        public Transform Player;
        public NPCEnums Behavior;
        public GameObject GameoverUI;
        [Header("Patroll list")]
        public List<Transform> PatrolPattern = new List<Transform>();

        private Vector3 InitialPos;
        private bool IsFollowing = false;
        private bool IsMoving = false;
        bool isCaught = false;
        private Animator animator;

        [SerializeField]
        private float idleTime = 0;
        private Vector2 newPos;

        [SerializeField]
        private GameObject gameOverButton;

        [Header("NPC sounds")]
        public AudioClip angrySheep;
        public AudioClip[] SheepSounds;
        private AudioSource audioSrc;
        private bool hasPlayed;

        private List<Vector2> PatrollPatterPoints = new List<Vector2>();

        private void Start()
        {
            animator = GetComponent<Animator>();
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            playerData = Player.gameObject.GetComponent<PlayerBehavior>();
            InitialPos = transform.position;
            idleTime = Random.Range(5, 10);
            audioSrc = GetComponent<AudioSource>();
            hasPlayed = false;

            if(PatrolPattern.Count > 0)
            {
                for( int i = 0; i < PatrolPattern.Count; i++)
                {
                    PatrollPatterPoints.Add(new Vector2(PatrolPattern[i].position.x, PatrolPattern[i].position.y));
                }
            }

            StartCoroutine(doVoice());
        }

        PlayerBehavior playerData;

        private void Update()
        {
            if (isCaught)
                return;
            if(Vector2.Distance(transform.position, new Vector2(Player.position.x, Player.position.y)) <= ViewRadius && !playerData.IsMaskOn)
            {
                IsFollowing = true;
            }
            if (IsFollowing)
            {
                if(Vector3.Distance(transform.position, Player.position) > ViewRadius)
                {
                    IsFollowing = false;
                    animator.SetFloat("Speed", 0);
                }
            }

            if (IsFollowing == true && hasPlayed == false)
            {
                audioSrc.PlayOneShot(angrySheep);
                hasPlayed = true;
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
                hasPlayed = false;
            }
            else
            {
                Vector2 PlayerPos = new Vector2(Player.position.x, Player.position.y);
                Vector2 NpcPosition = new Vector2(transform.position.x, transform.position.y);
                Vector2 PlayerDir = (PlayerPos - NpcPosition).normalized;
                Vector2 speed = NpcPosition + PlayerDir * Speed * Time.deltaTime;
                transform.position = speed;
                Animate(PlayerDir, speed.magnitude);
                if (Vector2.Distance(PlayerPos, NpcPosition) < caughtMinDistance)
                {
                    playerData.IsCaught = true;
                    //TODO Add game over
                    if (GameoverUI != null)
                        GameoverUI.SetActive(true);
                    StopFollowing();
                    NPCBehavior[] otherSheeps = GameObject.FindGameObjectsWithTag("NPC").Select(x => x.GetComponent<NPCBehavior>()).ToArray(); ;
                    EventSystem.current.SetSelectedGameObject(gameOverButton);
                    foreach (NPCBehavior sheep in otherSheeps)
                    {
                        sheep.StopFollowing();
                    }
                }
            }
        }
        int TargetIndex = 0;

        private void PatrollBehavior()
        {
            if (Vector2.Distance(transform.position, PatrollPatterPoints[TargetIndex]) >= 0.2f)
            {
                Vector2 dir = (PatrollPatterPoints[TargetIndex] - new Vector2(transform.position.x, transform.position.y)).normalized;
                Vector2 speed = new Vector2(transform.position.x, transform.position.y) + dir * Speed * Time.deltaTime;
                transform.position = speed;
                Animate(dir, speed.magnitude);
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
                Vector2 speed = new Vector2(transform.position.x, transform.position.y) + dir * Speed * Time.deltaTime;
                transform.position = speed;
                Animate(dir, speed.magnitude);
            }
            else if (Vector2.Distance(transform.position, newPos) <= 0.4f)
            {
                IsMoving = false;
            }
        }

        Vector2 lastDirecton;
        void Animate(Vector2 direction, float speed)
        {

            if (direction != Vector2.zero)
                lastDirecton = direction;
            animator.SetFloat("_speed", speed);
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);

            animator.SetFloat("LastX", lastDirecton.x);
            animator.SetFloat("LastY", lastDirecton.y);
        }

        public void StopFollowing()
        {
            isCaught = true;
            IsFollowing = false;
        }

        private IEnumerator doVoice()
        {
            int length = SheepSounds.Length;
            int soundIndex = Random.Range(0, length);

            while (true)
            {
                yield return new WaitForSeconds(Random.Range(4, 20));
                audioSrc.PlayOneShot(SheepSounds[soundIndex]);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, ViewRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, caughtMinDistance);
        }

        private void OnDisable()
        {
            StopCoroutine(doVoice());
        }

    }
}
