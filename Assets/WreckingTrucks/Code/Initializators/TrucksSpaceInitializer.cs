using UnityEngine;

public class TrucksSpaceInitializer : SpaceInitializer<Truck, TruckFactory>
{
    [Header("Presenter Factories")]
    [SerializeField] private GreenTruckPresenterFactory _greenTruckPresenterFactory;
    [SerializeField] private OrangeTruckPresenterFactory _orangeTruckPresenterFactory;
    [SerializeField] private PurpleTruckPresenterFactory _purpleTruckPresenterFactory;

    protected override ModelsProduction<Truck, TruckFactory> CreateModelsProduction()
    {
        TrucksProduction production = new TrucksProduction();

        production.AddFactory<GreenTruck>(new GreenTruckFactory(_factorySettings.InitialPoolSize,
                                                                _factorySettings.MaxPoolCapacity));

        production.AddFactory<OrangeTruck>(new OrangeTruckFactory(_factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));

        production.AddFactory<PurpleTruck>(new PurpleTruckFactory(_factorySettings.InitialPoolSize,
                                                                  _factorySettings.MaxPoolCapacity));

        return production;
    }

    protected override Field CreateField(LevelSettings levelSettings)
    {
        return new Field(_position.position,
                        _position.forward,
                        _position.right,
                        _intervalBetweenModels,
                        _distanceBetweenModels,
                        levelSettings.WidthTrucksField,
                        levelSettings.LengthTrucksField);
    }

    protected override PresentersProduction<Truck> CreatePresentersProduction()
    {
        InitializePresenterFactories();

        PresentersProduction<Truck> production = new PresentersProduction<Truck>();

        production.AddFactory<GreenTruck>(_greenTruckPresenterFactory);
        production.AddFactory<OrangeTruck>(_orangeTruckPresenterFactory);
        production.AddFactory<PurpleTruck>(_purpleTruckPresenterFactory);

        return production;
    }

    protected override void InitializePresenterFactories()
    {
        _greenTruckPresenterFactory.Initialize();
        _orangeTruckPresenterFactory.Initialize();
        _purpleTruckPresenterFactory.Initialize();
    }
}