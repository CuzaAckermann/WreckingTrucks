using System;

public class ButtonWithIndex : BaseButton
{
    private int _index;

    public event Action<int> Pressed;

    public void Initailize(int index)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        _index = index;
    }

    protected override void OnPressed()
    {
        Pressed?.Invoke(_index);
    }
}