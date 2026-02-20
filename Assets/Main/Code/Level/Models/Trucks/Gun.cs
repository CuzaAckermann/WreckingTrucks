using System;
using UnityEngine;

public class Gun : Model, ICommandCreator
{
    private readonly BulletFactory _bulletFactory;
    private readonly Placer _placer;
    private readonly float _shotCooldown;

    private int _currentAmountBullet;

    private Block _target;

    private Vector3 _defaultRotation;

    private Command _currentCommand;

    private bool _isAiming;
    private bool _needFinished;
    private bool _isFinished;

    public Gun(Placeable positionManipulator,
               IMover mover,
               IRotator rotator,
               BulletFactory bulletFactory,
               Placer placer,
               int capacity,
               float shotCooldown)
        : base(positionManipulator,
               mover,
               rotator)
    {
        Validator.ValidateNotNull(bulletFactory, placer);
        Validator.ValidateMin(capacity, 0, true);
        Validator.ValidateMin(shotCooldown, 0, false);

        _bulletFactory = bulletFactory;
        _placer = placer;

        Capacity = capacity;
        _shotCooldown = shotCooldown;
    }

    public event Action<Bullet> ShotFired;
    public event Action<Gun> ShootingEnded;
    public event Action ReadyToFire;

    public event Action<Command> CommandCreated;

    public int Capacity { get; private set; }

    public Gunner Gunner { get; private set; }

    public void SetGunner(Gunner gunner)
    {
        Gunner = gunner ?? throw new ArgumentNullException(nameof(gunner));
    }

    public override void Destroy()
    {
        CancelCommand();

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

        Gunner.Rotator.Deactivated += ShootWithEmptyInParametr;

        Gunner.AimAtTarget(block.Placeable);

        //Rotator.SetTarget(block.PositionManipulator.Position);
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

            CancelCommand();
        }

        _target?.StayFree();

        ShootingEnded?.Invoke(this);
    }

    public void Charge()
    {
        _currentAmountBullet = Capacity;
    }

    private void ShootWithEmptyInParametr(ITickable _)
    {
        Gunner.Rotator.Deactivated -= ShootWithEmptyInParametr;

        if (Validator.IsRequiredType(_bulletFactory.Create(), out Bullet bullet) == false)
        {
            throw new InvalidOperationException();
        }

        _currentAmountBullet--;

        bullet.Placeable.SetForward(Placeable.Forward);
        _placer.Place(bullet, Placeable.Position);

        bullet.SetTarget(_target);
        ShotFired?.Invoke(bullet);

        _isAiming = false;

        if (_needFinished)
        {
            _isFinished = true;
        }
        else
        {
            SendCommand();
        }
    }

    private void Shoot(Model _)
    {
        //TargetRotationReached -= Shoot;

        if (Validator.IsRequiredType(_bulletFactory.Create(), out Bullet bullet) == false)
        {
            throw new InvalidOperationException();
        }

        _currentAmountBullet--;

        //bullet.SetFirstPosition(Placeable.Position);
        bullet.Placeable.SetForward(Placeable.Forward);
        bullet.SetTarget(_target);
        ShotFired?.Invoke(bullet);

        _isAiming = false;

        if (_needFinished)
        {
            _isFinished = true;
        }
        else
        {
            SendCommand();
        }
    }

    private void NotifyFree()
    {
        _currentCommand = null;

        ReadyToFire?.Invoke();
    }

    private void SendCommand()
    {
        _currentCommand = new Command(NotifyFree, _shotCooldown);

        CommandCreated?.Invoke(_currentCommand);
    }

    private void CancelCommand()
    {
        _currentCommand?.Cancel();

        _currentCommand = null;
    }
}