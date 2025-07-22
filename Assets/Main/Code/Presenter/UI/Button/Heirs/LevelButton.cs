using System;

public class LevelButton : BaseButton
{
    private int _indexOfLevel;

    public event Action<int> Pressed;

    public void Initailize(int indexOfLevel)
    {
        if (indexOfLevel < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(indexOfLevel));
        }

        _indexOfLevel = indexOfLevel;
    }

    protected override void OnPressed()
    {
        Pressed?.Invoke(_indexOfLevel);
    }
}