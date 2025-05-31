using System;
using System.Collections.Generic;

public class Row // ��������, � ��� ���� ��� ������� ������
{
    private List<Type> _models;

    public Row(List<Type> models)
    {
        _models = models ?? throw new ArgumentNullException(nameof(models));
    }

    public IReadOnlyList<Type> Models => _models;
}