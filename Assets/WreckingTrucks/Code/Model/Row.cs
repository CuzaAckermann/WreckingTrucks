using System.Collections.Generic;

public class Row
{
    private List<Block> _blocks;

    public Row(List<Block> blocks)
    {
        _blocks = blocks;
    }

    public IReadOnlyList<Block> Blocks => _blocks;
}