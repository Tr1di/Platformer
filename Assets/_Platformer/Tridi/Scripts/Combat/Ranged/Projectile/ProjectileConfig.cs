using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Ranged/Projectile Config", fileName = "Projectile Config")]
public class ProjectileConfig : ScriptableObject
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField, Min(0f)] private float initialSpeed;
    [SerializeField, Min(0f)] private float lifeTime;
    
    public GameObject Projectile => projectilePrefab;
    public float InitialSpeed => initialSpeed;
    public float LifeTime => lifeTime;
}