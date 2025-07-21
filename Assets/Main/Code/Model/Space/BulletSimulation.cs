using System;
using System.Collections.Generic;

public class BulletSimulation : IModelPositionObserver,
                                IModelAddedNotifier
{
    private readonly Charger _charger;
    private readonly List<Gun> _guns;

    public BulletSimulation(Charger charger)
    {
        _charger = charger ?? throw new ArgumentNullException(nameof(charger));
        _guns = new List<Gun>();
    }

    public event Action<Model> ModelAdded;
    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;

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
        gun.Destroyed += UnsubscribeFromGun;
        gun.ShotFired += OnShotFired;
        gun.Preparing += OnPreparing;
    }

    private void UnsubscribeFromGun(Model model)
    {
        model.Destroyed -= UnsubscribeFromGun;

        if (model is Gun gun)
        {
            gun.ShotFired -= OnShotFired;
            gun.Preparing -= OnPreparing;
            //_guns.Remove(gun);
        }
    }

    private void OnShotFired(Bullet bullet)
    {
        ModelAdded?.Invoke(bullet);
        PositionChanged?.Invoke(bullet);
    }

    private void OnPreparing(Gun gun)
    {
        _charger.ChargeGun(gun);
    }
}