using System;
using System.Collections.Generic;

public class Row // »«ћ≈Ќ»“№, в нем пока нет особого смысла
{
    private List<Block> _blocks;

    public Row(List<Block> blocks)
    {
        _blocks = blocks ?? throw new ArgumentNullException(nameof(blocks));
    }

    public IReadOnlyList<Block> Blocks => _blocks;
}