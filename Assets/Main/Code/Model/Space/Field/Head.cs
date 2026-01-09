using System;
using System.Collections.Generic;
using UnityEngine;

public class Head
{
    private readonly Pointer _layer;
    private readonly Pointer _column;

    public Head(Pointer layer, Pointer column)
    {
        _layer = layer ?? throw new ArgumentNullException(nameof(layer));
        _column = column ?? throw new ArgumentNullException(nameof(column));
    }

    public int CurrentLayer => _layer.Current;

    public int CurrentColumn => _column.Current;

    public bool TryShift()
    {
        if (_column.TryShift())
        {
            return true;
        }

        if (_layer.TryShift())
        {
            return true;
        }

        return false;
    }
}