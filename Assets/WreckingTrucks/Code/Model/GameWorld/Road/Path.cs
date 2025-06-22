using System;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private readonly List<CheckPoint> _checkPoints;

    public Path(List<CheckPoint> checkPoints)
    {
        _checkPoints = checkPoints ?? throw new ArgumentNullException(nameof(checkPoints));
    }

    public CheckPoint GetFirstCheckPoint()
    {
        return _checkPoints[0];
    }

    public bool TryGetNextCheckPoint(CheckPoint current, out CheckPoint next)
    {
        next = current;

        for (int i = 0; i < _checkPoints.Count - 1; i++)
        {
            if (current == _checkPoints[i])
            {
                next = _checkPoints[i + 1];

                return true;
            }
        }

        return false;
    }
}