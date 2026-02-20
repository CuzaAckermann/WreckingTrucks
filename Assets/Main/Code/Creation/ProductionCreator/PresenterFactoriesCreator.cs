using System;
using System.Collections.Generic;
using UnityEngine;

public class PresenterFactoriesCreator
{
    private readonly PresenterFactoriesSettings _settings;
    private readonly Transform _poolParent;
    private readonly Func<Presenter, Transform, Presenter> _createFunction;

    public PresenterFactoriesCreator(PresenterFactoriesSettings settings,
                                     Transform poolParent,
                                     Func<Presenter, Transform, Presenter> createFunction)
    {
        Validator.ValidateNotNull(settings, poolParent, createFunction);

        _settings = settings;
        _poolParent = poolParent;
        _createFunction = createFunction;
    }

    public List<Factory> Create()
    {
        BlockPresenterFactory blockPresenterFactory = new BlockPresenterFactory(_settings.BlockPresenterFactorySettings,
                                                                                GetPoolParent(_settings.BlockPresenterFactorySettings),
                                                                                _createFunction);

        TruckPresenterFactory truckPresenterFactory = new TruckPresenterFactory(_settings.TruckPresenterFactorySettings,
                                                                                GetPoolParent(_settings.TruckPresenterFactorySettings),
                                                                                _createFunction);

        BulletPresenterFactory bulletPresenterFactory = new BulletPresenterFactory(_settings.BulletPresenterFactorySettings,
                                                                                   GetPoolParent(_settings.BulletPresenterFactorySettings),
                                                                                   _createFunction);

        CartrigeBoxPresenterFactory cartrigeBoxPresenterFactory = new CartrigeBoxPresenterFactory(_settings.CartrigeBoxPresenterFactorySettings,
                                                                                                  GetPoolParent(_settings.CartrigeBoxPresenterFactorySettings),
                                                                                                  _createFunction);

        PlanePresenterFactory planePresenterFactory = new PlanePresenterFactory(_settings.PlanePresenterFactorySettings,
                                                                                GetPoolParent(_settings.PlanePresenterFactorySettings),
                                                                                _createFunction);

        SpawnDetectorFactory spawnDetectorFactory = new SpawnDetectorFactory(_settings.SpawnDetectorFactorySettings,
                                                                             GetPoolParent(_settings.SpawnDetectorFactorySettings),
                                                                             _createFunction);

        return new List<Factory>
        {
            blockPresenterFactory,
            truckPresenterFactory,
            bulletPresenterFactory,
            cartrigeBoxPresenterFactory,
            planePresenterFactory,
            spawnDetectorFactory
        };
    }

    private Transform GetPoolParent<P>(PresenterFactorySettings<P> settings) where P : Presenter
    {
        GameObject poolParent = new GameObject($"{typeof(Pool<>)}With{settings.Prefab.GetType()}");
        poolParent.transform.SetParent(_poolParent);

        return poolParent.transform;
    }
}