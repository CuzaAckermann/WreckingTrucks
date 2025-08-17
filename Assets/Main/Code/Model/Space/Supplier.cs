using System;
using System.Collections.Generic;

public class Supplier : IModelPositionObserver
{
    private readonly List<CartrigeBox> _cartrigeBoxes;

    public Supplier()
    {
        _cartrigeBoxes = new List<CartrigeBox>();
    }



    public event Action<Model> ModelPositionChanged;

    public event Action<Model> PositionReached;

    public event Action<IModel> InterfacePositionChanged;

    public event Action<List<Model>> PositionsChanged;

    public event Action<List<IModel>> InterfacePositionsChanged;



    public IReadOnlyList<CartrigeBox> CartrigeBoxes => _cartrigeBoxes;

    public void Clear()
    {
        _cartrigeBoxes.Clear();
    }

    public void AddCartrigeBox(CartrigeBox cartrigeBox)
    {
        if (cartrigeBox == null)
        {
            throw new ArgumentNullException(nameof(cartrigeBox));
        }

        if (_cartrigeBoxes.Contains(cartrigeBox))
        {
            throw new InvalidOperationException($"{nameof(cartrigeBox)} is added");
        }

        SubscribeToCartrigeBox(cartrigeBox);

        ModelPositionChanged?.Invoke(cartrigeBox);
    }

    private void SubscribeToCartrigeBox(CartrigeBox cartrigeBox)
    {
        cartrigeBox.Destroyed += OnDestroyed;
        _cartrigeBoxes.Add(cartrigeBox);
    }

    private void OnDestroyed(Model model)
    {
        model.Destroyed -= OnDestroyed;

        if (model is CartrigeBox cartrigeBox)
        {
            _cartrigeBoxes.Remove(cartrigeBox);
        }
    }
}