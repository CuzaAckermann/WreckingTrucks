using System;
using UnityEngine;

public class Gun : Model
{
    private readonly StopwatchWaitingState _stopwatchWaitingState;

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

        _stopwatchWaitingState = new StopwatchWaitingState(_stopwatch, shotCooldown);
    }

    public event Action<Bullet> ShotFired;
    public event Action<Gun> ShootingEnded;
    public event Action ReadyToFire;

    public int Capacity { get; private set; }

    public Gunner Gunner { get; private set; }

    public void SetGunner(Gunner gunner)
    {
        Gunner = gunner ?? throw new ArgumentNullException(nameof(gunner));
    }

    public override void Destroy()
    {
        _stopwatch.Destroy();
        Gunner.Destroy();

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

        Gunner.Aimed += ShootWithEmptyInParametr;
        Gunner.AimAtTarget(block);
    }

    public void Finish(Vector3 defaultRotation)
    {
        //_defaultRotation = defaultRotation;
        //SetTargetRotation(defaultRotation);
    }

    public void StopShooting()
    {
        _needFinished = true;

        if (_isAiming == false)
        {
            _isFinished = true;

            _stopwatchWaitingState.Exit();
        }

        _target?.StayFree();

        ShootingEnded?.Invoke(this);
    }

    public void Charge()
    {
        _currentAmountBullet = Capacity;
    }

    public override void SetFirstPosition(Vector3 position)
    {
        base.SetFirstPosition(position);
        Gunner.Turret.SetFirstPosition(position);
        Gunner.Turret.Barrel.SetFirstPosition(position);
    }

    private void ShootWithEmptyInParametr()
    {
        Gunner.Aimed -= ShootWithEmptyInParametr;

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
            _stopwatchWaitingState.Enter(OnIntervalPassed);
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
            _stopwatchWaitingState.Enter(OnIntervalPassed);
        }
    }

    private void OnIntervalPassed()
    {
        _stopwatchWaitingState.Exit();

        ReadyToFire?.Invoke();
    }
}