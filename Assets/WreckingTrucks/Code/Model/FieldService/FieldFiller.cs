using System;

public abstract class FieldFiller<T> : IClearable, IResetable where T : Model
{
    public abstract void Reset();

    public abstract void Clear();
}