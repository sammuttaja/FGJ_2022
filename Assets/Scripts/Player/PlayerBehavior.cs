using UnityEngine;

namespace FGJ_2022.Player
{
    /// <summary>
    /// Player behavior script.
    /// </summary>
    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
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
            _moveInput = _playerActions.Player.Move.ReadValue<Vector2>();
            _rbody.velocity = _moveInput * _speed;
        }
    }
}