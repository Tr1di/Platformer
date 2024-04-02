using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tridi
{
    public class Ground : MonoBehaviour
    {
        [Serializable]
        private struct Events
        {
            [SerializeField] public UnityEvent OnLanded;
            [SerializeField] public UnityEvent OnLeave;
        }
        
        [SerializeField, Range(0f, 90f)] private float walkableAngle = 40f;

        private PhysicsMaterial2D _material;
        
        private bool _isOnGround;
        
        private float WalkableY => Mathf.Cos(walkableAngle * Mathf.Deg2Rad);
        
        public bool IsOnGround 
        { 
            get => _isOnGround;
            private set
            {
                switch (value)
                {
                    case false when _isOnGround:
                        onLeave?.Invoke();
                        events.OnLeave?.Invoke();
                        break;
                    case true when !_isOnGround:
                        onLanded?.Invoke();
                        events.OnLanded?.Invoke();
                        break;
                }

                _isOnGround = value;
            } 
        }
        
        public float Friction { get; private set; }

        public Vector2 Normal { get; private set; }

        [SerializeField, Space] private Events events;
        
        public delegate void OnLanded();
        public event OnLanded onLanded;

        public delegate void OnLeave();
        public event OnLeave onLeave;
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            IsOnGround = false;
            Friction = 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        private void EvaluateCollision(Collision2D collision)
        {
            foreach (var contact in collision.contacts)
            {
                var isWalkable = IsWalkable(contact.normal);
                IsOnGround |= isWalkable;
                
                if (isWalkable)
                {
                    Normal = contact.normal;
                }
            }
        }

        private void RetrieveFriction(Collision2D collision)
        {
            _material = collision.collider.sharedMaterial;

            Friction = 0;

            if (_material)
            {
                Friction = _material.friction;
            }
        }

        private bool IsWalkable(Vector2 normal)
        {
            return normal.y >= WalkableY;
        }
    }
}