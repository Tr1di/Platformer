using System;
using System.Linq;
using Tridi;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private DamageInfo damage;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask layers;
    
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void InitVelocity(Vector2 newVelocity)
    {
        _rigidbody.velocity = newVelocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var start = other.contacts[0].point;
        var hits = Physics2D.CircleCastAll(start, explosionRadius, Vector2.right, 0.01f, layers);

        foreach (var hit in hits)
        {
           var target = hit.collider.GetComponent<IDamageable>();
           target?.TakeDamage(new DamageInfo
           {
               Damage = damage.Damage,
               Type = damage.Type,
               Causer = gameObject,
               Hit = hit
           });
        }

        gameObject.SetActive(false);
    }
}