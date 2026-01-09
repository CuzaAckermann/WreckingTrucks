using System;

public class Pointer
{
    private readonly bool _isIncreased;
    private readonly int _min;
    private readonly int _max;

    public Pointer(int startCurrent, int min, int max, bool isIncreased)
    {
        //if (startCurrent < 0)
        //{
        //    throw new ArgumentOutOfRangeException(nameof(startCurrent));
        //}

        if (min < 0 || min >= max)
        {
            throw new ArgumentOutOfRangeException(nameof(min));
        }

        if (max < 0 || max <= min)
        {
            throw new ArgumentOutOfRangeException(nameof(max));
        }

        if (startCurrent > max && isIncreased)
        {
            throw new ArgumentOutOfRangeException(nameof(startCurrent));
        }

        if (startCurrent < min && isIncreased == false)
        {
            throw new ArgumentOutOfRangeException(nameof(startCurrent));
        }

        _min = min;
        _max = max;
        Current = startCurrent;
        _isIncreased = isIncreased;

        //if (startCurrent == _min)
        //{
        //    Current = _min;
        //    _isIncreased = true;
        //}
        //else if (startCurrent == _max)
        //{
        //    Current = _max;
        //    _isIncreased = false;
        //}
        //else
        //{
        //    throw new ArgumentOutOfRangeException(nameof(startCurrent));
        //}
    }

    public int Current { get; private set; }

    public void Reset()
    {
        if (_isIncreased)
        {
            Current = _min;
        }
        else
        {
            Current = _max;
        }
    }

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

    public bool TryDecrease()
    {
        Current--;

        if (Current < _min)
        {
            Current = _max;

            return false;
        }

        return true;
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
}