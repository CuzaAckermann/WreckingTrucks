using System;
using System.Collections.Generic;

public class FillingCard<T>
{
    private readonly List<RecordModelToPosition<T>> _records;

    public FillingCard(int length, int width)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        Length = length;
        Width = width;
        _records = new List<RecordModelToPosition<T>>();
    }

    public int Length { get; private set; }

    public int Width { get; private set; }

    public int Amount => _records.Count;

    public void Clear()
    {
        _records.Clear();
    }

    public void Add(RecordModelToPosition<T> record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        if (record.NumberOfColumn < 0 || record.NumberOfColumn >= Width)
        {
            throw new ArgumentOutOfRangeException(nameof(record.NumberOfColumn));
        }

        if (record.NumberOfRow < 0 || record.NumberOfRow >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(record.NumberOfRow));
        }

        _records.Add(record);
    }

    public void AddRange(IEnumerable<RecordModelToPosition<T>> records)
    {
        if (records == null)
        {
            throw new ArgumentNullException(nameof(records));
        }

        foreach (var record in records)
        {
            Add(record);
        }
    }

    public RecordModelToPosition<T> GetFirstRecord()
    {
        return GetRecord(0);
    }

    public RecordModelToPosition<T> GetRecord(int index)
    {
        if (index < 0 || index >= _records.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _records[index];
    }

    public void RemoveRecord(RecordModelToPosition<T> record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        _records.Remove(record);
    }
}