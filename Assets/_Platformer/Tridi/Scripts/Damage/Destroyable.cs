using UnityEngine;
using UnityEngine.Events;

namespace Tridi
{
    [RequireComponent(typeof(Health))]
    public class Destroyable : MonoBehaviour, IDamageable
    {
        [SerializeField] private DamageCooldownDecorator damageCooldown;
        [Space]
        [SerializeField] private UnityEvent onDeath;

        private IHealth _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _health.onDeath += Death;
            
            _health = damageCooldown.Assign(_health);
        }

        private void Death(IHealth component, DamageInfo damageinfo)
        {
            onDeath?.Invoke();
        }

        public float TakeDamage(DamageInfo damageInfo)
        {
            return _health.TakeDamage(damageInfo);
        }
    }
}
