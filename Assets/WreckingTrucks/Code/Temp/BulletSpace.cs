using System;
using System.Collections.Generic;

public class BulletSpace : IPositionsModelsChangedNotifier
{
    private RoadSpace _roadSpace;

    private List<Gun> _guns;

    public BulletSpace(Mover bulletsMover, RoadSpace roadSpace)
    {
        _roadSpace = roadSpace ?? throw new ArgumentNullException(nameof(roadSpace));
        _guns = new List<Gun>();
    }

    public event Action<Model> ModelAdded;
    public event Action<List<Model>> TargetPositionsModelsChanged;

    public void Start()
    {
        _roadSpace.GunReady += OnGunReady;
    }

    public void Exit()
    {
        _roadSpace.GunReady -= OnGunReady;
    }

    public void Clear()
    {
        for (int i = 0; i < _guns.Count; i++)
        {
            UnsubscribeFromGun(_guns[i]);
        }
    }

    private void OnGunReady(Gun gun)
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

    private void OnDestroyed(Model gun)
    {
        UnsubscribeFromGun((Gun)gun);
    }

    private void OnModelAdded(Model model)
    {
        ModelAdded?.Invoke(model);
    }

    private void SubscribeToGun(Gun gun)
    {
        gun.Destroyed += OnDestroyed;
        gun.ModelAdded += OnModelAdded;

    }

    private void UnsubscribeFromGun(Gun gun)
    {
        gun.Destroyed -= OnDestroyed;
        gun.ModelAdded -= OnModelAdded;
    }
}