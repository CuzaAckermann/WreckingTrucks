using System.Collections.Generic;
using UnityEngine;

public class ZigZagFiller : FillingStrategy
{
    private int _currentColumn;
    private bool _isMovingRight = true;

    public ZigZagFiller(IFillable field, float frequency)
                 : base(field, frequency)
    {
        _currentColumn = 0;
    }

    protected override void Fill(IFillable field, Queue<Model> models)
    {
        field.PlaceModel(models.Dequeue(), _currentColumn);
        UpdateColumnIndex(field.AmountColumns);
    }

    private void UpdateColumnIndex(int amountColumns)
    {
        if (_isMovingRight)
        {
            _currentColumn++;

            if (_currentColumn >= amountColumns)
            {
                _currentColumn = Mathf.Max(0, amountColumns - 1);
                _isMovingRight = false;
            }
        }
        else
        {
            _currentColumn--;

            if (_currentColumn < 0)
            {
                _currentColumn = Mathf.Min(0, amountColumns - 1);
                _isMovingRight = true;
            }
        }
    }
}