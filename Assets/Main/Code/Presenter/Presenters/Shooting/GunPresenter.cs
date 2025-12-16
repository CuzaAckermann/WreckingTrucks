using System;
using UnityEngine;

public class GunPresenter : Presenter, ICompletionNotifier
{
    [SerializeField] private Transform _shootingPoint;

    private Gun _gun;
    private bool _isSubscribed;

    public event Action ShootingEnded;
    public event Action Completed;

    public override void Bind(Model model)
    {
        if (model is Gun gun)
        {
            UnsubscribeFromGun();

            _gun = gun;
            _gun.SetPosition(Transform.position);
            _gun.SetDirectionForward(Transform.forward);
        }

        base.Bind(model);
    }

    public void SetTargetRotation(Vector3 defaultForward)
    {
        //Logger.Log("Поворачиваемся поумолчанию");

        _gun.Finish(defaultForward);
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
            _gun.DestroyedModel += OnDestroyed;
            _gun.RotationChanged += OnRotationChanged;

            _gun.ShotFired += OnShotFired;
            _gun.ShootingEnded += OnShootingEnded;
            _isSubscribed = true;
        }
    }

    private void UnsubscribeFromGun()
    {
        if (_gun != null && _isSubscribed)
        {
            _gun.DestroyedModel -= OnDestroyed;
            _gun.RotationChanged -= OnRotationChanged;

            _gun.ShotFired -= OnShotFired;
            _gun.ShootingEnded -= OnShootingEnded;
            _isSubscribed = false;
        }
    }

    private void OnShotFired(Bullet bullet)
    {
        bullet.SetPosition(_shootingPoint.position);
        bullet.SetDirectionForward(_shootingPoint.forward);
    }

    private void OnShootingEnded(Gun _)
    {
        ShootingEnded?.Invoke();
        Completed?.Invoke();
    }

    private void OnDestroyed(Model model)
    {
        if (model is Gun gun)
        {
            if (gun == _gun)
            {
                UnsubscribeFromGun();
            }
        }
    }
}