using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using FGJ_2022.Audio;
using TMPro;

namespace FGJ_2022.Player
{
    /// <summary>
    /// Player behavior script.
    /// </summary>
    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private int _maskPower;

        [SerializeField]
        private GameObject pauseContainer;

        [SerializeField]
        private GameObject noButton;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private TextMeshProUGUI powerText;

        [SerializeField]
        private AudioClip whooshLoud;

        [SerializeField]
        private AudioClip whoosh;

        private PlayerInputActions _playerActions;
        private Rigidbody2D _rbody;
        private Vector2 _moveInput;
        private PlayerMask _mask;
        private bool _withMask;
        private MusicManager manager;
        private AudioSource audioSrc;
        [SerializeField]
        private AudioSource MaskEfectSrc;

        public bool IsCaught { get; set; }

        private List<Transform> Enemys = new List<Transform>();

        public bool IsMaskOn
        {
            get { return _withMask; }
        }

        private void Start()
        {
            manager = GetComponent<MusicManager>();
            audioSrc = GetComponent<AudioSource>();
        }

        private void Awake()
        {
            Enemys = GameObject.FindGameObjectsWithTag("NPC").Select( x => x.transform).ToList();

            if (animator == null)
                animator = GetComponent<Animator>();

            _withMask = false;

            _playerActions = new PlayerInputActions();
            _mask = new PlayerMask(_maskPower);

            _rbody = GetComponent<Rigidbody2D>();
            if (_rbody == null)
                Debug.LogError("RigidBody2D not found!");
        }

        private void OnEnable()
        {
            _playerActions.Player.Enable();
        }

        private void OnDisable()
        {
            _playerActions.Player.Disable();
        }

        Vector2 lastDirecton;

        private void FixedUpdate()
        {
            if (IsCaught)
            {
                _rbody.velocity = Vector2.zero;
                animator.SetFloat("_speed", 0);
                return;
            }
            // Get the input from WASD / arrows and set the movement.
            _moveInput = _playerActions.Player.Move.ReadValue<Vector2>();
            _rbody.velocity = _moveInput * _speed;
            if(_moveInput != Vector2.zero)
                lastDirecton = _moveInput;
            animator.SetFloat("_speed", _rbody.velocity.magnitude);
            animator.SetFloat("Horizontal", _moveInput.x);
            animator.SetFloat("Vertical", _moveInput.y);

            animator.SetFloat("LastX", lastDirecton.x);
            animator.SetFloat("LastY", lastDirecton.y);

            // Activate the quit UI.
            if (_playerActions.Player.Exit.WasPressedThisFrame())
            {
                if (pauseContainer.activeSelf == false)
                {
                    pauseContainer.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(noButton);
                }
                else
                {
                    pauseContainer.SetActive(false);
                }
            }

            // Activate the mask.
            if (_playerActions.Player.ActivateMask.WasPressedThisFrame() && _withMask == false)
            {
                _withMask = true;
                animator.SetBool("MaskOn", true);
                MaskEfectSrc.PlayOneShot(whooshLoud);
            }
            else if (_playerActions.Player.ActivateMask.WasPressedThisFrame() && _withMask == true)
            {
                _withMask = false;
                animator.SetBool("MaskOn", false);
                MaskEfectSrc.PlayOneShot(whoosh);
            }

            if (_withMask && _mask._maskPower > 0)
            {
                if(IsEnemyClose(out float distance))
                    _mask._maskPower -= (10f + Mathf.Lerp(1, 3, distance / 3)) * Time.deltaTime;
                _mask._maskPower -= 10f * Time.deltaTime;
            }
            else if (!_withMask && (_mask._maskPower < _mask._maxMaskPower))
                _mask._maskPower += 10f * Time.deltaTime;

            if (_mask._maskPower <= 0)
            {
                _withMask = false;
                animator.SetBool("MaskOn", false);
                MaskEfectSrc.PlayOneShot(whoosh);
            }
            if (_withMask && !manager.hiipiPlaying)
            {
                manager.PlayAudio(manager.walkbassoloop);
                manager.hiipiPlaying = true;
                manager.walkPlaying = false;
            }
            else
            {
                if (!manager.walkPlaying && !_withMask)
                {
                    manager.PlayAudio(manager.hiipbassoloop);
                    manager.walkPlaying = true;
                    manager.hiipiPlaying = false;
                }
            }

            // Update the UI in regards of power level.
            powerText.SetText("Mask power: " + _mask._maskPower);
        }


        private bool IsEnemyClose(out float distancetoEnemy)
        {
            Vector2 currentPos = new Vector2(transform.position.x, transform.position.y);
            for(int i = 0; i < Enemys.Count; i++)
            {
                float distance = Vector2.Distance(currentPos, new Vector2(Enemys[i].position.x, Enemys[i].position.y));
                if ( distance <= 3)
                {
                    distancetoEnemy = distance;
                    return true;
                }
            }
            distancetoEnemy = float.PositiveInfinity;
            return false;
        }
    }
}