using System;

public class IndexPointer
{
    private readonly bool _isIncreased;
    private readonly int _min;
    private readonly int _max;

    public IndexPointer(int startCurrent, int min, int max, bool isIncreased)
    {
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

    public bool TryIncrease()
    {
        Current++;

        if (Current > _max)
        {
            if (_isIncreased)
            {
                Current = _min;

                return false;
            }
            else
            {
                Current = _max;

                return false;
            }
        }

        return true;
    }

    public bool TryDecrease()
    {
        Current--;

        if (Current < _min)
        {
            if (_isIncreased == false)
            {
                Current = _max;

                return false;
            }
            else
            {
                Current = _min;

                return false;
            }
        }

        return true;
    }
}