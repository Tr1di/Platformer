using UnityEngine;
using UnityEngine.Serialization;

namespace Tridi
{
    public class Jumping : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Ground ground;
        
        [Space]
        [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
        [SerializeField, Range(0, 5)   ] private int _maxAirJumps = 0;
        [SerializeField, Range(0f, 5f) ] private float _downwardMovementMultiplier = 3f;
        [SerializeField, Range(0f, 5f) ] private float _upwardMovementMultiplier = 1.7f;
        
        private Vector2 _velocity;
        
        private int _jumpPhase;
        private float _defaultGravityScale = 1f;
        private float _jumpSpeed;

        private bool _pendingJump;
        public bool IsOnGround => ground.IsOnGround;

        public delegate void OnJump();
        public OnJump onJump;

        private void FixedUpdate()
        {
            PerformJump();
        }

        private void PerformJump()
        {
            _velocity = body.velocity;

            if (IsOnGround)
            {
                _jumpPhase = 0;
            }

            if (_pendingJump)
            {
                _pendingJump = false;
                DoJump();
            }

            body.gravityScale = body.velocity.y switch
            {
                > 0 => _upwardMovementMultiplier,
                < 0 => _downwardMovementMultiplier,
                0 => _defaultGravityScale,
                _ => body.gravityScale
            };

            body.velocity = _velocity;
        }

        private void DoJump()
        {
            if (!IsOnGround && _jumpPhase >= _maxAirJumps) return;

            _jumpPhase += 1;

            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);

            switch (_velocity.y)
            {
                case > 0f:
                    _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                    break;
                case < 0f:
                    _jumpSpeed += Mathf.Abs(body.velocity.y);
                    break;
            }

            onJump?.Invoke();
            
            _velocity.y += _jumpSpeed;
        }

        public void Jump()
        {
            if (_pendingJump) return;
            _pendingJump = true;
        }
    }
}
