using System;
using System.Collections.Generic;

public abstract class RowWithSeveralTypesGenerator : RowGenerationStrategy
{
    private readonly int _amountTypes;
    private int _remainingModels;
    private int _typesLeft;
    private bool _isInitialized;

    public RowWithSeveralTypesGenerator(int amountTypes)
    {
        if (amountTypes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountTypes));
        }

        _amountTypes = amountTypes;
    }

    public override List<ColorType> Generate(List<ColorType> differentTypes, int amountModels)
    {
        ValidateInput(differentTypes, amountModels);

        List<ColorType> elements = new List<ColorType>(amountModels);
        List<ColorType> availableTypes = new List<ColorType>(differentTypes);

        InitializeState(amountModels);

        while (_typesLeft > 0 && elements.Count < amountModels)
        {
            ColorType selectedType = availableTypes[Random.Next(0, availableTypes.Count)];
            int period = GetPeriod();

            AddElements(elements, selectedType, Math.Min(period, amountModels - elements.Count));
            availableTypes.Remove(selectedType);
        }

        return elements;
    }

    protected abstract int CalculatePeriod(int remainingModels, int typesLeft);

    private void InitializeState(int amountModels)
    {
        if (_isInitialized == false)
        {
            _remainingModels = amountModels;
            _typesLeft = _amountTypes;
            _isInitialized = true;
        }
    }

    private void AddElements(List<ColorType> elements, ColorType type, int count)
    {
        for (int i = 0; i < count; i++)
        {
            elements.Add(type);
        }
    }

    private int GetPeriod()
    {
        if (_typesLeft == 1)
        {
            int lastPeriod = _remainingModels;
            ResetState();

            return lastPeriod;
        }

        int period = CalculatePeriod(_remainingModels, _typesLeft);
        _remainingModels -= period;
        _typesLeft--;

        return period;
    }

    private void ResetState()
    {
        _remainingModels = 0;
        _typesLeft = 0;
        _isInitialized = false;
    }
}