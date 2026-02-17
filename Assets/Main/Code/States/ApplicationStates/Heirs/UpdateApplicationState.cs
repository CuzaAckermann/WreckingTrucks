using System;

public class UpdateApplicationState
{
    public event Action Updated;

    public void Update()
    {
        Updated?.Invoke();
    }
}