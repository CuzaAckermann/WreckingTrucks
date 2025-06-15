using System;

public class RecordModelToPosition<T>
{
    public RecordModelToPosition(T model, int numberOfRow, int numberOfColumn)
    {
        PlaceableModel = model ?? throw new ArgumentNullException(nameof(model));
        NumberOfRow = numberOfRow;
        NumberOfColumn = numberOfColumn;
    }

    public T PlaceableModel { get; private set; }

    public int NumberOfRow { get; private set; }

    public int NumberOfColumn { get; private set; }
}