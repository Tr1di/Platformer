using System;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Tridi
{
    [Serializable]
    public class DamageCooldownDecorator : HealthDecorator
    {
        [SerializeField] private float cooldownTime = 1f;

        private bool _isInCooldown;

        public override IHealth Assign(IHealth health)
        {
            base.Assign(health);
            health.onDamage += OnDamage;
            return this;
        }

        public override bool CanBeDamaged(DamageInfo damageInfo)
        {
            return !_isInCooldown && base.CanBeDamaged(damageInfo);
        }

        public override float TakeDamage(DamageInfo damageInfo)
        {
            return _isInCooldown ? 0f : base.TakeDamage(damageInfo);
        }

        private void OnDamage(IHealth component, DamageInfo damageInfo)
        {
            CoroutineRunner.instance.StartCoroutine(DoCooldown());
        }

        private IEnumerator DoCooldown()
        {
            _isInCooldown = true;
            yield return new WaitForSeconds(cooldownTime);
            _isInCooldown = false;
        }
    }
}