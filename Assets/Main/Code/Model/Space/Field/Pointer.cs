using System;

public class Pointer
{
    private readonly bool _isIncreased;
    private readonly int _min = 0;
    private readonly int _max;

    public Pointer(int startCurrent, int max)
    {
        if (startCurrent < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startCurrent));
        }

        if (max < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(max));
        }

        _max = max;

        if (startCurrent == _min)
        {
            Current = _min;
            _isIncreased = true;
        }
        else if (startCurrent == _max)
        {
            Current = _max;
            _isIncreased = false;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(startCurrent));
        }
    }

    public int Current { get; private set; }

    public bool TryShift()
    {
        if (_isIncreased)
        {
            return TryIncrease();
        }
        else
        {
            return TryDecrease();
        }
    }

    private bool TryIncrease()
    {
        Current++;

        if (Current > _max)
        {
            Current = _min;

            return false;
        }

        return true;
    }

    private bool TryDecrease()
    {
        Current--;

        if (Current < _min)
        {
            Current = _max;

            return false;
        }

        return true;
    }
}