using System;
using System.Collections.Generic;

public class BulletSimulation : IPositionsModelsChangedNotifier,
                                IModelAddedNotifier
{
    private readonly List<Gun> _guns;

    public BulletSimulation()
    {
        _guns = new List<Gun>();
    }

    public event Action<List<Model>> TargetPositionsModelsChanged;
    public event Action<Model> ModelAdded;

    public void Clear()
    {
        foreach (Gun gun in _guns)
        {
            if (gun != null)
            {
                UnsubscribeFromGun(gun);
            }
        }

        _guns.Clear();
    }

    public void AddGun(Gun gun)
    {
        if (gun == null)
        {
            throw new ArgumentNullException(nameof(gun));
        }

        if (_guns.Contains(gun))
        {
            throw new InvalidOperationException(nameof(gun));
        }

        _guns.Add(gun);
        SubscribeToGun(gun);
    }

    private void SubscribeToGun(Gun gun)
    {
        gun.ShotFired += OnShotFired;
    }

    private void UnsubscribeFromGun(Gun gun)
    {
        gun.ShotFired -= OnShotFired;
    }

    private void OnShotFired(Bullet bullet)
    {
        ModelAdded?.Invoke(bullet);
        TargetPositionsModelsChanged?.Invoke(new List<Model> { bullet });
    }
}