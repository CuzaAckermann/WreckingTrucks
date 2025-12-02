using System;

public class BlockPresenterShakerCreator
{
    private readonly TickEngine _tickEngine;

    public BlockPresenterShakerCreator(TickEngine tickEngine)
    {
        _tickEngine = tickEngine ?? throw new ArgumentNullException(nameof(tickEngine));
    }

    public BlockPresenterShaker Create()
    {
        return new BlockPresenterShaker(_tickEngine, 100);
    }
}