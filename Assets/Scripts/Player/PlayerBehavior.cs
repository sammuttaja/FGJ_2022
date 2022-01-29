using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


        private void Awake()
        {
            _playerActions = new PlayerInputActions();

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
        }
    }
}