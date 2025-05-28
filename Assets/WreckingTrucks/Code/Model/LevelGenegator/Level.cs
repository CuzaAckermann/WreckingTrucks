using System;
using System.Collections.Generic;

public class Level
{
    private readonly List<Row> _rows = new List<Row>();

    public Level(List<Row> rows)
    {
        _rows = rows ?? throw new ArgumentNullException(nameof(rows));
    }

    public IReadOnlyList<Row> Rows => _rows;
}