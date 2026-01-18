using System;

public class CreatedSignal<T> where T : class
{
    private readonly T _creatable;

    public CreatedSignal(T creatable)
    {
        _creatable = creatable ?? throw new ArgumentNullException(nameof(creatable));
    }

    public T Creatable => _creatable;
}