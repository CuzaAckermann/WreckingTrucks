using System;
using System.Collections.Generic;

public class BulletSimulation : IModelPositionObserver,
                                IModelAddedNotifier
{
    private readonly Charger _charger;
    private readonly List<Gun> _guns;
    private readonly List<Bullet> _bullets;

    public BulletSimulation(Charger charger)
    {
        _charger = charger ?? throw new ArgumentNullException(nameof(charger));
        _guns = new List<Gun>();
        _bullets = new List<Bullet>();
    }

    public event Action<Model> ModelAdded;
    public event Action<Model> PositionChanged;
    public event Action<List<Model>> PositionsChanged;

    public IReadOnlyList<Bullet> Bullets => _bullets;

    public void Clear()
    {
        for (int i = _guns.Count - 1; i >= 0; i--)
        {
            if (_guns[i] != null)
            {
                UnsubscribeFromGun(_guns[i]);
            }
        }

        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            if (_bullets[i] != null)
            {
                OnDestroyed(_bullets[i]);
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
            _guns.Remove(gun);
        }
    }

    private void OnShotFired(Bullet bullet)
    {
        bullet.Destroyed += OnDestroyed;
        _bullets.Add(bullet);
        ModelAdded?.Invoke(bullet);
        PositionChanged?.Invoke(bullet);
    }

    private void OnPreparing(Gun gun)
    {
        _charger.ChargeGun(gun);
    }

    private void OnDestroyed(Model model)
    {
        model.Destroyed -= OnDestroyed;

        if (model is Bullet bullet)
        {
            _bullets.Remove(bullet);
        }
    }
}