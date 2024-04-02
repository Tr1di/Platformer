using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace Tridi
{
    [Serializable]
    public class AutoRegenDecorator : HealthDecorator
    {
        [SerializeField] private float targetHealth = 100f;

        [Tooltip("Health point per second")] [SerializeField]
        private float speed = 5f;

        [SerializeField] private float delay = 4f;
        [SerializeField] private float deadDelay = 4f;
        [SerializeField] private bool regenOnDeath;
        
        private float RegenAmount => speed * Time.fixedDeltaTime;
        private float TargetRatio => Mathf.Min(1f, targetHealth / Max);

        private CancellationTokenSource _tokenSource = new();
        private CancellationToken CancelOnDamage => _tokenSource.Token;
        
        private DamageInfo HealInfo => new()
        {
            Damage = RegenAmount
        };

        public override IHealth Assign(IHealth health)
        {
            base.Assign(health);
            health.onDamage += OnDamage;
            health.onDeath += OnDeath;
            return this;
        }

        private void OnDeath(IHealth component, DamageInfo damageinfo)
        {
            Cancel();
            if (regenOnDeath) CoroutineRunner.instance.StartCoroutine(DoRegen(deadDelay, CancelOnDamage));
        }

        private void OnDamage(IHealth health, DamageInfo damageInfo)
        {
            if (!health.IsAlive) return;

            Cancel();
            CoroutineRunner.instance.StartCoroutine(DoRegen(delay, CancelOnDamage));
        }

        private void Cancel()
        {
            _tokenSource.Cancel();
            _tokenSource = new CancellationTokenSource();
        }

        private IEnumerator DoRegen(float inDelay, CancellationToken cancellation)
        {
            yield return new WaitForSeconds(inDelay);

            while (Ratio < TargetRatio)
            {
                if (cancellation.IsCancellationRequested) yield break;
                TakeHeal(HealInfo);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}