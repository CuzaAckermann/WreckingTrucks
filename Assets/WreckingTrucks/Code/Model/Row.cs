using System;
using System.Collections.Generic;

public class Row // »«ћ≈Ќ»“№, в нем пока нет особого смысла
{
    private List<Type> _models;

    public Row(List<Type> models)
    {
        _models = models ?? throw new ArgumentNullException(nameof(models));
    }

    public IReadOnlyList<Type> Models => _models;
}