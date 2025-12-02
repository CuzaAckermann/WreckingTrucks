using System;
using System.Collections.Generic;

public class FillingCard
{
    private readonly List<RecordPlaceableModel> _records;

    public FillingCard(int amountLayers, int amountColumns, int amountRows)
    {
        if (amountLayers <= 0)
        {
            throw new ArgumentNullException(nameof(amountLayers));
        }

        if (amountColumns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountColumns));
        }

        if (amountRows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amountRows));
        }

        AmountLayers = amountLayers;
        AmountColumns = amountColumns;
        AmountRows = amountRows;
        _records = new List<RecordPlaceableModel>();
    }

    public int AmountLayers { get; private set; }

    public int AmountColumns { get; private set; }

    public int AmountRows { get; private set; }

    public int Amount => _records.Count;

    public IReadOnlyList<RecordPlaceableModel> Records => _records;

    public void Clear()
    {
        _records.Clear();
    }

    public void Add(RecordPlaceableModel record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        if (record.IndexOfLayer < 0 || record.IndexOfLayer >= AmountLayers)
        {
            throw new ArgumentOutOfRangeException(nameof(record.IndexOfLayer));
        }

        if (record.IndexOfColumn < 0 || record.IndexOfColumn >= AmountColumns)
        {
            throw new ArgumentOutOfRangeException(nameof(record.IndexOfColumn));
        }

        //if (record.IndexOfRow < 0 || record.IndexOfRow >= AmountRows)
        //{
        //    throw new ArgumentOutOfRangeException(nameof(record.IndexOfRow));
        //}

        _records.Add(record);
    }

    public void AddRange(IEnumerable<RecordPlaceableModel> records)
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

    public RecordPlaceableModel GetFirstRecord()
    {
        return GetRecord(0);
    }

    public RecordPlaceableModel GetRecord(int index)
    {
        if (index < 0 || index >= _records.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return _records[index];
    }

    public void RemoveRecord(RecordPlaceableModel record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        _records.Remove(record);
    }
}