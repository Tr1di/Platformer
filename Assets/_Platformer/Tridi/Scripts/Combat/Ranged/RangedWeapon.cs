using System;
using System.Collections;
using UnityEngine;

public enum WeaponState
{
    Idle,
    Fire,
    Reloading
}

public abstract class RangedWeapon : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] private RangedConfig config;
    [SerializeField] private Transform muzzle;
    [SerializeField] private LayerMask layers;
    [Space]
    [SerializeField, Min(0)] private int initialClips;
    
    private int _currentClip;
    private int _currentAmmo;

    private float _lastFireTime;
    
    private int _burstCounter;
    
    private bool _pendingFire;
    private bool _pendingShoot;
    private bool _pendingReload;

    private Coroutine _fireCoroutine;
    
    public float ClipRatio => _currentClip / (float)config.ClipSize;
    
    public int Burst => _burstCounter;
    
    public event Action<int> onAmmoUpdated;
    
    public WeaponState State { get; private set; }

    private void Start()
    {
        _currentAmmo = Mathf.Min(config.MaxAmmo, initialClips * config.ClipSize);
        PerformReload();
    }

    private void SetState(WeaponState state)
    {
        if (State == WeaponState.Fire && state != WeaponState.Fire)
        {
            StopBurst();
        }

        var previousState = State;
        State = state;

        if (State == WeaponState.Fire && previousState != WeaponState.Fire)
        {
            StartBurst();
        }
    }
    
    private void DetermineState()
    {
        var result = WeaponState.Idle;

        if (_pendingReload)
        {
            result = CanReload() ? WeaponState.Reloading : State;
        }
        else if (_pendingFire && CanFire())
        {
            result = WeaponState.Fire;
        }
        
        SetState(result);
    }

    private void UseAmmo()
    {
        _currentClip--;
        onAmmoUpdated?.Invoke(_currentClip);
    }
    
    public void StartFire()
    {
        if (_pendingFire) return;

        _pendingFire = true;
        DetermineState();
    }

    protected virtual void StartBurst()
    {
        if (_pendingShoot) return;

        var gameTime = Time.time;
        var nextFireTime = _lastFireTime + config.TimeBetweenShots;
        
        _fireCoroutine = StartCoroutine(HandleFire(Mathf.Max(0f, nextFireTime - gameTime)));
    }

    private IEnumerator HandleFire(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        
        if (_currentClip > 0 && CanFire())
        {
            Fire();
            UseAmmo();
            _burstCounter++;
        }
        else if (CanReload())
        {
            Reload();
        }
        else
        {
            StopBurst();
        }

        if (CanReFire())
        {
            yield return new WaitForSeconds(config.TimeBetweenShots);
            _fireCoroutine = StartCoroutine(HandleFire());
        }
        else
        {
            StopBurst();
        }

        _lastFireTime = Time.time;
    }
    
    protected abstract void Fire();
    
    protected virtual void StopBurst()
    {
        _burstCounter = 0;
        StopCoroutine(_fireCoroutine);
    }
    
    public void StopFire()
    {
        if (!_pendingFire) return;
        
        _pendingFire = false;
        DetermineState();
    }
    
    public void Reload()
    {
        if (_pendingReload || !CanReload()) return;
        
        _pendingReload = true;
        DetermineState();

        var reloadDuration = Mathf.Max(0.01f, config.ReloadTime);
        Invoke(nameof(PerformReload), reloadDuration);
        Invoke(nameof(StopReload), reloadDuration);
    }

    private void PerformReload()
    {
        var delta = config.ClipSize - _currentClip;
        var result = Mathf.Min(delta, _currentAmmo);
        
        if (result <= 0) return;
        
        _currentClip += result;
        _currentAmmo -= result;
        
        onAmmoUpdated?.Invoke(_currentClip);
    }
    
    private void StopReload()
    {
        if (!_pendingReload) return;
        
        _pendingReload = false;
        DetermineState();
    }
    
    public bool CanFire()
    {
        return State < WeaponState.Reloading || (State == WeaponState.Reloading && !_pendingReload);
    }
    
    public bool CanReFire()
    {
        var modeOk = config.Mode == FireMode.Auto;
        var stateOk = State == WeaponState.Fire;
        return modeOk && stateOk;
    }

    public bool CanReload()
    {
        var canReload = _currentAmmo > 0;
        var needAmmo = _currentClip < config.ClipSize;
        var stateOk = State < WeaponState.Reloading;
        return canReload && needAmmo && stateOk;
    }

    public RaycastHit2D Trace(Vector2 start, Vector2 end)
    {
        return Physics2D.Linecast(start, end, layers);
    }
    
    public Vector3 GetMuzzleLocation()
    {
        return muzzle.position;
    }

    public Vector3 GetMuzzleDirection()
    {
        return muzzle.forward;
    }
}
