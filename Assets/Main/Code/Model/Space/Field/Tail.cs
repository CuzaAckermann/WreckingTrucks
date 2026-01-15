using System;
using System.Collections.Generic;
using UnityEngine;

public class Tail
{
    private const int MinAmountRows = 0;

    private readonly IndexPointer _layer;
    private readonly IndexPointer _column;
    private readonly IndexPointer _row;

    public Tail(IndexPointer layer, IndexPointer column, IndexPointer row)
    {
        _layer = layer ?? throw new ArgumentNullException(nameof(layer));
        _column = column ?? throw new ArgumentNullException(nameof(column));
        _row = row ?? throw new ArgumentNullException(nameof(row));
    }

    public int CurrentLayer => _layer.Current;

    public int CurrentColumn => _column.Current;

    public int CurrentRow => _row.Current;

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

        _row.TryShift();

        return false;
    }

    public void DecreaseCurrentRow()
    {
        if (_row.Current > MinAmountRows)
        {
            _row.TryDecrease();
        }
    }

    public void IncreaseCurrentRow()
    {
        _row.TryIncrease();

        Reset();
    }

    public void Reset()
    {
        _layer.Reset();
        _column.Reset();
    }
}