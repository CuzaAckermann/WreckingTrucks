public class PresenterCreatedSignal<P> : CreatedSignal<P> where P : Presenter
{
    public PresenterCreatedSignal(P presenter) : base(presenter)
    {

    }
}