using System;
using System.Collections.Generic;

public class Filler : ITickable
{
    private readonly IFillable _field;
    private readonly List<FillingStrategy> _fillingStrategies;
    private FillingStrategy _currentStrategy;
    private Random _random;

    public Filler(IFillable field)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _fillingStrategies = new List<FillingStrategy>();
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

    public void StartFilling(FillingCard<Model> fillingCard)
    {
        _currentStrategy = _fillingStrategies[_random.Next(0, _fillingStrategies.Count)];
        _currentStrategy.StartFilling(_field, fillingCard);
    }

    public void Tick(float deltaTime)
    {
        _currentStrategy.Tick(deltaTime);
    }
}