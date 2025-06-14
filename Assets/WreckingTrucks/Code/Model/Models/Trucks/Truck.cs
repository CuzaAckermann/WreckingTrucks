using System;

public abstract class Truck : Model
{
    private Type _destroyableType;

    public Truck(/*Gun gun*/)
    {
        //Gun = gun ?? throw new ArgumentNullException(nameof(gun));
    }

    public event Action<Truck> CurrentPositionReached;

    public Gun Gun { get; private set; }

    public void SetDestroyableType<T>() where T : Block
    {
        _destroyableType = typeof(T);
    }

    public override void FinishMovement()
    {
        base.FinishMovement();
        CurrentPositionReached?.Invoke(this);
    }
}