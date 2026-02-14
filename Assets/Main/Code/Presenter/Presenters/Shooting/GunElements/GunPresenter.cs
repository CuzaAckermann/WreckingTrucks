using System;
using UnityEngine;

public class GunPresenter : Presenter, ICompletionNotifier
{
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private TurretPresenter _turretPresenter;
    [SerializeField] private BarrelPresenter _barrelPresenter;

    private Gun _gun;
    private bool _isSubscribed;

    public override void Init()
    {
        base.Init();

        _turretPresenter?.Init();
        _barrelPresenter?.Init();
    }

    public event Action ShootingEnded;
    public event Action Completed;

    public override void Bind(Model model)
    {
        if (model is Gun gun)
        {
            UnsubscribeFromGun();

            _gun = gun;
            _gun.Placeable.SetPosition(Transform.position);
            _gun.Placeable.SetForward(Transform.forward);

            _turretPresenter?.Bind(_gun.Gunner.Turret);
            _barrelPresenter?.Bind(_gun.Gunner.Turret.Barrel);
        }

        base.Bind(model);
    }

    public void SetTargetRotation(Vector3 defaultForward)
    {
        //Logger.Log("Поворачиваемся поумолчанию");

        _gun.Finish(defaultForward);
    }

    public override void ChangePosition()
    {
        _turretPresenter?.ChangePosition();
        _barrelPresenter?.ChangePosition();

        base.ChangePosition();
    }

    protected override void Subscribe()
    {
        SubscribeToGun();
    }

    protected override void Unsubscribe()
    {
        UnsubscribeFromGun();
    }

    private void SubscribeToGun()
    {
        if (_gun != null && _isSubscribed == false)
        {
            _gun.Destroyed += OnDestroyed;
            _gun.Placeable.RotationChanged += OnRotationChanged;

            _gun.ShotFired += OnShotFired;
            _gun.ShootingEnded += OnShootingEnded;
            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromGun()
    {
        if (_gun != null && _isSubscribed)
        {
            _gun.Destroyed -= OnDestroyed;
            _gun.Placeable.RotationChanged -= OnRotationChanged;

            _gun.ShotFired -= OnShotFired;
            _gun.ShootingEnded -= OnShootingEnded;
            _isSubscribed = false;
        }
    }

    private void OnShotFired(Bullet bullet)
    {
        bullet.Placeable.SetPosition(_shootingPoint.position);
        bullet.Placeable.SetForward(_shootingPoint.forward);
    }

    private void OnShootingEnded(Gun _)
    {
        ShootingEnded?.Invoke();
        Completed?.Invoke();
    }

    private void OnDestroyed(IDestroyable destroyable)
    {
        if (Validator.IsRequiredType(destroyable, out Gun gun) == false)
        {
            return;
        }

        if (gun != _gun)
        {
            return;
        }

        UnsubscribeFromGun();
    }
}