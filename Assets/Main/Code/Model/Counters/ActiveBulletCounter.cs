using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBulletCounter
{
    private readonly List<Bullet> _activedBullets;

    public ActiveBulletCounter()
    {
        _activedBullets = new List<Bullet>();
    }

    public event Action ActivedBulletIsEmpty;

    public int Amount => _activedBullets.Count;

    public void SubscribeToGun(Gun gun)
    {
        if (gun == null)
        {
            throw new ArgumentNullException(nameof(gun));
        }

        gun.DestroyedModel += UnsubscribeFromGun;
        gun.ShotFired += OnShotFired;
    }

    private void UnsubscribeFromGun(Model model)
    {
        model.DestroyedModel -= UnsubscribeFromGun;

        if (model is Gun gun)
        {
            gun.ShotFired -= OnShotFired;
        }
    }

    private void OnShotFired(Bullet bullet)
    {
        SubscribeToBullet(bullet);
    }

    private void SubscribeToBullet(Bullet bullet)
    {
        if (bullet == null)
        {
            throw new ArgumentNullException(nameof(bullet));
        }

        if (_activedBullets.Contains(bullet))
        {
            throw new InvalidOperationException($"{nameof(bullet)} already added");
        }

        _activedBullets.Add(bullet);

        bullet.DestroyedModel += UnsubscribeFromBullet;
    }

    private void UnsubscribeFromBullet(Model model)
    {
        model.DestroyedModel -= UnsubscribeFromBullet;

        if (model is Bullet bullet)
        {
            _activedBullets.Remove(bullet);

            if (_activedBullets.Count == 0)
            {
                ActivedBulletIsEmpty?.Invoke();
            }
        }
    }
}