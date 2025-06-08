using System;
using System.Collections.Generic;

public class FillingCard<T>
{
    private readonly RecordModelToPosition<T>[,] _records;
    private readonly List<RecordModelToPosition<T>> _recordsList = new List<RecordModelToPosition<T>>();

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
        _records = new RecordModelToPosition<T>[width, length];
    }

    public int Length { get; private set; }

    public int Width { get; private set; }

    public int Amount => _recordsList.Count;

    public void Add(RecordModelToPosition<T> record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        if (record.LocalX < 0 || record.LocalX >= Width)
        {
            throw new ArgumentOutOfRangeException(nameof(record.LocalX));
        }

        if (record.LocalY < 0 || record.LocalY >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(record.LocalY));
        }

        _records[record.LocalX, record.LocalY] = record;
        _recordsList.Add(record);
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

    public RecordModelToPosition<T> GetRecord(int x, int y)
    {
        if (x < 0 || x >= Width)
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }

        if (y < 0 || y >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        return _records[x, y];
    }

    public RecordModelToPosition<T> GetFirstRecord()
    {
        return _recordsList[0];
    }

    public RecordModelToPosition<T> GetRecord(int index)
    {
        if (index < 0 || index >= _recordsList.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _recordsList[index];
    }

    public void RemoveRecord(RecordModelToPosition<T> record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        _records[record.LocalX, record.LocalY] = null;
        _recordsList.Remove(record);
    }

    public void Clear()
    {
        Array.Clear(_records, 0, _records.Length);
        _recordsList.Clear();
    }
}