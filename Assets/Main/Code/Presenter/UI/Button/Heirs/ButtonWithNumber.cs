using System;
using UnityEngine;
using TMPro;

public class ButtonWithNumber : BaseButton
{
    [SerializeField] private TMP_Text _text;

    public event Action<int> Pressed;

    public int Number { get; private set; } = 0;

    public void SetNumber(int number)
    {
        if (Number != 0)
        {
            throw new InvalidOperationException($"{nameof(number)} has already been assigned");
        }

        if (number <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(number));
        }

        Number = number;

        _text.text = Number.ToString();
    }

    protected override void OnPressed()
    {
        Pressed?.Invoke(Number);
    }
}