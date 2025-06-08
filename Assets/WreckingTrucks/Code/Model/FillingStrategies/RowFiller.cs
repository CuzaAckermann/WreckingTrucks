using UnityEngine;

public class RowFiller : FillingStrategy
{
    public RowFiller(IFillable field, Vector3 sourceFilling, float frequency)
              : base(field, sourceFilling, frequency)
    {

    }

    protected override void Fill(FillingCard<Model> fillingCard)
    {
        int columnsToFill = Mathf.Min(Field.Width, fillingCard.Amount);

        for (int i = 0; i < columnsToFill; i++)
        {
            RecordModelToPosition<Model> record = fillingCard.GetFirstRecord();
            PlaceModel(record);
            fillingCard.RemoveRecord(record);
        }
    }

    private void PlaceModel(RecordModelToPosition<Model> record)
    {
        record.Model.SetStartPosition(SourceFilling);
        Field.PlaceModel(record.Model, record.LocalX, record.LocalY);
    }
}