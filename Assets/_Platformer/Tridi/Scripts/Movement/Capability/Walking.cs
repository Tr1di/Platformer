using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tridi
{
    public class Walking : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Ground ground;
        
        [Space]
        [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

        private Vector2 _direction;
        private Vector2 _targetVelocity;
        private Vector2 _velocity;
        
        public bool IsOnGround => ground.IsOnGround;
        public Vector2 Velocity => _velocity;
        public float Speed  => _velocity.magnitude;

        private void Start()
        {
            _direction = new Vector2();
            _targetVelocity = new Vector2();
            _velocity = new Vector2();
        }

        private void FixedUpdate()
        {
            CalculateTargetVelocity();
            PerformMove();
        }

        private Vector2 MoveAlongSurface(Vector2 forward)
        {
            return forward - Vector2.Dot(forward, ground.Normal) * ground.Normal;
        }

        private void CalculateTargetVelocity()
        {
            _targetVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - ground.Friction, 0f);
        }

        private void PerformMove()
        {
            _velocity = body.velocity;

            var acceleration = IsOnGround ? _maxAcceleration : _maxAirAcceleration;
            var maxSpeedChange = acceleration * Time.deltaTime;

            var alongSurface = MoveAlongSurface(_targetVelocity);
            _velocity.x = Mathf.MoveTowards(_velocity.x, alongSurface.x, maxSpeedChange);

            if (IsOnGround)
            {
                _velocity.y = Mathf.MoveTowards(_velocity.y, alongSurface.y, maxSpeedChange);
            }

            body.velocity = _velocity;
            _direction = Vector2.zero;
        }

        public void Move(Vector2 direction)
        {
            _direction += direction;
        }

        private void OnDrawGizmos()
        {
            if (!ground || !body) return;

            var pos = body.position;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + _velocity.normalized * 3.0f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, pos + ground.Normal * 3.0f);
        }
    }
}
