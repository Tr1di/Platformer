using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tridi
{
    [Serializable]
    public enum DamageType
    {
        Common
    }

    [Serializable]
    public struct DamageInfo
    {
        [SerializeField, Min(0f)] private float damage;
        [SerializeField] private DamageType type;
        private GameObject _causer;
        private RaycastHit2D _hit;
        
        public DamageType Type
        {
            get => type; 
            set => type = value;
        }

        public float Damage
        {
            get => damage; 
            set => damage = Mathf.Max(0, value);
        }

        public GameObject Causer
        {
            get => _causer;
            set => _causer = value;
        }
        
        public RaycastHit2D Hit
        {
            get => _hit;
            set => _hit = value;
        }
    }

    public interface IDamageable
    {
        float TakeDamage(DamageInfo damageInfo);
    }
}