using System;

public class RecordPlaceableModel
{
    public RecordPlaceableModel(ColorType color,
                                int indexOfLayer,
                                int indexOfColumn,
                                int indexOfRow)
    {
        Color = color != ColorType.Unknown ? color : throw new ArgumentException($"{nameof(ColorType)} is {nameof(ColorType.Unknown)}");
        IndexOfLayer = indexOfLayer >= 0 ? indexOfLayer : throw new ArgumentOutOfRangeException(nameof(indexOfLayer));
        IndexOfColumn = indexOfColumn >= 0 ? indexOfColumn : throw new ArgumentOutOfRangeException(nameof(indexOfColumn));
        IndexOfRow = indexOfRow >= 0 ? indexOfRow : throw new ArgumentOutOfRangeException(nameof(indexOfRow));
    }

    public ColorType Color { get; private set; }

    public int IndexOfLayer { get; private set; }

    public int IndexOfColumn { get; private set; }

    public int IndexOfRow { get; private set; }
}