using System;
using UnityEngine;

namespace Tridi
{
    [Serializable]
    public class DamageRatioDecorator : HealthDecorator
    {
        [SerializeField, Range(0f, 100f)] private float damagePercentage = 20f;

        private float DamageRatio => damagePercentage / 100f;

        public override float TakeDamage(DamageInfo damageInfo)
        {
            damageInfo.Damage *= DamageRatio;
            return base.TakeDamage(damageInfo);
        }
    }
}