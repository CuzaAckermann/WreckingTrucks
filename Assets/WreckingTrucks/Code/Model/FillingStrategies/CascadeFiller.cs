using UnityEngine;

public class CascadeFiller : FillingStrategy
{
    public CascadeFiller(IFillable field, Vector3 sourceFilling, float frequency)
                  : base(field, sourceFilling, frequency)
    {

    }

    protected override void Fill(FillingCard<Model> fillingCard)
    {
        RecordModelToPosition<Model> record = fillingCard.GetFirstRecord();

        record.Model.SetStartPosition(SourceFilling);
        Field.PlaceModel(record.Model, record.LocalX, record.LocalY);
        fillingCard.RemoveRecord(record);
    }
}
