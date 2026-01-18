public class BlockPresenterFactory : PresenterFactory<BlockPresenter>
{
    public override Presenter CreatePresenter()
    {
        BlockPresenter blockPresenter = Create();

        blockPresenter.SetEventBus(EventBus);

        return blockPresenter;
    }
}