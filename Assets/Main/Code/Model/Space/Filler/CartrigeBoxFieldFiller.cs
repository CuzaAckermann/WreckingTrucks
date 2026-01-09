using System;
using UnityEngine;

public class CartrigeBoxFieldFiller
{
    private readonly FillingStrategy<CartrigeBox> _fillingStrategy;

    private readonly FillingState<CartrigeBox> _fillingState;

    private bool _isFillingCardEmpty;

    public CartrigeBoxFieldFiller(FillingStrategy<CartrigeBox> fillingStrategy)
    {
        _fillingStrategy = fillingStrategy ?? throw new ArgumentNullException(nameof(fillingStrategy));

        _fillingState = new FillingState<CartrigeBox>(_fillingStrategy);

        _isFillingCardEmpty = false;
    }

    public void Clear()
    {
        _fillingStrategy.Clear();
    }

    public void Enable()
    {
        if (_isFillingCardEmpty == false)
        {
            _fillingState.Enter(OnFillingFinished);
        }
    }

    public void Disable()
    {
        _fillingState.Exit();
    }

    private void OnFillingFinished()
    {
        _fillingState.Exit();

        _isFillingCardEmpty = true;
    }
}