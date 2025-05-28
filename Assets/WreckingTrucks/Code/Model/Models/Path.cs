using System;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public Path(IReadOnlyList<Vector3> positions)
    {
        Positions = positions ?? throw new ArgumentNullException(nameof(positions));
    }

    public IReadOnlyList<Vector3> Positions { get; private set; }
}