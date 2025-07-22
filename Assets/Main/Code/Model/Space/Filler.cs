using System;
using System.Collections.Generic;

public class Filler
{
    private readonly IFillable _field;
    private readonly List<FillingStrategy> _fillingStrategies;
    private readonly Stopwatch _stopwatch;
    private readonly Random _random;

    private FillingStrategy _currentStrategy;
    private readonly FillingCard _fillingCard;
    private bool _isFillingWorks;
    private bool _isSubscribed;

    public Filler(IFillable field, Stopwatch stopwatch, FillingCard fillingCard)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _fillingStrategies = new List<FillingStrategy>();
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _random = new Random();
        _fillingCard = fillingCard ?? throw new ArgumentNullException(nameof(fillingCard));
    }

    public void Clear()
    {
        _currentStrategy.Clear();
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

    public void PrepareFilling()
    {
        if (_fillingStrategies.Count == 0)
        {
            throw new InvalidOperationException($"{nameof(_fillingStrategies)} is empty.");
        }

        _currentStrategy = _fillingStrategies[_random.Next(0, _fillingStrategies.Count)];
        _stopwatch.SetNotificationInterval(_currentStrategy.Frequency);
        _currentStrategy.PrepareFilling(_field, _fillingCard);
        _isFillingWorks = true;
        _isSubscribed = false;
    }

    public void Enable()
    {
        if (_isFillingWorks && _isSubscribed == false)
        {
            _currentStrategy.FillingCardEmpty += OnFillingCardEmpty;
            _stopwatch.IntervalPassed += OnIntervalPassed;

            _stopwatch.Start();
            _isSubscribed = true;
        }
    }

    public void Disable()
    {
        if (_isSubscribed)
        {
            _stopwatch.Stop();

            _currentStrategy.FillingCardEmpty -= OnFillingCardEmpty;
            _stopwatch.IntervalPassed -= OnIntervalPassed;
            _isSubscribed = false;
        }
    }

    public void PlaceModel(Model model, int indexOfLayer, int indexOfColumn)
    {
        _currentStrategy.PlaceModel(model, indexOfLayer, indexOfColumn);
    }

    private void OnIntervalPassed()
    {
        _currentStrategy.ExecuteFillingStep();
    }

    private void OnFillingCardEmpty()
    {
        Disable();
        _isFillingWorks = false;
    }
}