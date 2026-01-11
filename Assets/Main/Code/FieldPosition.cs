using System;

public struct FieldPosition
{
    public FieldPosition(int indexOfLayer, int indexOfColumn, int indexOfRow)
    {
        IndexOfLayer = indexOfLayer >= 0 ? indexOfLayer : throw new ArgumentOutOfRangeException(nameof(indexOfLayer));
        IndexOfColumn = indexOfColumn >= 0 ? indexOfColumn : throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        IndexOfRow = indexOfRow >= 0 ? indexOfRow : throw new ArgumentOutOfRangeException(nameof(indexOfRow));
    }

    public int IndexOfLayer { get; private set; }

    public int IndexOfColumn { get; private set; }

    public int IndexOfRow { get; private set; }
}