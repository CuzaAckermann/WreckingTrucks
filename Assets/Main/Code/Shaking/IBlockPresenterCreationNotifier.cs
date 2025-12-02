using System;

public interface IBlockPresenterCreationNotifier
{
    public event Action<BlockPresenter> BlockPresenterCreated;
}