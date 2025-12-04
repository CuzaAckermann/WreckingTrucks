using System;

public class BlockPresenterShakerCreator : ITickableCreator
{
    public BlockPresenterShakerCreator()
    {

    }

    public event Action<ITickable> StopwatchCreated;

    public BlockPresenterShaker Create()
    {
        BlockPresenterShaker blockPresenterShaker = new BlockPresenterShaker(100);

        StopwatchCreated?.Invoke(blockPresenterShaker);

        return blockPresenterShaker;
    }
}