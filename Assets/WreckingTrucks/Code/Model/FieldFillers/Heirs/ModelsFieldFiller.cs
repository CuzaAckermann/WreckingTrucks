using System;
using System.Collections.Generic;

public abstract class ModelsFieldFiller<M, MF> where M : Model
                                      where MF : ModelFactory<M>
{
    private ModelsProduction<M, MF> _modelsProduction;
    private Field<M> _modelsField;

    private Queue<M> _models;

    private List<Action> _fillingOptions;
    private int _numberOfCurrentColumn;
    private bool _isIncreasing = true;
    private Action _currentFillingOption;

    private Random _random;

    public ModelsFieldFiller(ModelsProduction<M, MF> modelsProduction,
                             Field<M> modelsField,
                             int startCapacityQueue)
    {
        _modelsProduction = modelsProduction ?? throw new ArgumentNullException(nameof(modelsProduction));
        _modelsField = modelsField ?? throw new ArgumentNullException(nameof(modelsField));
        _models = new Queue<M>(startCapacityQueue);
        _random = new Random();

        _fillingOptions = new List<Action>();
        _numberOfCurrentColumn = 0;

        _fillingOptions.Add(FillRowOfField);
        _fillingOptions.Add(FillByZigZag);
        _fillingOptions.Add(FillByCascade);
    }

    public event Action FillingCompleted;

    public void PutModels()
    {
        _currentFillingOption?.Invoke();
    }

    public void Reset()
    {
        _numberOfCurrentColumn = 0;
    }

    public void Clear()
    {
        _models.Clear();
    }

    public void PrepareModels(LevelSettings level)
    {
        for (int i = 0; i < level.Rows.Count; i++)
        {
            Row row = level.Rows[i];

            for (int j = 0; j < row.Models.Count; j++)
            {
                _models.Enqueue(_modelsProduction.Create(row.Models[j]));
            }
        }

        _currentFillingOption = _fillingOptions[_random.Next(0, _fillingOptions.Count)];
    }

    private void FillRowOfField()
    {
        List<M> models = new List<M>(_modelsField.AmountColumns);

        for (int i = 0; i < _modelsField.AmountColumns; i++)
        {
            models.Add(_models.Dequeue());
        }

        _modelsField.PlaceModels(models);

        if (_models.Count == 0)
        {
            FillingCompleted?.Invoke();
        }
    }

    private void FillByZigZag()
    {
        M model = _models.Dequeue();

        _modelsField.PlaceModel(model, _numberOfCurrentColumn);

        if (_models.Count == 0)
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

            if (_numberOfCurrentColumn == _modelsField.AmountColumns)
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
        M model = _models.Dequeue();

        _modelsField.PlaceModel(model, _numberOfCurrentColumn);

        if (_models.Count == 0)
        {
            FillingCompleted?.Invoke();
        }

        GenerateNumberNextColumn();
    }

    private void GenerateNumberNextColumn()
    {
        _numberOfCurrentColumn = (_numberOfCurrentColumn + 1) % _modelsField.AmountColumns;
    }
}