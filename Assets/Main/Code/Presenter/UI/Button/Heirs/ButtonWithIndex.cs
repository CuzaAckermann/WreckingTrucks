using System;
using UnityEngine;
using TMPro;

public class ButtonWithIndex : BaseUiButton
{
    [SerializeField] private TMP_Text _text;

    private const int UnassignedValue = -1;

    public event Action<int> Pressed;

    public int Index { get; private set; } = UnassignedValue;

    public void SetIndex(int index)
    {
        if (Index != UnassignedValue)
        {
            throw new InvalidOperationException($"{nameof(index)} has already been assigned");
        }

        if (index <= UnassignedValue)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        Index = index;

        _text.text = (Index + 1).ToString();
    }

    protected override void OnPressed()
    {
        Pressed?.Invoke(Index);
    }
}