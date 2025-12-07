using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockFieldFiller
{
    private readonly Field _field;
    private readonly FillingStrategy _fillingStrategy;

    private readonly BlockFactory _blockFactory;
    private readonly RowGenerator _rowGenerator;
    private readonly Stopwatch _stopwatch;

    //private bool _isFillingCardEmpty;
    private bool _isNonstopFilling;

    private bool _isSubscribedToFillingStrategy;
    private bool _isSubscribedToStopwatch;

    public BlockFieldFiller(Field field,
                            FillingStrategy fillingStrategy,
                            BlockFactory blockFactory,
                            RowGenerator rowGenerator,
                            Stopwatch stopwatch)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));

        _blockFactory = blockFactory ?? throw new ArgumentNullException(nameof(blockFactory));
        _rowGenerator = rowGenerator ?? throw new ArgumentNullException(nameof(rowGenerator));
        _stopwatch = stopwatch ?? throw new ArgumentNullException(nameof(stopwatch));

        //_isFillingCardEmpty = false;
        _isNonstopFilling = false;

        _isSubscribedToFillingStrategy = false;
    }

    public IReadOnlyList<ColorType> GetColorTypes()
    {
        return _fillingStrategy.GetColorType();
    }

    public void Clear()
    {
        _fillingStrategy.Clear();
    }

    public void Enable()
    {
        if (_isSubscribedToFillingStrategy == false && _isNonstopFilling == false)
        {
            SubscribeToFillingStrategy();
            _fillingStrategy.Enable();
        }
        else if (_isNonstopFilling)
        {
            ActivateStopwatch();
        }
    }

    public void Disable()
    {
        if (_isSubscribedToFillingStrategy)
        {
            _fillingStrategy.Disable();
            UnsubscribeFromFillingStrategy();
        }

        if (_isSubscribedToStopwatch)
        {
            DeactivateStopwatch();
        }
    }

    public void ActivateNonstopFilling()
    {
        _isNonstopFilling = true;
    }

    public void DeactivateNonstopFilling()
    {
        _isNonstopFilling = false;
    }

    private void SubscribeToFillingStrategy()
    {
        _fillingStrategy.FillingCardEmpty += OnFillingFinished;
        _isSubscribedToFillingStrategy = true;
    }

    private void UnsubscribeFromFillingStrategy()
    {
        _fillingStrategy.FillingCardEmpty -= OnFillingFinished;
        _isSubscribedToFillingStrategy = false;
    }

    private void OnFillingFinished()
    {
        Disable();
    }

    private void ActivateStopwatch()
    {
        if (_isSubscribedToStopwatch == false)
        {
            _stopwatch.IntervalPassed += OnIntervalPassed;
            _stopwatch.SetNotificationInterval(1f);
            _stopwatch.Start();
            _isSubscribedToStopwatch = true;
        }
    }

    private void DeactivateStopwatch()
    {
        if (_isSubscribedToStopwatch)
        {
            _stopwatch.Stop();
            _stopwatch.IntervalPassed -= OnIntervalPassed;
            _isSubscribedToStopwatch = false;
        }
    }

    private void OnIntervalPassed()
    {
        List<ColorType> colorTypes = _rowGenerator.Create(_field.AmountColumns);

        for (int i = 0; i < _field.AmountColumns; i++)
        {
            Block block = _blockFactory.Create();

            block.SetColor(colorTypes[i]);

            PlaceModel(block, 0, i);
        }
    }

    private void PlaceModel(Model model, int indexOfLayer, int indexOfColumn)
    {
        PlaceModel(new RecordPlaceableModel(model,
                                            indexOfLayer,
                                            indexOfColumn,
                                            _field.GetAmountModelsInColumn(indexOfLayer, indexOfColumn)));
    }

    private void PlaceModel(RecordPlaceableModel record)
    {
        Vector3 spawnPosition = GetSpawnPosition(record);

        spawnPosition += _field.Position;

        record.PlaceableModel.SetFirstPosition(spawnPosition);

        _field.AddModel(record.PlaceableModel,
                        record.IndexOfLayer,
                        record.IndexOfColumn);
    }

    private Vector3 GetSpawnPosition(RecordPlaceableModel record)
    {
        return _field.Right * record.IndexOfColumn * _field.IntervalBetweenColumns +
               _field.Up * record.IndexOfLayer * _field.IntervalBetweenLayers +
               _field.Forward * _field.AmountRows * _field.IntervalBetweenRows;
    }

    //private void SubscribeToField()
    //{
    //    _field.ModelRemoved += OnModelRemoved;
    //    //_isSubscribedToField = true;
    //}

    //private void UnsubscribeFromField()
    //{
    //    _field.ModelRemoved -= OnModelRemoved;
    //    //_isSubscribedToField = false;
    //}

    //private void OnModelRemoved(int layer, int column, int _)
    //{
    //    //_fillingStrategy.PlaceModel(_blockGenerator.Generate(), layer, column);
    //}
}