using Tridi;
using UnityEngine;

public class RangedInstant : RangedWeapon
{
    [Header("Instant")]
    [SerializeField] private InstantConfig instantConfig;
    
    protected override void Fire()
    {
        var start = GetMuzzleLocation();
        var direction = GetMuzzleDirection();
        var end = start + direction * instantConfig.Range;
        
        ProcessHit(Trace(start, end));
    }
    
    private void ProcessHit(RaycastHit2D hit)
    {
        if (!hit.collider) return;
        DealDamage(hit.collider.GetComponent<IDamageable>(), hit);
    }
    
    private void DealDamage(IDamageable target, RaycastHit2D hit)
    {
        if (target == null) return;

        var damage = instantConfig.Damage;
        damage.Causer = gameObject;
        damage.Hit = hit;

        target.TakeDamage(damage);
    }
}
