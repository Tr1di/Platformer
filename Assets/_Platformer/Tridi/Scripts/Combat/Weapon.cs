using System;
using System.Collections;
using UnityEngine;

namespace Tridi
{
    public class Weapon : MonoBehaviour
    {
        public enum WeaponState
        {
            Preparation,
            Execution,
            Cooldown,
            Idle
        }

        [Serializable]
        public class AttackInfo
        {
            [SerializeField, Min(0f)] public float range;
            [SerializeField, Min(0f)] public float preparation;
            [SerializeField, Min(0f)] public float duration;
            [SerializeField, Min(0f)] public float cooldown;
        }
        
        [Header("Weapon")] 
        [SerializeField] private DamageInfo damage;

        [Header("Attack")] 
        [SerializeField] private Transform attackOffset;
        [SerializeField] private LayerMask attackingLayers;
        
        [Header("Combo")]
        [SerializeField, Min(0f)] private float comboCooldown;
        [SerializeField] private AttackInfo[] attacks;

        private bool _pendingAttack;
        private int _comboCounter;

        private WeaponState _state = WeaponState.Idle;
        
        public int ComboCounter
        {
            get => _comboCounter;
            private set => _comboCounter = value % attacks.Length;
        }

        public bool IsAttacking => _state < WeaponState.Idle;

        public delegate void OnAttack(int counter);
        public event OnAttack onAttack;
        
        private void Awake()
        {
            damage.Causer = gameObject;
        }

        public void Attack()
        {
            if (_pendingAttack) return;
            
            if (_state < WeaponState.Idle)
            {
                _pendingAttack = true;
                return;
            }
            
            StartCoroutine(PerformAttack());
        }

        private IEnumerator PerformAttack()
        {
            if (IsAttacking) yield break;
            
            onAttack?.Invoke(ComboCounter);
            _state = WeaponState.Preparation;
            yield return new WaitForSeconds(attacks[ComboCounter].preparation);

            _pendingAttack = false;
            _state = WeaponState.Execution;
            StartCoroutine(DealDamage());
            yield return new WaitForSeconds(attacks[ComboCounter].duration);
            
            _state = WeaponState.Cooldown;
            yield return new WaitForSeconds(attacks[ComboCounter].cooldown);

            ComboCounter++;
            _state = WeaponState.Idle;
            if (_pendingAttack)
            {
                StartCoroutine(PerformAttack());
                yield break;
            }
            yield return new WaitForSeconds(comboCooldown);
            
            if (!IsAttacking) ComboCounter = 0;
        }

        private IEnumerator DealDamage()
        {
            while (_state == WeaponState.Execution)
            {
                var results = Physics2D.CircleCastAll(attackOffset.position, attacks[ComboCounter].range, 
                    transform.right, 0f, attackingLayers);
            
                foreach (var result in results)
                {
                    var damageable = result.collider.GetComponent<IDamageable>();
                    damageable?.TakeDamage(new DamageInfo
                    {
                        Damage = damage.Damage,
                        Type = damage.Type,
                        Causer = damage.Causer,
                        Hit = result
                    });
                }

                yield return new WaitForFixedUpdate();
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            if (attacks.Length == 0) return;
            Gizmos.DrawWireSphere(attackOffset.position, attacks[0].range);
        }
    }
}
