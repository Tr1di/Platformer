using Tridi;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Ranged/Instant Config", fileName = "Instant Config")]
public class InstantConfig : ScriptableObject
{
    [Header("Firing")]
    [SerializeField] private DamageInfo damage;
    [SerializeField] private float range;

    public DamageInfo Damage => new()
    {
        Damage = damage.Damage,
        Type = damage.Type
    };
    
    public float Range => range;
}