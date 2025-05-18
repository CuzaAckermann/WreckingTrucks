using System;
using System.Collections.Generic;

public class Row
{
    private List<BlockType> _blocksTypes;

    public Row(List<BlockType> blocksTypes)
    {
        _blocksTypes = blocksTypes ?? throw new ArgumentNullException(nameof(blocksTypes));
    }

    public IReadOnlyList<BlockType> Blocks => _blocksTypes;
}