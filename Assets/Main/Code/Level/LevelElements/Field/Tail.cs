using System;

public class Tail
{
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

    public void Shift()
    {
        if (_column.TryShift())
        {
            return;
        }

        if (_layer.TryShift())
        {
            return;
        }

        _row.TryShift();
    }

    public void DecreaseCurrentRow()
    {
        _row.TryDecrease();
    }

    public void IncreaseCurrentRow()
    {
        _row.TryIncrease();

        _layer.Reset();
        _column.Reset();
    }

    public void ResetAll()
    {
        _layer.Reset();
        _column.Reset();
        _row.Reset();
    }
}