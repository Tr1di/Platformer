using System;
using Tridi;
using UnityEngine;

[Serializable]
public enum FireMode
{
    Auto,
    Single
}

[CreateAssetMenu(menuName = "Weapon/Ranged/Config", fileName = "Ranged Config")]
public class RangedConfig : ScriptableObject
{
    [Header("Firing")]
    [SerializeField, Min(0f)] private float fireRate;
    [SerializeField] private FireMode mode;
    [SerializeField, Min(0f)] private float reloadTime;
    
    [Header("Ammo")]
    [SerializeField, Min(0)] private int clipSize;
    [SerializeField, Min(0)] private int maxAmmo;
    
    public float FireRate => fireRate;
    public float TimeBetweenShots => 1f / FireRate;
    public FireMode Mode => mode;
    
    public int ClipSize => clipSize;
    public int MaxAmmo => maxAmmo;
    public float ReloadTime => reloadTime;
}