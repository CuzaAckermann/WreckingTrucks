using UnityEngine;

public class ZigZagFiller : FillingStrategy
{
    private int _currentColumn;
    private bool _isMovingRight = true;

    public ZigZagFiller(IFillable field,
                        Vector3 sourceFilling,
                        float frequency, 
                        bool startFromLeft = true) 
                 : base(field,
                        sourceFilling,
                        frequency)
    {
        _isMovingRight = startFromLeft;
        _currentColumn = startFromLeft ? 0 : field.Width - 1;
    }

    protected override void Fill(FillingCard<Model> fillingCard)
    {
        RecordModelToPosition<Model> record = FindRecordForColumn(fillingCard);
        
        if (record != null)
        {
            record.Model.SetStartPosition(SourceFilling);
            Field.PlaceModel(record.Model, record.LocalX, record.LocalY);
            fillingCard.RemoveRecord(record);
        }

        UpdateCurrentColumn();
    }

    private RecordModelToPosition<Model> FindRecordForColumn(FillingCard<Model> card)
    {
        for (int i = 0; i < card.Amount; i++)
        {
            RecordModelToPosition<Model> record = card.GetRecord(i);

            if (record.LocalX == _currentColumn)
            {
                return record;
            }
        }

        return null;
    }

    private void UpdateCurrentColumn()
    {
        if (_isMovingRight)
        {
            _currentColumn++;

            if (_currentColumn >= Field.Width)
            {
                _currentColumn = Field.Width - 1;
                _isMovingRight = false;
            }
        }
        else
        {
            _currentColumn--;

            if (_currentColumn < 0)
            {
                _currentColumn = 0;
                _isMovingRight = true;
            }
        }
    }
}