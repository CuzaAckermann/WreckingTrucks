using System;
using System.Collections.Generic;

public class FieldFiller : ITickable
{
    private IFillable _field;

    private List<FillingStrategy> _fillingStrategies;
    private FillingStrategy _currentStrategy;
    private float _timeLastFill;
    private Random _random;
    private bool _isFilling;

    public FieldFiller(IFillable field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _fillingStrategies = new List<FillingStrategy>();
        _random = new Random();
        _isFilling = false;
    }

    public void AddFillingStrategy(FillingStrategy strategy)
    {
        if (strategy == null)
        {
            throw new ArgumentNullException(nameof(strategy));
        }

        if (_fillingStrategies.Contains(strategy))
        {
            throw new InvalidOperationException($"{strategy.GetType().Name} already exists.");
        }

        _fillingStrategies.Add(strategy);
    }

    public void StartFilling(Queue<Model> models)
    {
        _currentStrategy = _fillingStrategies[_random.Next(0, _fillingStrategies.Count)];
        _currentStrategy.SetQueueModels(models);
        _timeLastFill = 0;
        _isFilling = true;
        _currentStrategy.FillingCompleted += OnFillingCompleted;
    }

    public void Tick(float deltaTime)
    {
        if (_isFilling == false)
        {
            return;
        }

        _timeLastFill += deltaTime;

        if (_timeLastFill >= _currentStrategy.Frequency)
        {
            _currentStrategy.FillStep();
            _timeLastFill -= _currentStrategy.Frequency;
        }
    }

    public void Clear()
    {
        _currentStrategy.Clear();
    }

    private void OnFillingCompleted()
    {
        _currentStrategy.FillingCompleted -= OnFillingCompleted;
        _isFilling = false;
    }
}