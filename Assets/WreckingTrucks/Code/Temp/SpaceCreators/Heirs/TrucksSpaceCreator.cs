using System;
using UnityEngine;

public class TrucksSpaceCreator : MonoBehaviour
{
    [Header("Field Settings")]
    [SerializeField] protected Transform _position;
    [SerializeField] protected float _intervalBetweenModels;
    [SerializeField] protected float _distanceBetweenModels;

    [Header("Mover Settings")]
    [SerializeField, Min(1)] private int _capacityMovables = 300;
    [SerializeField, Min(0.1f)] private float _movementSpeed = 35;
    [SerializeField, Min(0.001f)] private float _minSqrDistanceToTargetPosition = 0.001f;

    [Header("Factory Settings")]
    [SerializeField] protected FactorySettings _factorySettings;

    [Header("Filler Settings")]

    [Header("Row Filler Settings")]
    [SerializeField] protected float _frequencyForRowFiller = 0.5f;

    [Header("Cascade Filler Settings")]
    [SerializeField] protected float _frequencyForCascadeFiller = 0.05f;

    [Header("Presenter Factories")]
    [SerializeField] private GreenTruckPresenterFactory _greenTruckPresenterFactory;
    [SerializeField] private OrangeTruckPresenterFactory _orangeTruckPresenterFactory;
    [SerializeField] private PurpleTruckPresenterFactory _purpleTruckPresenterFactory;

    [Header("Gun Factory Settings")]
    [SerializeField] protected FactorySettings _factorySettingsForGunFactory;
    [SerializeField, Range(0.01f, 5)] private float _shotCooldown = 1;

    private GunFactory _gunFactory;

    public TruckSpace CreateTruckSpace(SpaceSettings spaceSettings)
    {
        if (spaceSettings == null)
        {
            throw new ArgumentNullException(nameof(spaceSettings));
        }

        TruckField truckField = CreateField(spaceSettings.WidthField, spaceSettings.LengthField);
        Mover mover = CreateMover(truckField);
        ModelsProduction<Truck, TruckFactory> production = CreateTruckFactory();
        Filler fieldFiller = CreateFiller(truckField);
        PresentersProduction<Truck> presentersProduction = CreatePresentersProduction();
        ModelPresenterBinder binder = CreateModelPresenterBinder(truckField, presentersProduction);
        FillingCardModelCreator fillingCardModelCreator = CreateFillingCardModelCreator(production);

        return new TruckSpace(truckField,
                              mover,
                              production,
                              fieldFiller,
                              binder,
                              fillingCardModelCreator);
    }

    public void Initialize()
    {
        InitializePresenterFactories();
    }

    private void InitializePresenterFactories()
    {
        _greenTruckPresenterFactory.Initialize();
        _orangeTruckPresenterFactory.Initialize();
        _purpleTruckPresenterFactory.Initialize();
    }

    private TruckField CreateField(int width, int length)
    {
        return new TruckField(_position.position,
                              _position.forward,
                              _position.right,
                              _intervalBetweenModels,
                              _distanceBetweenModels,
                              width,
                              length);
    }

    private Mover CreateMover(Field field)
    {
        return new Mover(field,
                         _capacityMovables,
                         _movementSpeed,
                         _minSqrDistanceToTargetPosition);
    }

    private Filler CreateFiller(Field field)
    {
        Filler filler = new Filler(field);
        CastomizeFiller(filler);

        return filler;
    }

    private PresentersProduction<Truck> CreatePresentersProduction()
    {
        PresentersProduction<Truck> production = new PresentersProduction<Truck>();
        CastomizePresentersProduction(production);

        return production;
    }

    private ModelsProduction<Truck, TruckFactory> CreateTruckFactory()
    {
        ModelsProduction<Truck, TruckFactory> production = new ModelsProduction<Truck, TruckFactory>();
        CastomizeModelsProduction(production);

        return production;
    }

    private FillingCardModelCreator CreateFillingCardModelCreator(ModelsProduction<Truck, TruckFactory> production)
    {
        return new FillingCardModelCreator(production);
    }

    private void CastomizeModelsProduction(ModelsProduction<Truck, TruckFactory> production)
    {
        _gunFactory = new GunFactory(_factorySettingsForGunFactory.InitialPoolSize,
                                     _factorySettingsForGunFactory.MaxPoolCapacity);

        production.AddFactory<GreenTruck>(new GreenTruckFactory(_gunFactory,
                                                                _shotCooldown,
                                                                _factorySettings.InitialPoolSize,
                                                                _factorySettings.MaxPoolCapacity));

        production.AddFactory<OrangeTruck>(new OrangeTruckFactory(_gunFactory,
                                                                  _shotCooldown,
                                                                  _factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));

        production.AddFactory<PurpleTruck>(new PurpleTruckFactory(_gunFactory,
                                                                  _shotCooldown,
                                                                  _factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));
    }

    private void CastomizePresentersProduction(PresentersProduction<Truck> production)
    {
        production.AddFactory<GreenTruck>(_greenTruckPresenterFactory);
        production.AddFactory<OrangeTruck>(_orangeTruckPresenterFactory);
        production.AddFactory<PurpleTruck>(_purpleTruckPresenterFactory);
    }

    private void CastomizeFiller(Filler filler)
    {
        filler.AddFillingStrategy(new RowFiller(_frequencyForRowFiller));
        filler.AddFillingStrategy(new CascadeFiller(_frequencyForCascadeFiller));
    }

    private ModelPresenterBinder CreateModelPresenterBinder(Field field,
                                                            PresentersProduction<Truck> presentersProduction)
    {
        return new ModelPresenterBinder(field, presentersProduction);
    }
}