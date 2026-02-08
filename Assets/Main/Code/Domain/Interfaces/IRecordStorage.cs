using System;
using System.Collections.Generic;

public interface IRecordStorage
{
    public event Action RecordAppeared;

    public int Amount { get; }

    public void Clear();

    public IReadOnlyList<ColorType> GetUniqueStoredColors();

    public bool TryGetNextRecord(out RecordPlaceableModel record);
}