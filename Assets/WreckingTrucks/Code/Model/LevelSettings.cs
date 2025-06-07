using System;
using System.Collections.Generic;

public class LevelSettings
{
    private readonly List<Row> _rowsWithBlocks = new List<Row>();
    private readonly List<Row> _rowsWithTrucks = new List<Row>();

    public LevelSettings(List<Row> rowsWithBlocks, List<Row> rowsWithTrucks)
    {
        _rowsWithBlocks = rowsWithBlocks ?? throw new ArgumentNullException(nameof(rowsWithBlocks));
        _rowsWithTrucks = rowsWithTrucks ?? throw new ArgumentNullException(nameof(rowsWithTrucks));
    }

    public int WidthBlocksField => _rowsWithBlocks[0].Models.Count;

    public int LengthBlocksField => _rowsWithBlocks.Count;

    public int WidthTrucksField => _rowsWithTrucks[0].Models.Count;

    public int LengthTrucksField => _rowsWithTrucks.Count;

    public IReadOnlyList<Row> RowsWithBlocks => _rowsWithBlocks;

    public IReadOnlyList<Row> RowsWithTrucks => _rowsWithTrucks;
}