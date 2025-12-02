using System;
using System.Collections.Generic;

public class BlockFieldFiller
{
    private readonly FillingStrategy _fillingStrategy;
    //private readonly ModelGenerator<Block> _blockGenerator;
    private readonly Field _field;

    private bool _isFillingCardEmpty;
    private bool _isNonstopFilling;

    private bool _isSubscribedToFillingStrategy;
    //private bool _isSubscribedToField;

    public BlockFieldFiller(Field field, FillingStrategy fillingStrategy/*, ModelGenerator<Block> blockGenerator*/)
    {
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));
        //_blockGenerator = blockGenerator ?? throw new ArgumentNullException(nameof(blockGenerator));
        _field = field ?? throw new ArgumentNullException(nameof(field));

        _isFillingCardEmpty = false;
        _isNonstopFilling = false;

        _isSubscribedToFillingStrategy = false;
        //_isSubscribedToField = false;
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
        if (_isFillingCardEmpty == false && _isSubscribedToFillingStrategy == false)
        {
            SubscribeToFillingStrategy();
            _fillingStrategy.Enable();
        }
        else if (_isNonstopFilling && _isFillingCardEmpty)
        {
            SubscribeToField();
        }
    }

    public void Disable()
    {
        if (_isSubscribedToFillingStrategy)
        {
            _fillingStrategy.Disable();
            UnsubscribeFromFillingStrategy();
        }
        //else if (_isSubscribedToField)
        //{
        //    UnsubscribeFromField();
        //}
    }

    //public void ActivateNonstopFilling()
    //{
    //    _isNonstopFilling = true;
    //}

    //public void DeactivateNonstopFilling()
    //{
    //    _isNonstopFilling = false;
    //}

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

        if (_isNonstopFilling)
        {
            SubscribeToField();
            _isFillingCardEmpty = true;
        }
    }

    private void SubscribeToField()
    {
        _field.ModelRemoved += OnModelRemoved;
        //_isSubscribedToField = true;
    }

    private void UnsubscribeFromField()
    {
        _field.ModelRemoved -= OnModelRemoved;
        //_isSubscribedToField = false;
    }

    private void OnModelRemoved(int layer, int column, int _)
    {
        //_fillingStrategy.PlaceModel(_blockGenerator.Generate(), layer, column);
    }
}