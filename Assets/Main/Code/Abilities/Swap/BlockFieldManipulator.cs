using System;

public class BlockFieldManipulator
{
    private readonly int _amountShiftedRows;

    private Field _blockField;

    public BlockFieldManipulator(int amountShiftedRows)
    {
        if (amountShiftedRows <= 0)
        {
            throw new ArgumentNullException(nameof(amountShiftedRows));
        }

        _amountShiftedRows = amountShiftedRows;
    }

    public void SetField(Field blockField)
    {
        _blockField = blockField ?? throw new ArgumentNullException(nameof(blockField));
    }

    public void OpenField()
    {
        _blockField.ShowRows(_amountShiftedRows);
    }

    public void CloseField()
    {
        _blockField.HideRows();
    }
}