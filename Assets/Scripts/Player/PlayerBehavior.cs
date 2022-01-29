using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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
        private GameObject pauseContainer;

        [SerializeField]
        private GameObject noButton;

        private PlayerInputActions _playerActions;
        private Rigidbody2D _rbody;
        private Vector2 _moveInput;
        private PlayerMask _mask;
        private bool _withMask;
        private float MaskDrain = 10f;

        private List<Transform> Enemys = new List<Transform>();

        public bool IsMaskOn
        {
            get { return _withMask; }
        }

        private void Awake()
        {
            Enemys = GameObject.FindGameObjectsWithTag("NPC").Select( x => x.transform).ToList();

            _withMask = false;

            _playerActions = new PlayerInputActions();
            _mask = new PlayerMask(100);

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

        private void FixedUpdate()
        {
            // Get the input from WASD / arrows and set the movement.
            _moveInput = _playerActions.Player.Move.ReadValue<Vector2>();
            _rbody.velocity = _moveInput * _speed;

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
            }
            else if (_playerActions.Player.ActivateMask.WasPressedThisFrame() && _withMask == true)
            {
                _withMask = false;
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
                _withMask = false;

            Debug.Log("Is with mask: " + _mask._maskPower);
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