using System;
using System.Collections.Generic;

public class Road : IModelPositionObserver
{
    private readonly BezierCurve _mainPath;
    private readonly StorageTemporaryCurves _storageTemporaryCurves;

    private readonly List<Truck> _movableTrucks;

    private readonly Dictionary<Truck, int> _truckToCurrentPoint;

    public Road(BezierCurve mainPath, BezierCurveSettings settings)
    {
        _mainPath = mainPath ? mainPath : throw new ArgumentNullException(nameof(mainPath));

        if (_mainPath.TryGetFirstNode(out BezierNode node) == false)
        {
            throw new InvalidOperationException($"{nameof(BezierNode)} was not found");
        }

        _storageTemporaryCurves = new StorageTemporaryCurves(settings, node);
        _movableTrucks = new List<Truck>();
        _truckToCurrentPoint = new Dictionary<Truck, int>();
    }

    public event Action<Truck> TruckReachedEnd;
    public event Action<Model> PositionChanged;
    public event Action<Model> PositionReached;
    public event Action<IModel> InterfacePositionChanged;
    public event Action<List<Model>> PositionsChanged;
    public event Action<List<IModel>> InterfacePositionsChanged;

    public IReadOnlyList<Truck> MovableTrucks => _movableTrucks;

    public void Clear()
    {
        for (int i = _movableTrucks.Count - 1; i >= 0; i--)
        {
            UnsubscribeFromTruck(_movableTrucks[i]);
        }

        _movableTrucks.Clear();
    }

    public void Prepare(Field truckField)
    {
        _storageTemporaryCurves.CalculateCurves(truckField);
    }

    public void AddTruck(Truck truck, int indexOfColumn)
    {
        int indexOfStartPoint = 0;
        SubscribeToTruck(truck);
        truck.SetTargetPosition(_mainPath.CurvePoints[indexOfStartPoint]);
        truck.SetTargetRotation(_mainPath.CurvePoints[indexOfStartPoint]);
        _truckToCurrentPoint[truck] = indexOfStartPoint;
        _movableTrucks.Add(truck);
        PositionChanged?.Invoke(truck);
    }

    private void SubscribeToTruck(Model model)
    {
        model.Destroyed += UnsubscribeFromTruck;
        model.TargetPositionReached += UpdateTruck;
    }

    private void UnsubscribeFromTruck(Model model)
    {
        model.Destroyed -= UnsubscribeFromTruck;
        model.TargetPositionReached -= UpdateTruck;
    }

    // обработчик события Truck'a когда он достигнет целевой позиции
    private void UpdateTruck(Model model)
    {
        PositionReached?.Invoke(model);

        if (model is Truck truck)
        {
            _truckToCurrentPoint[truck]++;

            if (_truckToCurrentPoint[truck] < _mainPath.CurvePoints.Count)
            {
                truck.SetTargetPosition(_mainPath.CurvePoints[_truckToCurrentPoint[truck]]);
                truck.SetTargetRotation(_mainPath.CurvePoints[_truckToCurrentPoint[truck]]);
                PositionChanged?.Invoke(truck);
            }
            else
            {
                FinishMovement(truck);
            }
        }
    }

    // удаляем при уничтожении объекта или при достижении конца пути
    private void RemoveTruck(Truck truck)
    {
        UnsubscribeFromTruck(truck);
        _movableTrucks.Remove(truck);
    }

    private void FinishMovement(Truck truck)
    {
        RemoveTruck(truck);
        TruckReachedEnd?.Invoke(truck);
    }
}