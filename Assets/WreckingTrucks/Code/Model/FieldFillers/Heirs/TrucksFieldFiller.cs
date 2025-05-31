using System;
using System.Collections.Generic;

public class TrucksFieldFiller
{
    private TrucksProduction _trucksProduction;
    private TrucksField _trucksField;

    private Queue<Truck> _trucks;

    private List<Action> _fillingOptions;
    private int _numberOfCurrentColumn;
    private bool _isIncreasing = true;
    private Action _currentFillingOption;

    private Random _random;

    public TrucksFieldFiller(TrucksProduction trucksFactory,
                             TrucksField trucksField,
                             int startCapacityQueue)
    {
        _trucksProduction = trucksFactory ?? throw new ArgumentNullException(nameof(trucksFactory));
        _trucksField = trucksField ?? throw new ArgumentNullException(nameof(trucksField));
        _trucks = new Queue<Truck>(startCapacityQueue);
        _random = new Random();

        _fillingOptions = new List<Action>();
        _numberOfCurrentColumn = 0;

        _fillingOptions.Add(FillRowOfField);
        _fillingOptions.Add(FillByZigZag);
        _fillingOptions.Add(FillByCascade);
    }

    public event Action FillingCompleted;

    public void PutTrucks()
    {
        _currentFillingOption?.Invoke();
    }

    public void Reset()
    {
        _numberOfCurrentColumn = 0;
    }

    public void Clear()
    {
        _trucks.Clear();
    }

    public void PrepareBlocks(Level level)
    {
        for (int i = 0; i < level.Rows.Count; i++)
        {
            Row row = level.Rows[i];

            for (int j = 0; j < row.Blocks.Count; j++)
            {
                _trucks.Enqueue(_trucksProduction.Create(row.Blocks[j]));
            }
        }

        _currentFillingOption = _fillingOptions[_random.Next(0, _fillingOptions.Count)];
    }

    private void FillRowOfField()
    {
        List<Truck> trucks = new List<Truck>(_trucksField.AmountColumns);

        for (int i = 0; i < _trucksField.AmountColumns; i++)
        {
            trucks.Add(_trucks.Dequeue());
        }

        _trucksField.PlaceModels(trucks);

        if (_trucks.Count == 0)
        {
            FillingCompleted?.Invoke();
        }
    }

    private void FillByZigZag()
    {
        Truck truck = _trucks.Dequeue();

        _trucksField.PlaceModel(truck, _numberOfCurrentColumn);

        if (_trucks.Count == 0)
        {
            FillingCompleted?.Invoke();
        }

        GenerateNumberNextColumnZigzag();
    }

    private void GenerateNumberNextColumnZigzag()
    {
        if (_isIncreasing)
        {
            _numberOfCurrentColumn++;

            if (_numberOfCurrentColumn == _trucksField.AmountColumns)
            {
                _numberOfCurrentColumn--;
                _isIncreasing = false;
            }
        }
        else
        {
            _numberOfCurrentColumn--;

            if (_numberOfCurrentColumn < 0)
            {
                _numberOfCurrentColumn++;
                _isIncreasing = true;
            }
        }
    }

    private void FillByCascade()
    {
        Truck truck = _trucks.Dequeue();

        _trucksField.PlaceModel(truck, _numberOfCurrentColumn);

        if (_trucks.Count == 0)
        {
            FillingCompleted?.Invoke();
        }

        GenerateNumberNextColumn();
    }

    private void GenerateNumberNextColumn()
    {
        _numberOfCurrentColumn = (_numberOfCurrentColumn + 1) % _trucksField.AmountColumns;
    }
}