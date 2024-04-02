using System;
using UnityEngine;

public class RangedProjectile : RangedWeapon
{
    [SerializeField] private ProjectileConfig projectileConfig;
    
    protected override void Fire()
    {
        var go = Instantiate(projectileConfig.Projectile);
        
        var projectile = go.GetComponent<Projectile>();
        if (!projectile) throw new ArgumentException("Projectile prefab doesn't have a projectile script");

        go.transform.position = GetMuzzleLocation();

        var direction = GetMuzzleDirection();
        var speed = projectileConfig.InitialSpeed;
        
        projectile.InitVelocity(direction * speed);
        
        Destroy(go, projectileConfig.LifeTime);
    }
}