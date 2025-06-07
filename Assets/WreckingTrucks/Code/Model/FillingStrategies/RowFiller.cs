using System.Collections.Generic;
using UnityEngine;

public class RowFiller : FillingStrategy
{
    public RowFiller(IFillable field, float frequency)
              : base(field, frequency)
    {

    }

    protected override void Fill(IFillable field, Queue<Model> models)
    {
        int columnsToFill = Mathf.Min(field.AmountColumns, models.Count);

        for (int i = 0; i < columnsToFill; i++)
        {
            field.PlaceModel(models.Dequeue(), i);
        }
    }
}