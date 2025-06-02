using System;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private List<Vector3> _positions;

    public Path(List<Vector3> positions)
    {
        _positions = positions ?? throw new ArgumentNullException(nameof(positions));
    }
}