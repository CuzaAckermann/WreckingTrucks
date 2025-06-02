using System;
using UnityEngine;

public class TrucksSpace : MonoBehaviour
{
    [Header("Settings Field")]
    [SerializeField] private Transform _position;
    [SerializeField] private Vector3 _columnDirection = Vector3.back;
    [SerializeField] private Vector3 _rowDirection = Vector3.right;
    [SerializeField, Min(1)] private int _amountColumns = 3;
    [SerializeField, Min(1)] private int _capacityColumn = 5;
    [SerializeField] private int _spawnPosition = 5;

    [Header("Settings FieldFiller")]
    [SerializeField] private int _startCapacityForFieldFiller = 100;

    [Header("Settings Stopwatch for FieldFiller")]
    [SerializeField] private float _notificationIntervalForFieldFiller = 0.5f;

    [Header("Settings Level Generation")]
    [SerializeField, Min(1)] private int _amountRows = 3;

    [Header("PathPresenter")]
    [SerializeField] private PathPresenter _pathPresenter;

    private TrucksField _trucksField;
    private TrucksFieldFiller _trucksFieldFiller;
    private TrucksProduction _trucksProduction;
    private PresentersProduction<Truck> _truckPresentersProduction;
    private Stopwatch _stopwatchForTrucksFieldFiller;
    private Generator<Truck> _truckGenerator;
    private ITickEngineUpdaterOnlyAddAndRemove _tickEngineUpdater;
    private Path _path;

    public void Initialize(TrucksProduction trucksProduction,
                           PresentersProduction<Truck> truckPresenterProduction,
                           ITickEngineUpdaterOnlyAddAndRemove tickEngineUpdater,
                           Mover<Truck> trucksMover)
    {
        _trucksProduction = trucksProduction ?? throw new ArgumentNullException(nameof(trucksProduction));
        _truckPresentersProduction = truckPresenterProduction ?? throw new ArgumentNullException(nameof(truckPresenterProduction));
        _tickEngineUpdater = tickEngineUpdater ?? throw new ArgumentNullException(nameof(tickEngineUpdater));

        InitializeTrucksGenerator();
        InitializeTrucksField(trucksMover, _trucksProduction);

        _pathPresenter.Initialize();
        _path = new Path(_pathPresenter.Positions);
    }

    public void Reset()
    {
        _trucksFieldFiller.Reset();
        _trucksField.Reset();
        _stopwatchForTrucksFieldFiller.Reset();
    }

    public void Clear()
    {
        _stopwatchForTrucksFieldFiller.Stop();

        _trucksFieldFiller.Clear();
        _trucksField.Clear();

        _tickEngineUpdater.Remove(_stopwatchForTrucksFieldFiller);
    }
    
    public void PrepareFields()
    {
        _trucksFieldFiller.PrepareModels(new LevelSettings(_truckGenerator.GetRows(_amountRows, _amountColumns)));

        _tickEngineUpdater.Add(_stopwatchForTrucksFieldFiller);
    }

    public void StartLevel()
    {
        _stopwatchForTrucksFieldFiller.Start();
    }

    private void InitializeTrucksField(Mover<Truck> trucksMover, TrucksProduction trucksProduction)
    {
        _trucksField = new TrucksField(_position.position,
                                       _columnDirection,
                                       _rowDirection,
                                       _amountColumns,
                                       _capacityColumn,
                                       _spawnPosition,
                                       trucksMover);

        _stopwatchForTrucksFieldFiller = new Stopwatch(_notificationIntervalForFieldFiller);

        _trucksFieldFiller = new TrucksFieldFiller(trucksProduction,
                                                   _trucksField,
                                                   _startCapacityForFieldFiller);

        _trucksField.ModelTaken += OnTruckTaken;
        _stopwatchForTrucksFieldFiller.IntervalPassed += _trucksFieldFiller.PutModels;
        _trucksFieldFiller.FillingCompleted += OnTrucksFieldFilled;
    }

    private void InitializeTrucksGenerator()
    {
        _truckGenerator = new Generator<Truck>();

        _truckGenerator.AddType<GreenTruck>();
        _truckGenerator.AddType<OrangeTruck>();
        _truckGenerator.AddType<PurpleTruck>();

        _truckGenerator.AddGenerator(new RowWithRandomTypesGenerator());
    }

    private void OnTruckTaken(Truck truck)
    {
        _truckPresentersProduction.CreatePresenter(truck).Initialize(truck);
    }

    private void OnTrucksFieldFilled()
    {
        _stopwatchForTrucksFieldFiller.Stop();
    }
}