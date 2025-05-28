using System;
using System.Collections.Generic;

public class Row // ��������, � ��� ���� ��� ������� ������
{
    private List<Type> _blocks;

    public Row(List<Type> blocks)
    {
        _blocks = blocks ?? throw new ArgumentNullException(nameof(blocks));
    }

    public IReadOnlyList<Type> Blocks => _blocks;
}