using UnityEngine;

namespace Tridi
{
    public class DamageTrigger : Trigger
    {
        [Header("Damage")] 
        [SerializeField] private float damage;

        public override void Enter(Collider2D other)
        {
            var damageable = other.GetComponent<IDamageable>();

            var damageInfo = new DamageInfo
            {
                Damage = damage
            };

            damageable?.TakeDamage(damageInfo);
        }
    }
}