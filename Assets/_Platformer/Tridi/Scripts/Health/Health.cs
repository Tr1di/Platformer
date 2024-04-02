using System;
using Tridi.HealthEvents;
using UnityEngine;
using UnityEngine.Events;

namespace Tridi
{
    public class Health : MonoBehaviour, IHealth, ISavable
    {
        [Serializable]
        public class SaveData
        {
            [SerializeField] public string id;
            [SerializeField] public float current;
        }
        
        [Serializable]
        private class Events
        {
            [SerializeField] public UnityEvent onDamage;
            [SerializeField] public UnityEvent onHeal;
            [SerializeField] public UnityEvent onDeath;
        }

        [Id]
        [SerializeField] private string id;
        
        [SerializeField, Min(0f)] private float max = 100f;
        [SerializeField, Min(0f)] private float current = 100f;

        public float Max => max;
        public float Ratio => current / max;

        public bool IsAlive => current > 0f;

        #region Events

        [SerializeField, Space] private Events events;

        public event OnDamage onDamage;
        public event OnDamage onHeal;
        public event OnDeath onDeath;

        #endregion

        private void Start()
        {
            current = Mathf.Clamp(current, 0f, max);
        }

        private float ApplyDamage(float damage)
        {
            var oldHealth = current;
            current = Mathf.Clamp(current - damage, 0f, max);

            return Mathf.Abs(oldHealth - current);
        }

        #region Damage

        public virtual bool CanBeDamaged(DamageInfo damageInfo)
        {
            return IsAlive && damageInfo.Causer != gameObject;
        }

        public virtual float TakeDamage(DamageInfo damageInfo)
        {
            if (damageInfo.Damage <= 0f) return 0f;
            if (!CanBeDamaged(damageInfo)) return 0f;

            damageInfo.Damage = ApplyDamage(damageInfo.Damage);

            if (damageInfo.Damage > 0f)
            {
                onDamage?.Invoke(this, damageInfo);
                events.onDamage.Invoke();
            }

            if (!IsAlive)
            {
                onDeath?.Invoke(this, damageInfo);
                events.onDeath?.Invoke();
            }

            return damageInfo.Damage;
        }

        #endregion

        #region Heal

        public virtual bool CanBeHealed(DamageInfo damageInfo)
        {
            return IsAlive;
        }

        public virtual float TakeHeal(DamageInfo damageInfo)
        {
            if (damageInfo.Damage <= 0f) return 0f;
            if (!CanBeHealed(damageInfo)) return 0f;

            damageInfo.Damage = ApplyDamage(-damageInfo.Damage);

            if (damageInfo.Damage > 0f)
            {
                onHeal?.Invoke(this, damageInfo);
                events.onHeal.Invoke();
            }

            return damageInfo.Damage;
        }

        #endregion

        #region
        public void LoadGame(SaveGame data)
        {
            var save = data.GetHealth(id);
            if (save == null) return;
            
            current = save.current;
            onHeal?.Invoke(this, new DamageInfo());
        }

        public void SaveGame(SaveGame data)
        {
            var save = new SaveData
            {
                id = id,
                current = current
            };
            
            data.SaveHealth(save);
        }
        #endregion
    }
}