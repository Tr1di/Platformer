using System;
using UnityEngine;

namespace Tridi
{
    [Serializable]
    public class ArmorDecorator : HealthDecorator
    {
        [Serializable]
        public enum ArmorType
        {
            ReduceDamage,
            AbsorbDamage
        }

        [SerializeField] private Health armor;
        [SerializeField] private ArmorType type = ArmorType.AbsorbDamage;

        private IHealth _armor;

        private bool FlatDamage => type == ArmorType.ReduceDamage || !_armor.IsAlive;

        public override bool CanBeDamaged(DamageInfo damageInfo)
        {
            return _armor.CanBeDamaged(damageInfo) || base.CanBeDamaged(damageInfo);
        }

        public override float TakeDamage(DamageInfo damageInfo)
        {
            if (damageInfo.Damage <= 0f) return 0f;
            if (!CanBeDamaged(damageInfo)) return 0f;

            var totalDamage = _armor.TakeDamage(damageInfo);
            damageInfo.Damage -= FlatDamage ? totalDamage : damageInfo.Damage;
            totalDamage += base.TakeDamage(damageInfo);

            return totalDamage;
        }

        public void Decorate(HealthDecorator decorator)
        {
            _armor ??= armor;
            _armor = decorator.Assign(_armor);
        }
    }
}
