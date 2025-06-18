using System;

public class TruckSpace
{
    private readonly TruckField _truckField;
    private readonly Mover _mover;
    private readonly IModelsProduction _modelsProduction;
    private readonly Filler _fieldFiller;
    private readonly ModelPresenterBinder _binderModelToModelPresenter;
    private readonly FillingCardModelCreator _fillingCardModelCreator;

    private readonly TickEngine _tickEngine;
    private Generator<Truck> _truckGenerator;

    public TruckSpace(TruckField field,
                      Mover mover,
                      IModelsProduction modelsProduction,
                      Filler fieldFiller,
                      ModelPresenterBinder binderModelToModelPresenter,
                      FillingCardModelCreator fillingCardModelCreator)
    {
        _truckField = field ?? throw new ArgumentNullException(nameof(field));
        _mover = mover ?? throw new ArgumentNullException(nameof(mover));
        _modelsProduction = modelsProduction ?? throw new ArgumentNullException(nameof(modelsProduction));
        _fieldFiller = fieldFiller ?? throw new ArgumentNullException(nameof(fieldFiller));
        _binderModelToModelPresenter = binderModelToModelPresenter ?? throw new ArgumentNullException(nameof(binderModelToModelPresenter));
        _fillingCardModelCreator = fillingCardModelCreator ?? throw new ArgumentNullException(nameof(fillingCardModelCreator));
        
        _tickEngine = new TickEngine();
        InitializeTrucksGenerator();
    }

    public TruckField TruckField => _truckField;

    public void Clear()
    {
        _truckField.Clear();
        _mover.Clear();
        _fieldFiller.Clear();
    }

    public void Prepare(SpaceSettings spaceSettings)
    {
        _fieldFiller.StartFilling(_fillingCardModelCreator.CreateFillingCard(spaceSettings.FillingCard));

        _tickEngine.AddTickable(_mover);
        _tickEngine.AddTickable(_fieldFiller);
    }

    public void Start()
    {
        _truckField.TruckRemoved += OnTruckRemoved;
        _binderModelToModelPresenter.Enable();
        _mover.Enable();

        _tickEngine.Continue();

    }

    public void Update(float deltaTime)
    {
        _tickEngine.Tick(deltaTime);
    }

    public void Stop()
    {
        _tickEngine.Pause();

        _binderModelToModelPresenter.Disable();
        _mover.Disable();
        _truckField.TruckRemoved -= OnTruckRemoved;
    }

    public bool TryRemoveTruck(Truck truck)
    {
        return _truckField.TryRemoveTruck(truck);
    }

    private void OnTruckRemoved(int numberOfColumn)
    {
        Type truckType = _truckGenerator.GenerateTypeModel();
        Model truck = _modelsProduction.CreateModel(truckType);
        _fieldFiller.PlaceModel(truck, numberOfColumn);
    }

    private void InitializeTrucksGenerator()
    {
        _truckGenerator = new Generator<Truck>();

        _truckGenerator.AddType<GreenTruck>();
        _truckGenerator.AddType<OrangeTruck>();
        _truckGenerator.AddType<PurpleTruck>();

        _truckGenerator.AddGenerator(new RowWithRandomTypesGenerator());
    }
}