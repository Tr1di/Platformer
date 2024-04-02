using System;
using UnityEngine;

namespace Tridi
{
    public class Movement : MonoBehaviour
    {
        private static readonly Vector3 ForwardRotation = new(0f, 0f, 0f);
        private static readonly Vector3 BackwardRotation = new(0f, 180f, 0f);
        
        [SerializeField] private Ground ground;
        
        [Space]
        [SerializeField] private Walking walking;
        [SerializeField] private Jumping jumping;

        [Space] 
        [SerializeField] private CharacterAnimator animator;

        private void OnEnable()
        {
            ground.onLeave += OnJump;
            ground.onLeave += OnLand;
        }
        
        private void OnDisable()
        {
            ground.onLeave -= OnJump;
            ground.onLeave -= OnLand;
        }

        private void OnJump()
        {
            animator.Jump();
        }
        
        private void OnLand()
        {
            animator.SetIsFalling(false);
        }

        private void FixedUpdate()
        {
            animator.SetVelocity(GetVelocity());
            animator.SetIsFalling(!ground.IsOnGround && GetVelocity().y <= 0);
        }

        public void Move(Vector2 direction)
        {
            walking.Move(direction);
            
            var character = transform;
            character.eulerAngles = GetVelocity().x switch
            {
                > 0 => ForwardRotation,
                < 0 => BackwardRotation,
                _ => character.eulerAngles
            };
        }

        public void DoJump()
        {
            jumping.Jump();
            animator.SetIsFalling(false);
            animator.Jump();
        }
        
        public Vector2 GetVelocity()
        {
            return walking.Velocity;
        }
        
        public float GetSpeed()
        {
            return walking.Speed;
        }

        public bool IsFalling()
        {
            return !ground.IsOnGround;
        }
    }
}
