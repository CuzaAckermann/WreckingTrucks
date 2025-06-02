using System;
using System.Collections.Generic;

public class LevelSettings
{
    private readonly List<Row> _rows = new List<Row>();

    public LevelSettings(List<Row> rows)
    {
        _rows = rows ?? throw new ArgumentNullException(nameof(rows));
    }

    public IReadOnlyList<Row> Rows => _rows;
}