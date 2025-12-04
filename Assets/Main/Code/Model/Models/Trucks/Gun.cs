using System;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Model
{
    private readonly BulletFactory _bulletFactory;
    private readonly Stopwatch _stopwatch;

    private int _currentAmountBullet;

    private Block _target;

    private Vector3 _defaultRotation;

    private bool _isAiming;
    private bool _needFinished;
    private bool _isFinished;

    public Gun(float movespeed,
               float rotatespeed,
               BulletFactory bulletFactory,
               int capacity,
               Stopwatch stopwatch,
               float shotCooldown)
        : base(movespeed,
               rotatespeed)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        if (shotCooldown < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shotCooldown));
        }

        Capacity = capacity;

        _bulletFactory = bulletFactory ?? throw new ArgumentNullException(nameof(bulletFactory));

        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));

        _stopwatch.SetNotificationInterval(shotCooldown);
    }

    public event Action<Bullet> ShotFired;
    public event Action<Gun> ShootingEnded;
    public event Action ReadyToFire;

    public int Capacity { get; private set; }

    public float AngleToTargetRotation => Vector3.Angle(Forward, DetermineTargetRotation());

    public override void Destroy()
    {
        _stopwatch.Destroy();

        base.Destroy();
    }

    public void Upload()
    {
        _isFinished = false;
        _needFinished = false;

        Charge();
    }

    public void Upload(int amountBullets)
    {
        if (amountBullets <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountBullets));
        }

        Capacity = amountBullets;
        Upload();
    }

    public bool CanShoot()
    {
        return _currentAmountBullet > 0;
    }

    public void Aim(Block block)
    {
        _isAiming = true;

        _target = block;

        TargetRotationReached += Shoot; 
        SetTargetRotation(_target.Position);
    }

    public void Finish(Vector3 defaultRotation)
    {
        _defaultRotation = defaultRotation;
        SetTargetRotation(defaultRotation);
    }

    public void StopShooting()
    {
        _needFinished = true;

        if (_isAiming == false)
        {
            _isFinished = true;

            _stopwatch.Stop();
            _stopwatch.IntervalPassed -= OnIntervalPassed;
        }

        _target?.StayFree();

        ShootingEnded?.Invoke(this);
    }

    public void Charge()
    {
        _currentAmountBullet = Capacity;
    }

    protected override Vector3 GetAxisOfRotation()
    {
        return Vector3.up;
    }

    protected override Vector3 GetTargetRotation(Vector3 target)
    {
        Vector3 targetDirection = target;
        targetDirection.y = 0;

        return targetDirection;
    }

    protected override Vector3 DetermineTargetRotation()
    {
        if (_isFinished == false)
        {
            return GetTargetRotation(_target.Position);
        }
        else
        {
            return GetTargetRotation(Position + Vector3.right);
        }
    }

    private void Shoot(Model _)
    {
        TargetRotationReached -= Shoot;

        Bullet bullet = _bulletFactory.Create();
        _currentAmountBullet--;

        bullet.SetFirstPosition(Position);
        bullet.SetDirectionForward(Forward);
        bullet.SetTarget(_target);
        ShotFired?.Invoke(bullet);

        _isAiming = false;

        if (_needFinished)
        {
            _isFinished = true;
        }
        else
        {
            _stopwatch.IntervalPassed += OnIntervalPassed;
            _stopwatch.Start();
        }
    }

    private void OnIntervalPassed()
    {
        _stopwatch.Stop();
        _stopwatch.IntervalPassed -= OnIntervalPassed;

        ReadyToFire?.Invoke();
    }
}