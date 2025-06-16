using System;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private readonly List<Vector3> _positions;

    public Path(List<Vector3> positions)
    {
        _positions = positions ?? throw new ArgumentNullException(nameof(positions));
    }

    public Vector3 GetFirstPosition()
    {
        return _positions[0];
    }

    public bool TryGetNextPosition(Vector3 currentPosition, out Vector3 nextPosition)
    {
        nextPosition = currentPosition;

        for (int i = 0; i < _positions.Count - 1; i++)
        {
            if (currentPosition == _positions[i])
            {
                nextPosition = _positions[i + 1];

                return true;
            }
        }

        return false;
    }
}