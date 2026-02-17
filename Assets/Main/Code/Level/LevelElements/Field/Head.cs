using System;
using System.Collections.Generic;
using UnityEngine;

public class Head
{
    private readonly IndexPointer _layer;
    private readonly IndexPointer _column;

    public Head(IndexPointer layer, IndexPointer column)
    {
        _layer = layer ?? throw new ArgumentNullException(nameof(layer));
        _column = column ?? throw new ArgumentNullException(nameof(column));
    }

    public int CurrentLayer => _layer.Current;

    public int CurrentColumn => _column.Current;

    public void Reset()
    {
        _layer.Reset();
        _column.Reset();
    }

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