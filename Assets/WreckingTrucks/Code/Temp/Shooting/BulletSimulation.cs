using System;
using System.Collections.Generic;

public class BulletSimulation : IPositionsModelsChangedNotifier
{
    private RoadSpace _roadSpace;
    private List<Gun> _guns;

    public BulletSimulation()
    {
        _guns = new List<Gun>();
    }

    public event Action<List<Model>> TargetPositionsModelsChanged;

    public void AddRoadSpace(RoadSpace roadSpace)
    {
        _roadSpace = roadSpace ?? throw new ArgumentNullException(nameof(roadSpace));

    }

    public void Clear()
    {
        for (int i = 0; i < _guns.Count; i++)
        {
            UnsubscribeFromGun(_guns[i]);
        }

        _guns.Clear();
    }

    public void Start()
    {
        //_roadSpace.GunReady += AddGun;
    }

    public void Exit()
    {
        //_roadSpace.GunReady -= AddGun;
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

    private void OnDestroyed(Model model)
    {
        UnsubscribeFromGun((Gun)model);
    }

    private void SubscribeToGun(Gun gun)
    {
        gun.Destroyed += OnDestroyed;
    }

    private void UnsubscribeFromGun(Gun gun)
    {
        gun.Destroyed -= OnDestroyed;
    }
}