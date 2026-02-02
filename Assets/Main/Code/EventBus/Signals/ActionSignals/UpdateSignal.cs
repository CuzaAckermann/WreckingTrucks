using System;

public class UpdateSignal : EventBusSignal
{
    private readonly float _deltaTime;

    public UpdateSignal(float deltaTime)
    {
        _deltaTime = deltaTime >= 0 ? deltaTime : throw new ArgumentOutOfRangeException(nameof(deltaTime));
    }

    public float DeltaTime => _deltaTime;
}