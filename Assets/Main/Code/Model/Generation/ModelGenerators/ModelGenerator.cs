using System;
using System.Collections.Generic;

public abstract class ModelGenerator<M> : IRecordStorage where M : Model
{
    private readonly ModelFactory<M> _modelFactory;
    private readonly ColorGenerator _colorGenerator;

    private readonly Queue<RecordPlaceableModel> _records;

    private IFillable _fillable;

    public ModelGenerator(ModelFactory<M> modelFactory,
                          ColorGenerator colorGenerator)
    {
        _modelFactory = modelFactory ?? throw new ArgumentNullException(nameof(modelFactory));
        _colorGenerator = colorGenerator ?? throw new ArgumentNullException(nameof(colorGenerator));

        _records = new Queue<RecordPlaceableModel>();
    }

    public event Action RecordAppeared;

    public int Amount => _records.Count;

    public void PrepareRecords(IFillable fillable)
    {
        _fillable = fillable ?? throw new ArgumentNullException(nameof(fillable));

        for (int layer = 0; layer < fillable.AmountLayers; layer++)
        {
            for (int row = 0; row < fillable.AmountRows; row++)
            {
                for (int column = 0; column < fillable.AmountColumns; column++)
                {
                    _records.Enqueue(new RecordPlaceableModel(Generate(),
                                                              layer,
                                                              column,
                                                              row));
                }
            }
        }
    }

    public void Clear()
    {
        Logger.Log("Method is empty");
    }

    public IReadOnlyList<ColorType> GetUniqueStoredColors()
    {
        return _colorGenerator.GetGeneratedColors();
    }

    public M Generate()
    {
        M model = _modelFactory.Create();
        ColorType colorType = _colorGenerator.GenerateEvenly();
        model.SetColor(colorType);

        return model;
    }

    public void SetColorTypes(IReadOnlyList<ColorType> colorTypes)
    {
        _colorGenerator.SetColorTypes(colorTypes);
    }

    public bool TryGetNextRecord(out RecordPlaceableModel record)
    {
        if (_records.Count == 0)
        {
            record = null;
            return false;
        }

        record = _records.Dequeue();

        return record != null;
    }

    public void AddRecord(int indexOfLayer, int indexOfColumn)
    {
        bool isEmpty = _records.Count == 0;

        _records.Enqueue(new RecordPlaceableModel(Generate(),
                                                  indexOfLayer,
                                                  indexOfColumn,
                                                  _fillable.GetAmountModelsInColumn(indexOfLayer, indexOfColumn)));

        if (isEmpty)
        {
            RecordAppeared?.Invoke();
        }
    }
}