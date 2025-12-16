using System;

public class BlockPresenterShakerCreator : ITickableCreator
{
    public BlockPresenterShakerCreator()
    {

    }

    public event Action<ITickable> TickableCreated;

    public BlockPresenterShaker Create()
    {
        BlockPresenterShaker blockPresenterShaker = new BlockPresenterShaker(100);

        TickableCreated?.Invoke(blockPresenterShaker);

        return blockPresenterShaker;
    }
}