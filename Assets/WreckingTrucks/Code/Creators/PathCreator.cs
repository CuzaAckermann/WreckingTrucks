using System;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [SerializeField] private List<Transform> _transform;
    [SerializeField, Min(0)] private int _indexStartOfShooting;

    public Path CreatePath()
    {
        if (_indexStartOfShooting < 0 && _indexStartOfShooting >= _transform.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(_indexStartOfShooting));
        }

        List<CheckPoint> positions = new List<CheckPoint>();

        for (int i = 0; i < _transform.Count; i++)
        {
            CheckPoint checkPoint = new CheckPoint(_transform[i].position);

            if (i == _indexStartOfShooting)
            {
                checkPoint.StayStarOfShooting();
            }

            positions.Add(checkPoint);
        }

        return new Path(positions);
    }
}