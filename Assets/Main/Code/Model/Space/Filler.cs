using System;
using System.Collections.Generic;

public class Filler
{
    private readonly IFillable _field;
    private readonly List<FillingStrategy> _fillingStrategies;
    private readonly Random _random;
    private readonly Stopwatch _stopwatch;

    private FillingStrategy _currentStrategy;

    public Filler(IFillable field, Stopwatch stopwatch)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _fillingStrategies = new List<FillingStrategy>();
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));
        _random = new Random();
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

    public void PrepareFilling(FillingCard fillingCard)
    {
        if (_fillingStrategies.Count == 0)
        {
            throw new InvalidOperationException($"{nameof(_fillingStrategies)} is empty.");
        }

        _currentStrategy = _fillingStrategies[_random.Next(0, _fillingStrategies.Count)];
        _stopwatch.SetNotificationInterval(_currentStrategy.Frequency);
        _currentStrategy.PrepareFilling(_field, fillingCard);
    }

    public void Enable()
    {
        _currentStrategy.FillingCardEmpty += OnFillingCardEmpty;
        _stopwatch.IntervalPassed += OnIntervalPassed;

        _stopwatch.Start();
    }

    public void Disable()
    {
        // ƒŒ¡¿¬»“‹ œ–Œ¬≈– ”, –¿¡Œ“¿≈“ À» ≈Ÿ≈ “¿…Ã≈–

        _stopwatch.Stop();

        _currentStrategy.FillingCardEmpty -= OnFillingCardEmpty;
        _stopwatch.IntervalPassed -= OnIntervalPassed;
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
    }
}