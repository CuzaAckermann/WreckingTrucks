using System;
using System.Collections.Generic;

public class FillingCard : IRecordStorage
{
    private readonly List<RecordPlaceableModel> _records;
    private readonly List<ColorType> _uniqueColors;

    private int _amountLayers;
    private int _amountColumns;
    private int _amountRows;

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

        _amountLayers = amountLayers;
        _amountColumns = amountColumns;
        _amountRows = amountRows;

        _records = new List<RecordPlaceableModel>();

        _uniqueColors = new List<ColorType>();
    }

    public event Action RecordAppeared;

    public int Amount => _records.Count;

    public IReadOnlyList<ColorType> GetUniqueStoredColors()
    {
        return _uniqueColors;
    }

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

        if (record.IndexOfLayer < 0 || record.IndexOfLayer >= _amountLayers)
        {
            throw new ArgumentOutOfRangeException(nameof(record.IndexOfLayer));
        }

        if (record.IndexOfColumn < 0 || record.IndexOfColumn >= _amountColumns)
        {
            throw new ArgumentOutOfRangeException(nameof(record.IndexOfColumn));
        }

        if (record.IndexOfRow < 0 || record.IndexOfRow >= _amountRows)
        {
            throw new ArgumentOutOfRangeException(nameof(record.IndexOfRow));
        }

        if (_uniqueColors.Contains(record.Color) == false)
        {
            _uniqueColors.Add(record.Color);
        }

        bool isEmpty = _records.Count == 0;

        _records.Add(record);

        if (isEmpty)
        {
            RecordAppeared?.Invoke();
        }
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

    public bool TryGetNextRecord(out RecordPlaceableModel record)
    {
        if (_records.Count > 0)
        {
            record = _records[0];
            _records.RemoveAt(0);

            return true;
        }

        record = null;

        return false;
    }

    public bool TryGetFirstRecord(out RecordPlaceableModel record)
    {
        return TryGetRecord(0, out record);
    }

    public bool TryGetRecord(int index, out RecordPlaceableModel record)
    {
        if (index >= 0 && index < _records.Count)
        {
            record = _records[index];

            return true;
        }

        record = null;

        return false;
    }
}