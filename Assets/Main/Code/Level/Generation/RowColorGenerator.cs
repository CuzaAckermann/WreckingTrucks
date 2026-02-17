using System;
using System.Collections.Generic;

public class RowColorGenerator : IRecordStorage
{
    private readonly List<RowGenerationStrategy> _rowGenerationStrategies;
    private readonly List<ColorType> _generatedColors;
    private readonly Random _random;

    private readonly Queue<RecordPlaceableModel> _records;

    private readonly int _amountLayers;
    private readonly int _amountInRow;

    public RowColorGenerator(List<RowGenerationStrategy> rowGenerationStrategies,
                             List<ColorType> colorTypes,
                             int amountLayers,
                             int amountInRow)
    {
        _rowGenerationStrategies = rowGenerationStrategies ?? throw new ArgumentNullException(nameof(rowGenerationStrategies));
        _generatedColors = colorTypes ?? throw new ArgumentNullException(nameof(colorTypes));
        _random = new Random();

        _records = new Queue<RecordPlaceableModel>();

        _amountLayers = amountLayers > 0 ? amountLayers : throw new ArgumentOutOfRangeException(nameof(amountLayers));
        _amountInRow = amountInRow > 0 ? amountInRow : throw new ArgumentOutOfRangeException(nameof(amountInRow));
    }

    public event Action RecordAppeared;

    public int Amount => _records.Count;

    public void Clear()
    {
        Logger.Log("Method is empty");
    }

    public IReadOnlyList<ColorType> GetUniqueStoredColors()
    {
        return _generatedColors;
    }

    public bool TryGetNextRecord(out RecordPlaceableModel record)
    {
        if (_records.Count == 0)
        {
            GenerateRecords();
        }

        record = _records.Dequeue();

        return record != null;
    }

    private void GenerateRecords()
    {
        List<RecordPlaceableModel> records = new List<RecordPlaceableModel>();

        for (int layer = 0; layer < _amountLayers; layer++)
        {
            List<ColorType> colorTypes = Generate(_amountInRow);

            for (int column = 0; column < _amountInRow; column++)
            {
                records.Add(new RecordPlaceableModel(colorTypes[column],
                                                     layer,
                                                     column,
                                                     0));
            }
        }

        bool isEmpty = _records.Count == 0;

        for (int record = 0; record < records.Count; record++)
        {
            _records.Enqueue(records[record]);
        }

        if (isEmpty)
        {
            RecordAppeared?.Invoke();
        }
    }

    private List<ColorType> Generate(int amount)
    {
        return _rowGenerationStrategies[_random.Next(0, _rowGenerationStrategies.Count)].Generate(_generatedColors, amount);
    }
}