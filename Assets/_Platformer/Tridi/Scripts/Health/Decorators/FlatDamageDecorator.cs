using System;
using UnityEngine;

namespace Tridi
{
    /// <summary>
    /// Ограничивает входящий урон до указанного значения
    /// </summary>
    [Serializable]
    public class FlatDamageDecorator : HealthDecorator
    {
        [SerializeField] private float maxDamage = 20f;

        public override float TakeDamage(DamageInfo damageInfo)
        {
            damageInfo.Damage = Mathf.Min(damageInfo.Damage, maxDamage);
            return base.TakeDamage(damageInfo);
        }
    }
}