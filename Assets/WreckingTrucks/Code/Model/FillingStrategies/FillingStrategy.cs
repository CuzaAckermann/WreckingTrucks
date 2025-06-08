using System;
using UnityEngine;

public abstract class FillingStrategy
{
    protected readonly IFillable Field;
    protected Vector3 SourceFilling;

    private FillingCard<Model> _fillingCard;

    public FillingStrategy(IFillable field, Vector3 sourceFilling, float frequency)
    {
        Field = field ?? throw new ArgumentNullException(nameof(field));
        SourceFilling = sourceFilling;
        Frequency = frequency > 0 ? frequency : throw new ArgumentOutOfRangeException(nameof(frequency));
    }

    public event Action FillingCompleted;

    public float Frequency { get; private set; }

    public void SetFillingCard(FillingCard<Model> fillingCard)
    {
        _fillingCard = fillingCard ?? throw new ArgumentNullException(nameof(fillingCard));
    }

    public void FillStep()
    {
        if (_fillingCard == null || _fillingCard.Amount == 0)
            return;

        Fill(_fillingCard);

        if (_fillingCard.Amount == 0)
        {
            OnFillingCompleted();
        }
    }

    public virtual void Clear()
    {
        _fillingCard?.Clear();
    }

    protected abstract void Fill(FillingCard<Model> fillingCard);

    protected virtual void OnFillingCompleted()
    {
        FillingCompleted?.Invoke();
    }
}