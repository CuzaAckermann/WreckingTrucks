using System;
using System.Collections.Generic;

public abstract class FillingStrategy
{
    private readonly IFillable _field;
    private Queue<Model> _modelsQueue;

    public event Action FillingCompleted;

    public FillingStrategy(IFillable field, float frequency)
    {
        _field = field ?? throw new ArgumentNullException(nameof(field));
        Frequency = frequency > 0 ? frequency : throw new ArgumentOutOfRangeException(nameof(frequency));
    }

    public float Frequency { get; private set; }

    public void FillStep()
    {
        Fill(_field, _modelsQueue);

        if (_modelsQueue.Count == 0)
        {
            OnFillingCompleted();
        }
    }

    public void SetQueueModels(Queue<Model> models)
    {
        _modelsQueue = models ?? throw new ArgumentNullException(nameof(models));
    }

    public void Clear()
    {
        _modelsQueue.Clear();
    }

    protected abstract void Fill(IFillable field, Queue<Model> models);

    private void OnFillingCompleted()
    {
        FillingCompleted?.Invoke();
    }
}