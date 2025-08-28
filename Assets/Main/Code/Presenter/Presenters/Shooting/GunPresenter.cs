using System;
using UnityEngine;

public class GunPresenter : Presenter
{
    [SerializeField] private Transform _shootingPoint;

    private Gun _gun;
    private bool _isSubscribed;

    public event Action ShootingEnded;

    public override void Bind(Model model)
    {
        if (model is Gun gun)
        {
            _gun = gun;
            _gun.SetPosition(Transform.position);
            _gun.SetDirectionForward(transform.forward);
        }

        base.Bind(model);
    }

    public void SetTargetRotation(Vector3 defaultForward)
    {
        Model.SetTargetRotation(defaultForward);
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
            _gun.ShotFired += OnShotFired;
            _gun.RotationChanged += OnRotationChanged;
            _gun.ShootingEnded += OnShootingEnded;
            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromGun()
    {
        if (_gun != null && _isSubscribed)
        {
            _gun.ShotFired -= OnShotFired;
            _gun.RotationChanged -= OnRotationChanged;
            _gun.ShootingEnded -= OnShootingEnded;
            _isSubscribed = false;
        }
    }

    private void OnShotFired(Bullet bullet)
    {
        //bullet.SetPosition(_shootingPoint.position);
        //bullet.SetDirectionForward(_shootingPoint.forward);
    }

    private void OnShootingEnded(Gun _)
    {
        ShootingEnded?.Invoke();
    }
}