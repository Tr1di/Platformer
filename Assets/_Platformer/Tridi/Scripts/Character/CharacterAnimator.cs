using System;
using UnityEngine;

namespace Tridi
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        private Animator _animator;
        
        [SerializeField] private string speedParameterName = "Speed";
        [SerializeField] private string jumpTriggerName = "Jump";
        [SerializeField] private string isFallingParameterName 
            = "IsFalling";
        [SerializeField] private string hurtTriggerName = "Hurt";
        
        [SerializeField] private string deathTriggerName = "Death";
        [SerializeField] private string attackTriggerName = "Attack";
        [SerializeField] private string attackCounterParameterName = "AttackCounter";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetVelocity(Vector2 velocity)
        {
            _animator.SetFloat(speedParameterName, velocity.magnitude);
        }

        public void SetIsFalling(bool isFalling)
        {
            _animator.SetBool(isFallingParameterName, isFalling);
        } 
        
        public void Jump()
        {
            _animator.SetTrigger(jumpTriggerName);
        }

        public void Attack(int counter)
        {
            _animator.SetFloat(attackCounterParameterName, counter);
            _animator.SetTrigger(attackTriggerName);
        }
        
        public void Hurt()
        {
            _animator.SetTrigger(hurtTriggerName);
        }
        
        public void Death()
        {
            _animator.SetTrigger(deathTriggerName);
        }
    }
}

