using System;

public class RecordModelToPosition<T>
{
    public RecordModelToPosition(T model, int localY, int localX)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
        LocalY = localY;
        LocalX = localX;
    }

    public T Model { get; private set; }

    public int LocalY { get; private set; }

    public int LocalX { get; private set; }
}